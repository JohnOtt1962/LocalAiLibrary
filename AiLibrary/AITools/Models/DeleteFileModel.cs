namespace LocalAiLibrary.AiLibrary.AITools.Models
{
    public class DeleteFileModel
    {
        public required string ToolName { get; set; }
        public required DeleteFileToolArgs Args { get; set; }
    }

    public class DeleteFileToolArgs
    {
        public required string Path { get; set; }
    }
}
