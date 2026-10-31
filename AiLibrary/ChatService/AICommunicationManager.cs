using FluentValidation;
using LocalAiLibrary.AiLibrary.AI_Models.ModelValidation;
using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Network;
using LocalAiLibrary.AiLibrary.Orchestration;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using LocalAiLibrary.AiLibrary.Validation.ModelValidation;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.ChatService
{
    public class AiCommunicationManager(IChatSessionService chatSessionService, IAiNetwork aiNetwork, 
        IOptions<AiConfig> config, IValidator<ChatRequest> requestValidator, 
        IValidator<ChatCompletionResponse> responseValidator, IPromptHelper promptHelper,
        IOrchestrationPlanManagement orchestrationPlanManagement, IToolSelection toolSelection) : IAiCommunicationManager
    {
        private readonly AiConfig _config = config.Value;
        private bool _checkOrchestration = true;

        public async Task MakeAiCallAsync(List<ChatRequestMessage> conversationHistory, string userPrompt)
        {
            await promptHelper.InsertConversationFragment("user", userPrompt);
            promptHelper.AddPromptToActiveContext(conversationHistory, "user", userPrompt, string.Empty);

            ChatCompletionResponse? response = await MakeModelCall(conversationHistory);

            if (response?.Choices[0].Message.ToolCalls?[0].Function.Name == "orchestrate_tool_calls")
            {
                _checkOrchestration = false;
                promptHelper.RemovePromptFromActiveContext(conversationHistory);
                await ProcessOrchestrationExec(response, conversationHistory);
            }
            else
            {
                await ManageResponse(conversationHistory, response);
            }
        }

        private async Task ProcessOrchestrationExec(ChatCompletionResponse? response, List<ChatRequestMessage> conversationHistory)
        {
            OrchestrationPlan? orchestrationPlan =
                orchestrationPlanManagement.SaveOrchestrationPlan(response);

            if (orchestrationPlan != null)
            {
                while (true)
                {
                    string chatPrompt = orchestrationPlanManagement.GetOrchestrationPrompt();

                    if (!string.IsNullOrEmpty(chatPrompt))
                    {
                        await promptHelper.AddOrchestrationPromptFragment(chatPrompt);
                        await MakeAiCallAsyncExec(conversationHistory, chatPrompt);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private async Task MakeAiCallAsyncExec(List<ChatRequestMessage> conversationHistory, string userPrompt)
        {
            bool hasError = (userPrompt.IndexOf("ERROR", StringComparison.Ordinal) > -1) ? true : false;

            promptHelper.AddPromptToActiveContext(conversationHistory, "user", userPrompt, string.Empty);

            ChatCompletionResponse? response = await MakeModelCall(conversationHistory);

            if (hasError)
                promptHelper.RemovePromptFromActiveContext(conversationHistory);

            await ManageResponse(conversationHistory, response);
            await ProcessToolCalls(response, conversationHistory);
        }

        private async Task<ChatCompletionResponse?> MakeModelCall(List<ChatRequestMessage> conversationHistory)
        {
            var toolOperation = ChatRequestBuilder.ToolOperation.CheckOrchestration;
            if (!_checkOrchestration) toolOperation = ChatRequestBuilder.ToolOperation.GetToolList;

            var request = ChatRequestBuilder.BuildAndValidate(requestValidator, _config.ModelName,
                _config.MaxTokens, conversationHistory, toolOperation);

            ChatCompletionResponse? response =
                await aiNetwork.GetResponse(_config.ApiKey, _config.Url, request);

            ResponseReturnChecker.CheckValidation(response, responseValidator);

            return response;
        }

        private async Task ManageResponse(List<ChatRequestMessage> conversationHistory, ChatCompletionResponse? response)
        {
            if (response?.Choices is { Count: > 0 } choices && choices[0].Message != null && !string.IsNullOrEmpty(choices[0].Message.Content))
                await promptHelper.AddAssistantToConversation(conversationHistory, choices);

            chatSessionService.SaveHistory(conversationHistory);
        }

        private async Task ProcessToolCalls(ChatCompletionResponse response, List<ChatRequestMessage> conversationHistory)
        {
            string returnMessage = await toolSelection.ProcessToolCalls(response);

            if (!string.IsNullOrEmpty(returnMessage))
                await MakeAiCallAsyncExec(conversationHistory, returnMessage);
        }

        private void LogRequest(ChatRequest request)
        {
            string jsonReq = JsonSerializer.Serialize(request);
            File.AppendAllText("D:\\Repos\\LocalAiChat_3\\LocalAiChat_3\\LogFile.txt", jsonReq + Environment.NewLine + Environment.NewLine);
        }

        private void LogResponse(ChatCompletionResponse response)
        {
            string jsonResp = JsonSerializer.Serialize(response);
            File.AppendAllText("D:\\Repos\\LocalAiChat_3\\LocalAiChat_3\\LogFile.txt", jsonResp + Environment.NewLine + Environment.NewLine);
        }
    }
}