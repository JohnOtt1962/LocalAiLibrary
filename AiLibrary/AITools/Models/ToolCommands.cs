namespace LocalAiLibrary.AiLibrary.AITools.Models
{
    public class ToolCommands
    {
        public List<ToolCommand> Commands = new List<ToolCommand>();
    }

    public class ToolCommand
    {
        public required string ToolName { get; set; }
        public required string ToolArgs { get; set; }
        public required string ToolCallId { get; set; }
    }
}