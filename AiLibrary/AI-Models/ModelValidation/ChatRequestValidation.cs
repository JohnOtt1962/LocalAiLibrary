using FluentValidation;

namespace LocalAiLibrary.AiLibrary.AI_Models.ModelValidation
{
    public class ChatRequestValidation : AbstractValidator<ChatRequest>
    {
        public ChatRequestValidation()
        {
            RuleFor(chatRequest => chatRequest.Model)
                .NotEmpty().WithMessage("Model name is required");

            RuleForEach(chatRequest => chatRequest.Tools)
                .SetValidator(new ToolValidator())
                .When(chatRequest => chatRequest.Tools != null && chatRequest.Tools.Any());
        }
    }

    public class ToolValidator : AbstractValidator<Tool>
    {
        public ToolValidator()
        {
            RuleFor(tool => tool.Type).Equal("function");

            RuleFor(tool => tool.Function).NotEmpty();

            RuleFor(tool => tool.Function)
                .NotNull().WithMessage("Function definition is required when tool type is set to function.")
                .SetValidator(new FunctionDefinitionValidator())
                .When(tool => tool.Type == "function");
        }
    }

    public class FunctionDefinitionValidator : AbstractValidator<FunctionDefinition>
    {
        public FunctionDefinitionValidator()
        {
            RuleFor(function => function.Name).NotEmpty().WithMessage("The function tool name cannot be empty.");
        }
    }
}