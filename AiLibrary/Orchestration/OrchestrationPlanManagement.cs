using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities.Logger;

namespace LocalAiLibrary.AiLibrary.Orchestration
{
    public class OrchestrationPlanManagement(IChatSessionService chatSessionService, IUtilLogger logger, IOptions<AiConfig> config) : IOrchestrationPlanManagement
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

        public string GetOrchestrationPrompt()
        {
            string chatPrompt = string.Empty;

            OrchestrationPlan? orchestrationPlan = chatSessionService.GetOrchestration();

            if (orchestrationPlan != null)
            {
                chatPrompt = ProcessOrchestration(orchestrationPlan);
            }

            return chatPrompt;
        }

        public string ProcessOrchestration(OrchestrationPlan orchestrationPlan)
        {
            string chatPrompt = string.Empty;

            OrchestrationItem? currentPendingItem = orchestrationPlan.Items.FirstOrDefault(y => y.ItemStatus == Status.Pending);
            OrchestrationItem? itemToProcess = orchestrationPlan.Items.FirstOrDefault(y => y.ItemStatus == Status.Open);

            if (itemToProcess != null && currentPendingItem == null)
            {
                chatPrompt = itemToProcess.Prompt;

                if (itemToProcess.DependsOnIndices.Count > 0)
                {
                    int index = itemToProcess.DependsOnIndices[0];
                    string payloadName = "result" + index.ToString();
                    string jsonPayload = orchestrationPlan.PayloadItems
                        .FirstOrDefault(y => y.ItemName == payloadName)!.Payload;

                    DataChat dataChat = new DataChat
                    {
                        Prompt = itemToProcess.Prompt,
                        PayloadItems = new List<string>
                        {
                            jsonPayload
                        }
                    };

                    chatPrompt = JsonSerializer.Serialize(dataChat);
                }

                itemToProcess.ItemStatus = Status.Pending;
                chatSessionService.SaveOrchestration(orchestrationPlan);
            } 
            else if (currentPendingItem != null)
            {
                chatPrompt = currentPendingItem.Prompt;
            }
            else
            {
                chatSessionService.ClearOrchestration();
            }

            return chatPrompt;
        }

        public string CacheToolResponse(string returnMessage, bool isDataCall)
        {
            bool hasError = (returnMessage?.IndexOf("ERROR") > -1) ? true : false;
            OrchestrationPlan? orchestrationPlan = chatSessionService.GetOrchestration();

            string jsonOrch = JsonSerializer.Serialize(orchestrationPlan);
            File.AppendAllText("D:\\Repos\\LocalAiChat_3\\LocalAiChat_3\\LogFile.txt", jsonOrch + Environment.NewLine + Environment.NewLine);

            if (orchestrationPlan != null && !hasError)
            {
                OrchestrationItem? itemToProcess =
                    orchestrationPlan.Items.FirstOrDefault(y => y.ItemStatus == Status.Pending);

                if (itemToProcess != null)
                {
                    string callIndex = itemToProcess?.CallIndex.ToString() ?? "";

                    if (!string.IsNullOrEmpty(callIndex))
                    {
                        if (!string.IsNullOrEmpty(returnMessage) && isDataCall)
                        {
                            string payloadItemName = "result" + callIndex;
                            PayloadItem payloadItem = new PayloadItem
                            {
                                ItemName = payloadItemName,
                                Payload = returnMessage
                            };

                            orchestrationPlan.PayloadItems.Add(payloadItem);

                            CollectionResponse response = new CollectionResponse
                            {
                                Prompt = "Present the results as an HTML table. Make the heading gray.",
                                Collection = returnMessage
                            };

                            returnMessage = JsonSerializer.Serialize(response);
                        }
                    }

                    itemToProcess!.ItemStatus = Status.Closed;
                    chatSessionService.SaveOrchestration(orchestrationPlan);
                }
            }
            else
            {
                File.AppendAllText("D:\\Repos\\LocalAiChat_3\\LocalAiChat_3\\LogFile.txt", "Error: " + returnMessage + Environment.NewLine + Environment.NewLine);
            }

            return returnMessage;
        }

        private OrchestrationPlan? GetOrchestrationPlan(string functionArguments)
        {
            OrchestrationPlan? orchestrationPlan = null;

            try
            {
                orchestrationPlan = DeserializerHelper.DeserializeItem<OrchestrationPlan>(functionArguments);

                if (orchestrationPlan != null)
                {
                    for (int i = 0; i <= orchestrationPlan.Items.Count; i++)
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

        private string GetPromptObject(string userPrompt)
        {
            string returnPrompt = userPrompt;

            try
            {
                DataChat? dataChat = DeserializerHelper.DeserializeItem<DataChat>(userPrompt);

                if (dataChat != null)
                    returnPrompt = dataChat.Prompt;
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return returnPrompt;
        }
    }
}