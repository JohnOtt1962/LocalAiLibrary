using FluentValidation;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AI_Models.ModelValidation
{
    public static class ResponseReturnChecker
    {
        public static void CheckValidation(ChatCompletionResponse response, 
            IValidator<ChatCompletionResponse> validator)
        {
            if (response == null)
                throw new InvalidOperationException("Response is null");

            var result = validator.Validate(response);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new InvalidOperationException(string.Join(", ", errorMessages));
            }
        }
    }
}