using System.Text.Json;
using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public class DbReadTool(IRepo repo, IOptions<AiConfig> config) : IDbReadTool
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public string ReadDatabase(ToolCommand command)
        {
            string returnMessage = string.Empty;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SchemaCommandModel schemaModel = new SchemaCommandModel
            {
                ToolName = command.ToolName,
                Args = JsonSerializer.Deserialize<SchemaToolArgs>(command.ToolArgs, options)!
            };

            EntityOps ops = _repoHelper.GetEntityOps(schemaModel.Args.Sql, _config.AdventureWorksConnectionString);
            ops.IsStoredProc = false;

            var result = repo.GetCollection(ops);

            string jsonResult = JsonSerializer.Serialize(result);

            CollectionResponse response = new CollectionResponse
            {
                Prompt = "Please present the below in an HTML table as your reponse. Ensure that the contents in the table do not wrap, and make the background color of the header row gray.",
                Collection = jsonResult
            };

            var jsonString = JsonSerializer.Serialize(response);

            return jsonString;
        }
    }
}