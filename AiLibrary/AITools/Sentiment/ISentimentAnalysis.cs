using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Sentiment
{
    public interface ISentimentAnalysis
    {
        string GetSentimentScore(ToolCommand command);
    }
}