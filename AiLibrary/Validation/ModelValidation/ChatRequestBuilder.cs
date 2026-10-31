using FluentValidation;
using LocalAiLibrary.AiLibrary.AITools;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LocalAiLibrary.AiLibrary.AI_Models.ModelValidation
{
    public static class ChatRequestBuilder
    {
        public enum ToolOperation
        {
            GetToolList,
            CheckOrchestration,
            IncludeNoTools
        }
        public static ChatRequest BuildAndValidate(
            IValidator<ChatRequest> validator,
            string modelName,
            int maxTokens,
            List<ChatRequestMessage> conversationHistory,
            ToolOperation toolOperation)
        {
            ChatRequest request;

            if (toolOperation == ToolOperation.GetToolList)
            {
                request = new ChatRequest(
                    Model: modelName,
                    Messages: conversationHistory,
                    Tools: ToolManifest.GetToolList(),
                    MaxTokens: maxTokens
                );
            }
            else if(toolOperation == ToolOperation.CheckOrchestration)
            {
                request = new ChatRequest(
                    Model: modelName,
                    Messages: conversationHistory,
                    Tools: ToolManifest.GetOrchToolList(),
                    ToolChoice: "required",
                    MaxTokens: maxTokens
                );
            }
            else
            {
                request = new ChatRequest(
                    Model: modelName,
                    Messages: conversationHistory,
                    Tools: [],
                    MaxTokens: maxTokens
                );
            }

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new InvalidOperationException(string.Join(", ", errorMessages));
            }

            return request;
        }
    }
}