namespace LocalAiLibrary.AiLibrary.AITools.Models
{
    public class ReadTextModel
    {
        public required string ToolName { get; set; }
        public required ReadTextToolArgs Args { get; set; }
    }

    public class ReadTextToolArgs
    {
        public required string Path { get; set; }
    }
}