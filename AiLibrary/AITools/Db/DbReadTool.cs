using System.Text.Json;
using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using Microsoft.Extensions.Options;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public class DbReadTool(IRepo repo, IOptions<AiConfig> config) : IDbReadTool
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public async Task<string> ReadDatabase(ToolCommand command)
        {
            string returnMessage = string.Empty;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SchemaCommandModel schemaModel = new SchemaCommandModel
            {
                ToolName = command.ToolName,
                Args = DeserializerHelper.DeserializeItem<SchemaToolArgs>(command.ToolArgs, options)!
            };

            EntityOps ops = _repoHelper.GetEntityOps(schemaModel.Args.Sql, _config.AdventureWorksConnectionString);
            ops.IsStoredProc = false;

            var result = await repo.GetCollection(ops);

            string jsonResult = JsonSerializer.Serialize(result);

            CollectionResponse response = new CollectionResponse
            {
                Prompt = "Present the results as an HTML table. Make the heading gray. Do not use markdown.",
                Collection = jsonResult
            };

            var jsonString = JsonSerializer.Serialize(response);

            return jsonResult;
        }
    }
}