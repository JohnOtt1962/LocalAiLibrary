using LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryService;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary
{
    public class AiManager(IAiCommunicationManager aiCommunicationManager, IAiCategoryManager aiCategoryManager) : IAiManager
    {
        public async Task MakeAiCallAsync(List<ChatRequestMessage> conversationHistory, string userPrompt,
            string? toolUser = null, bool cacheChat = true)
        {
            try
            {
                await aiCommunicationManager.MakeAiCallAsync(conversationHistory, userPrompt, toolUser, cacheChat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MakeAiCallAsync failed: {ex}");
                throw;
            }
        }

        public async Task<List<CategorizedChatEntry>?> MakeAiCallAsync(string jsonPrompt)
        {
            try
            {
                return await aiCategoryManager.MakeAiCallAsync(jsonPrompt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MakeAiCallAsync failed: {ex}");
                throw;
            }

            return null;
        }
    }
}