using LocalAiLibrary.AiLibrary.AI_Models;
using System.Text.Json;
using Utilities.Network;

namespace LocalAiLibrary.AiLibrary.Network
{
    public class AiNetwork(INetwork network) : IAiNetwork
    {
        public async Task<ChatCompletionResponse?> GetResponse(string apiKey, string url, ChatRequest request)
        {
            ChatCompletionResponse? responseRecord;
            string jsonRequest = JsonSerializer.Serialize(request);
            NetworkResult result = await network.SendAsync(apiKey, url, jsonRequest);

            if (result.CallSuccess && !string.IsNullOrEmpty(result.JsonResponse))
            {
                responseRecord = JsonSerializer.Deserialize<ChatCompletionResponse>(result.JsonResponse);
            }
            else
            {
                responseRecord = null;
            }

            return responseRecord;
        }
    }
}