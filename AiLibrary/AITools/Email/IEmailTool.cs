using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Email
{
    public interface IEmailTool
    {
        Task<string> SendEmail(ToolCommand command);
    }
}
