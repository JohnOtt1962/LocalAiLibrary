using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryService
{
    public interface IAiCategoryManager
    {
        Task<List<CategorizedChatEntry>?> MakeAiCallAsync(string jsonPrompt);
    }
}