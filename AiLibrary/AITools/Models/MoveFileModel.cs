namespace LocalAiLibrary.AiLibrary.AITools.Models
{
    public class MoveFileModel
    {
        public required string ToolName { get; set; }
        public required MoveFileToolArgs Args { get; set; }
    }

    public class MoveFileToolArgs
    {
        public required string Path { get; set; }
    }
}