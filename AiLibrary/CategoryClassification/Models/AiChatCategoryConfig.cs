namespace LocalAiLibrary.AiLibrary.CategoryClassification.Models
{
    public class AiChatCategoryConfig
    {
        public required string CategoryPrompt { get; set; }
        public required string ProcGetChatCategories { get; set; }
        public required string ProcGetUncategorizedChatDates { get; set; }
        public required string ProcGetUncategorizedChatByDate { get; set; }
        public required string ProcSetChatCategory { get; set; }
        public required string ConnectionString { get; set; }
    }
}