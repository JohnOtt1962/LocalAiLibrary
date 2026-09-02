using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Email
{
    public interface IEmailTool
    {
        string SendEmail(ToolCommand command);
    }
}
