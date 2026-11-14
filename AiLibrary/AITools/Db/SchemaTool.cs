using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public class SchemaTool(IRepo repo, IOptions<AiConfig> config, IChatSessionService chatSessionService) : ISchemaTool
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public string GetSchema(ToolCommand command, string filter)
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

            string connectionString = ToolConfigReconciliation.GetConnectionString(chatSessionService, _config);

            EntityOps ops = _repoHelper.GetEntityOps("GetSchemaByDomain", connectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("Filter", filter, false, "varchar", 1),
            };

            var result = repo.GetCollection(ops);
            string jsonResult = JsonSerializer.Serialize(result);

            return jsonResult;
        }

        public string GetGemmaSchema(ToolCommand command)
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

            EntityOps ops = _repoHelper.GetEntityOps("GetSentimentSchema", _config.ConnectionString);

            var result = repo.GetCollection(ops);
            string jsonResult = JsonSerializer.Serialize(result);

            return jsonResult;
        }
    }
}