namespace LocalAiLibrary.AiLibrary.CategoryClassification.Models
{
    public class WorkingCategoryChatIemsForDate
    {
        public required List<UncategorizedChat> UncategorizedChats { get; set; }
    }

    public class UncategorizedChat
    {
        public required int Id { get; set; }
        public required string Content { get; set; }
        //public required int CategoryId { get; set; }
    }
}