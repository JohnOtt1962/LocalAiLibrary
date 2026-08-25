namespace LocalAiLibrary.AiLibrary.CategoryClassification.Models
{
    public class CategorizedChatEntries
    {
        public required List<CategorizedChatEntry>? CategorizedEntries { get; set; }
    }

    public class CategorizedChatEntry
    {
        public required int ChatId { get; set; }
        public required int CategoryId { get; set; }
    }
}