using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.Network
{
    public interface IAiNetwork
    {
        Task<ChatCompletionResponse?> GetResponse(string apiKey, string url, ChatRequest request);
    }
}