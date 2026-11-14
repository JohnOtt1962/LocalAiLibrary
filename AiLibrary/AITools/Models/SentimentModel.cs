namespace LocalAiLibrary.AiLibrary.AITools.Models
{
    public class SentimentModel
    {
        public required string ToolName { get; set; }
        public required SentimentToolArgs Args { get; set; }
    }

    public class SentimentToolArgs
    {
        public required string Sentiment { get; set; }
    }
}
