using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using LocalAiLibrary.AiLibrary.Helpers;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public class DbUpdateTool(IRepo repo, IOptions<AiConfig> config) : IDbUpdateTool
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public async Task<string> UpdateDatabase(ToolCommand command)
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

            await repo.Update(ops);

            CollectionResponse response = new CollectionResponse
            {
                Prompt = "provide a mildly agitated declaration of mission completion",
                Collection = null
            };

            var jsonString = JsonSerializer.Serialize(response);

            return jsonString;
        }
    }
}