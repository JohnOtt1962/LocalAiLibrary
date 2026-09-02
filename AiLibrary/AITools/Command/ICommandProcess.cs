using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Command
{
    public interface ICommandProcess
    {
        List<ToolCommand> GetToolCommands(ChatCompletionResponse response);
    }
}