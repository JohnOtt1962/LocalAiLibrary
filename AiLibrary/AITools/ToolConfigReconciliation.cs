using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AITools
{
    public static class ToolConfigReconciliation
    {
        public static string GetConnectionString(IChatSessionService chatSessionService, AiConfig config)
        {
            string connectionString = config.ConnectionString;
            string serviceName = chatSessionService.GetServiceName();

            if (string.IsNullOrEmpty(serviceName))
                connectionString = config.ConnectionString;

            if (serviceName == "Sentiment")
                connectionString = config.Sentiment.ConnectionString;

            if (serviceName == "AdventureWorks")
                connectionString = config.AdventureWorks.ConnectionString;

            if (serviceName == "Category")
                connectionString = config.Category.ConnectionString;

            return connectionString;
        }

        public static string GetReadFilePath(IChatSessionService chatSessionService, AiConfig config)
        {
            string readFilePath = config.ReadFilePath;
            string serviceName = chatSessionService.GetServiceName();

            if (serviceName == "Sentiment")
                readFilePath = config.Sentiment.ReadFilePath;

            return readFilePath;
        }

        public static string GetMoveFilePath(IChatSessionService chatSessionService, AiConfig config)
        {
            string moveFilePath = config.MoveFilePath;
            string serviceName = chatSessionService.GetServiceName();

            if (serviceName == "Sentiment")
                moveFilePath = config.Sentiment.MoveFilePath;

            return moveFilePath;
        }

        public static string GetDeleteFilePath(IChatSessionService chatSessionService, AiConfig config)
        {
            string deleteFilePath = config.DeleteFilePath;
            string serviceName = chatSessionService.GetServiceName();

            if (serviceName == "Sentiment")
                deleteFilePath = config.Sentiment.DeleteFilePath;

            return deleteFilePath;
        }
    }
}