using FluentValidation;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;

namespace LocalAiLibrary.AiLibrary.AI_Models.ModelValidation
{
    public class ChatCompletionResponseValidation : AbstractValidator<ChatCompletionResponse>
    {
        public ChatCompletionResponseValidation(IOptions<AiConfig> config)
        {
            RuleFor(chatCompletionResponse => chatCompletionResponse.Model)
                .Equal(config.Value.BaseModelName)
                .WithMessage("The response model is empty or it populated with the wrong name.");

            RuleFor(chatCompletionResponse => chatCompletionResponse.Choices)
                .NotEmpty()
                .WithMessage("The response contained no choices.");

            RuleForEach(chatCompletionResponse => chatCompletionResponse.Choices)
                .SetValidator(new ChatChoiceValidation());
        }
    }

    public class ChatChoiceValidation : AbstractValidator<ChatChoice>
    {
        public ChatChoiceValidation()
        {
            RuleFor(chatMessage => chatMessage.Message.Role)
                .NotEmpty()
                .WithMessage("The returned role in the response was empty");

            RuleFor(chatChoice => chatChoice.Message.Content)
                .NotEmpty()
                .When(chatChoice => chatChoice.FinishReason != "tool_calls");

            RuleFor(chatChoice => chatChoice.FinishReason)
                .NotEmpty()
                .WithMessage("Finish reason came back empty. This value must be populated.");

            RuleFor(chatChoice => chatChoice.Message.ToolCalls)
                .NotEmpty()
                .WithMessage("A tool_calls response must contain at least one tool call.")
                .When(chatChoice => chatChoice.FinishReason == "tool_calls");

            RuleForEach(x => x.Message.ToolCalls)
                .SetValidator(new ToolCallValidation())
                .When(chatChoice => chatChoice.FinishReason == "tool_calls");
        }
    }

    public class ToolCallValidation : AbstractValidator<ToolCall>
    {
        public ToolCallValidation()
        {
            RuleFor(toolCall => toolCall.Id)
                .NotEmpty()
                .WithMessage("The tool_call id cannot be empty.");

            RuleFor(toolCall => toolCall.Type)
                .Equal("function")
                .WithMessage("The tool_call type must be set to function");

            RuleFor(toolCall => toolCall.Function)
                .NotNull()
                .WithMessage("Function definition is required when tool type is set to function.")
                .SetValidator(new FunctionCallValidator())
                .When(toolCall => toolCall.Type == "function");
        }
    }

    public class FunctionCallValidator : AbstractValidator<FunctionCall>
    {
        public FunctionCallValidator()
        {
            RuleFor(functionCall => functionCall.Name)
                .NotEmpty()
                .WithMessage("A function call in the response does not have a name");

            RuleFor(functionCall => functionCall.Arguments)
                .NotNull()
                .WithMessage("A function call in the response has a null parameters node.");
        }
    }
}