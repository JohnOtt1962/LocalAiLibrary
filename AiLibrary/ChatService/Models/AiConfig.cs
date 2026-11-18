namespace LocalAiLibrary.AiLibrary.ChatService.Models
{
    public class AiConfig
    {
        public required string Url { get; set; }
        public required string ApiKey { get; set; }
        public required string ModelName { get; set; }
        public required string BaseModelName { get; set; }
        public required string ProcSaveConversation { get; set; }
        public required string ProcRemoveConversation { get; set; }
        public required string ProcGetDailyConversation { get; set; }
        public required string ProcGetCategoryList { get; set; }
        public required string ProcGetChatApplications { get; set; }
        public required string ProcInsertGemmaEmailChat { get; set; }
        public required string ProcGetGemmaEmailChat { get; set; }
        public required string ConnectionString { get; set; }
        public required string AdventureWorksConnectionString { get; set; }
        public required string HumanResourcesSchemaFilter { get; set; }
        public required string PersonSchemaFilter { get; set; }
        public required string SalesSchemaFilter { get; set; }
        public required bool EnableDbCaching { get; set; }
        public required string SessionKey { get; set; }
        public required string SessionCategoriesKey { get; set; }
        public required string OrchestrationPlanKey { get; set; }
        public required string ReadFilePath { get; set; }
        public required string MoveFilePath { get; set; }
        public required string DeleteFilePath { get; set; }
        public required string LoggerProcedure { get; set; }
        public required int MaxTokens { get; set; }
        public required AiSentimentAnalysis Sentiment { get; set; }
        public required AiAdventureWorks AdventureWorks { get; set; }
        public required AiChatCagetgory Category { get; set; }
        public required int OrchestrationMaxRetry { get; set; }
        public required string OrchestrationFailureProc { get; set; }
    }

    public class AiSentimentAnalysis
    {
        public required string ReadFilePath { get; set; }
        public required string MoveFilePath { get; set; }
        public required string DeleteFilePath { get; set; }
        public required string SentimentJsonPromptLocation { get; set; }
        public required string SentimentCleanupPromptLocation { get; set; }
        public required string ConnectionString { get; set; }
    }

    public class AiAdventureWorks
    {
        public required string ConnectionString { get; set; }
        public required string HumanResourcesSchemaFilter { get; set; }
        public required string PersonSchemaFilter { get; set; }
        public required string SalesSchemaFilter { get; set; }

    }

    public class AiChatCagetgory
    {
        public required string CategoryPrompt { get; set; }
        public required string ConnectionString { get; set; }
        public required string ProcGetChatCategories { get; set; }
        public required string ProcGetUncategorizedChatDates { get; set; }
        public required string ProcGetUncategorizedChatByDate { get; set; }
        public required string ProcSetChatCategory { get; set; }
    }
}
