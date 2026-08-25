namespace LocalAiLibrary.AiLibrary.CategoryClassification.Models
{
    public class ChatCategories
    {
        public List<ChatCategory> Categories { get; set; } = new List<ChatCategory>();
    }

    public class ChatCategory
    {
        public required int Id { get; set; }

        public required string CategoryName { get; set; }
    }
}