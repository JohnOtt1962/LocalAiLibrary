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
    public class DbInsertTool(IRepo repo, IOptions<AiConfig> config, IChatSessionService chatSessionService) : IDbInsertTool
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public async Task InsertDatabase(ToolCommand command)
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

            EntityOps ops = _repoHelper.GetEntityOps(schemaModel.Args.Sql, connectionString);
            ops.IsStoredProc = false;

            await repo.Insert(ops);
        }
    }
}