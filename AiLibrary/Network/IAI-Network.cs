using LocalAiLibrary.AiLibrary.AI_Models;

namespace LocalAiLibrary.AiLibrary.Network
{
    public interface IAiNetwork
    {
        Task<ChatCompletionResponse?> GetResponse(string apiKey, string url, ChatRequest request);
    }
}