namespace LocalAiLibrary.AiLibrary.CategoryClassification.Models
{
    public class AiCategorizationPayload
    {
        public required string CategoryPrompt { get; set; }
        public required List<ChatCategory> Categories { get; set; }
        public required List<UncategorizedChat> UncategorizedChats { get; set; }
    }
}