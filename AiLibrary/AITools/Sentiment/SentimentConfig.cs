namespace LocalAiLibrary.AiLibrary.AITools.Sentiment
{
    public class SentimentConfig
    {
        public required string ReadFilePath { get; set; }
        public required string MoveFilePath { get; set; }
        public required string DeleteFilePath { get; set; }
        public required string SentimentJsonPromptLocation { get; set; }
        public required string SentimentCleanupPromptLocation { get; set; }
        public required string ConnectionString { get; set; }
    }
}