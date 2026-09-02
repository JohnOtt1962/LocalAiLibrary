using FluentValidation;
using LocalAiLibrary.AiLibrary.AITools;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AI_Models.ModelValidation
{
    public static class ChatRequestBuilder
    {
        public static ChatRequest BuildAndValidate(
            IValidator<ChatRequest> validator,
            string modelName,
            int maxTokens,
            List<ChatRequestMessage> conversationHistory)
        {
            var request = new ChatRequest(
                Model: modelName,
                Messages: conversationHistory,
                Tools: ToolManifest.GetToolList(),
                MaxTokens: maxTokens
            );

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