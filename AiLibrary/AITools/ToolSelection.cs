using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Orchestration;
using Microsoft.Extensions.Options;
using Utilities.Email.EmailOperations;
using Utilities.Logger;
using Utilities.Repo;

namespace LocalAiLibrary.AiLibrary.AITools
{
    public  class ToolSelection(IChatSessionService chatSessionService,
        IOptions<AiConfig> config, IEmail aiEmail, IRepo repo, 
        IOrchestrationPlanManagement orchestrationPlanManagement, IUtilLogger logger) : IToolSelection
    {
        private readonly ToolManager _toolManager = new(aiEmail, repo, config, logger);
        private readonly AiConfig _config = config.Value;

        public async Task<string> ProcessToolCalls(ChatCompletionResponse response)
        {
            string returnMessage = string.Empty;
            List<ToolCommand> commands = _toolManager.GetToolCommands(response);

            foreach (var command in commands)
            {
                if (command.ToolName == "send_email")
                {
                    returnMessage = await _toolManager.SendEmail(command);
                    returnMessage = orchestrationPlanManagement.CacheToolResponse(returnMessage, false);
                }
                else if (command.ToolName == "get_person_address_schema")
                {
                    returnMessage = _toolManager.GetSchema(command, _config.PersonSchemaFilter);
                }
                else if (command.ToolName == "get_sales_schema")
                {
                    returnMessage = _toolManager.GetSchema(command, _config.SalesSchemaFilter);
                }
                else if (command.ToolName == "get_human_resources_schema")
                {
                    returnMessage = _toolManager.GetSchema(command, _config.HumanResourcesSchemaFilter);
                }
                else if (command.ToolName == "read_database")
                {
                    returnMessage = await _toolManager.ReadDatabase(command);
                    returnMessage = orchestrationPlanManagement.CacheToolResponse(returnMessage, true);
                }
                else if (command.ToolName == "update_database")
                {
                    returnMessage = await _toolManager.UpdateDatabase(command);
                    returnMessage = orchestrationPlanManagement.CacheToolResponse(returnMessage, false);
                }
            }

            return returnMessage;
        }
    }
}