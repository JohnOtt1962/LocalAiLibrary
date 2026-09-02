using System.Text.Json;
using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public class SchemaTool(IRepo repo, IOptions<AiConfig> config) : ISchemaTool
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public string GetSchema(ToolCommand command)
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

            EntityOps ops = _repoHelper.GetEntityOps("GetPersonAddressSchema", _config.AdventureWorksConnectionString);

            var result = repo.GetCollection(ops);
            string jsonResult = JsonSerializer.Serialize(result);

            return jsonResult;
        }
    }
}