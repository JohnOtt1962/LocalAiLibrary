using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.AITools.Sentiment
{
    public class SentimentAnalysis : ISentimentAnalysis
    {
        public string GetSentimentScore(ToolCommand command)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SentimentModel sentimentModel = new SentimentModel
            {
                ToolName = command.ToolName,
                Args = DeserializerHelper.DeserializeItem<SentimentToolArgs>(command.ToolArgs, options)!
            };

            if (string.IsNullOrEmpty(sentimentModel.Args.Sentiment))
                throw new Exception("Sentiment score not found!");

            return sentimentModel.Args.Sentiment;
        }
    }
}