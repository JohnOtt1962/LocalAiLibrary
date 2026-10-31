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
        public required string LoggerProcedure { get; set; }
        public required int MaxTokens { get; set; }
    }
}
