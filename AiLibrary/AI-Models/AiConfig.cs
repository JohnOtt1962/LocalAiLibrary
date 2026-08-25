namespace LocalAiLibrary.AiLibrary.AI_Models
{
    public class AiConfig
    {
        public required string Url { get; set; }
        public required string ApiKey { get; set; }
        public required string ModelName { get; set; }
        public required string BaseModelName { get; set; }
        public required string ProcSaveConversation { get; set; }
        public required string ProcGetDailyConversation { get; set; }
        public required string ProcGetCategoryList { get; set; }
        public required string ConnectionString { get; set; }
        public required bool EnableDbCaching { get; set; }
        public required int MaxTokens { get; set; }
    }
}
