namespace LocalAiLibrary.AiLibrary.AI_Models
{
    internal class ToolCommands
    {
        public List<ToolCommand> Commands = new List<ToolCommand>();
    }

    internal class ToolCommand
    {
        public required string ToolName { get; set; }
        public required string ToolArgs { get; set; }
        public required string ToolCallId { get; set; }
    }
}