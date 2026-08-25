using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.Network
{
    public interface IAiCategoryManager
    {
        Task<List<CategorizedChatEntry>?> MakeAiCallAsync(string jsonPrompt);
    }
}