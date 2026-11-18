using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Orchestration.Helpers;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities.Logger;
using Utilities.Repo;

namespace LocalAiLibrary.AiLibrary.Orchestration
{
    public class OrchestrationPlanManagement(IChatSessionService chatSessionService, IUtilLogger logger, 
        IOptions<AiConfig> config, IAiRepo aiRepo) : IOrchestrationPlanManagement
    {
        private readonly OrchestrationPromptHelper _orchestrationPromptHelper = new();
        private readonly OrchestrationHelper _orchestrationHelper = new(chatSessionService, logger, config);
        private readonly AiConfig _config = config.Value;

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
                    DataChat dataChat = new DataChat
                    {
                        Prompt = itemToProcess.Prompt,
                        PayloadItems = new List<string>()
                    };

                    foreach (var index in itemToProcess.DependsOnIndices)
                    {
                        string payloadName = "result" + index.ToString();
                        string jsonPayload = orchestrationPlan.PayloadItems
                            .FirstOrDefault(y => y.ItemName == payloadName)!.Payload;

                        dataChat.PayloadItems.Add(jsonPayload);
                    }

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

        public string CacheToolResponse(string returnMessage, bool isDataCall, string callType)
        {
            bool hasError = (returnMessage?.IndexOf("ERROR") > -1) ? true : false;
            OrchestrationPlan? orchestrationPlan = chatSessionService.GetOrchestration();

            string jsonOrch = JsonSerializer.Serialize(orchestrationPlan);
            //File.AppendAllText("D:\\Repos\\LocalAiChat_3\\LocalAiChat_3\\LogFile.txt", jsonOrch + Environment.NewLine + Environment.NewLine);

            if (orchestrationPlan != null && !hasError)
            {
                OrchestrationItem? itemToProcess =
                    orchestrationPlan.Items.FirstOrDefault(y => y.ItemStatus == Status.Pending);

                if (itemToProcess != null)
                {
                    string callIndex = itemToProcess?.CallIndex.ToString() ?? "";

                    if (!string.IsNullOrEmpty(callIndex))
                    {
                        CollectionResponse response = new CollectionResponse();

                        if (!string.IsNullOrEmpty(returnMessage) && isDataCall)
                        {
                            string payloadItemName = "result" + callIndex;
                            PayloadItem payloadItem = new PayloadItem
                            {
                                ItemName = payloadItemName,
                                Payload = returnMessage
                            };

                            orchestrationPlan.PayloadItems.Add(payloadItem);

                            if (callType == "collection")
                                returnMessage = _orchestrationPromptHelper.GetCollectionPrompt(response, returnMessage);
                            else
                                returnMessage = _orchestrationPromptHelper.GetNonCollectionPrompt(response, returnMessage);
                        }
                        else
                            returnMessage = _orchestrationPromptHelper.GetSilentPrompt(response, itemToProcess);
                    }

                    itemToProcess!.ItemStatus = Status.Closed;
                    chatSessionService.SaveOrchestration(orchestrationPlan);
                }
                else
                {
                    returnMessage = string.Empty;
                }
            }
            else
            {
                orchestrationPlan.RetryCount++;

                if (orchestrationPlan.RetryCount > _config.OrchestrationMaxRetry)
                {
                    string serializedOrchPlan = JsonSerializer.Serialize(orchestrationPlan);
                    aiRepo.InsertOrchestrationFailure(orchestrationPlan.Id, serializedOrchPlan);

                    OrchestrationItem? itemToProcess =
                        orchestrationPlan.Items.FirstOrDefault(y => y.ItemStatus == Status.Pending);

                    returnMessage = _orchestrationPromptHelper.GetExceedRetryCountPrompt(orchestrationPlan);

                    for (int i = 0; i <= orchestrationPlan.Items.Count - 1; i++)
                        orchestrationPlan.Items[i].ItemStatus = Status.Closed;

                    orchestrationPlan.RetryCount = 0;
                }

                chatSessionService.SaveOrchestration(orchestrationPlan);

                //File.AppendAllText("D:\\Repos\\LocalAiChat_3\\LocalAiChat_3\\LogFile.txt", "Error: " + returnMessage + Environment.NewLine + Environment.NewLine);
            }

            return returnMessage;
        }

        public OrchestrationPlan? SaveOrchestrationPlan(ChatCompletionResponse? response)
        {
            return _orchestrationHelper.SaveOrchestrationPlan(response);
        }
    }
}