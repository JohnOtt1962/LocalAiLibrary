using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.Extensions.Options;
using Utilities.Logger;

namespace LocalAiLibrary.AiLibrary.Orchestration.Helpers
{
    public class OrchestrationHelper(IChatSessionService chatSessionService, IUtilLogger logger, IOptions<AiConfig> config)
    {
        private readonly AiConfig _config = config.Value;

        public OrchestrationPlan? SaveOrchestrationPlan(ChatCompletionResponse? response)
        {
            OrchestrationPlan? orchestrationPlan = null;
            var functionArguments = response?.Choices[0].Message.ToolCalls?[0].Function.Arguments;

            if (functionArguments != null)
            {
                orchestrationPlan = GetOrchestrationPlan(functionArguments);
                if (orchestrationPlan != null)
                    chatSessionService.SaveOrchestration(orchestrationPlan);
            }

            return orchestrationPlan;
        }

        private OrchestrationPlan? GetOrchestrationPlan(string functionArguments)
        {
            OrchestrationPlan? orchestrationPlan = null;

            try
            {
                orchestrationPlan = DeserializerHelper.DeserializeItem<OrchestrationPlan>(functionArguments);

                if (orchestrationPlan != null)
                {
                    orchestrationPlan.Id = chatSessionService.GetServiceName() + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-ss");

                    for (int i = 0; i <= orchestrationPlan.Items.Count - 1; i++)
                    {
                        if (orchestrationPlan.Items[i].CallIndex == 0)
                            orchestrationPlan.Items[i].CallIndex = i + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return orchestrationPlan;
        }
    }
}