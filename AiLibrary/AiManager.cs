using LocalAiLibrary.AiLibrary.CategoryClassification;
using LocalAiLibrary.AiLibrary.ChatService;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary
{
    public class AiManager(IAiCommunicationManager aiCommunicationManager, IManageChatEntryClassificationService aiCategoryService) : IAiManager
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

        public async Task ProcessCategories(string jsonPrompt)
        {
            try
            {
                await aiCategoryService.ProcessCategories();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MakeAiCallAsync failed: {ex}");
                throw;
            }
        }
    }
}