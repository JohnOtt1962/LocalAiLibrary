using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AITools
{
    public interface IToolSelection
    {
        Task<string> ProcessToolCalls(ChatCompletionResponse response);
    }
}