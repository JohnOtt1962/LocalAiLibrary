namespace LocalAiLibrary.AiLibrary.AITools.Models
{
    public class SchemaCommandModel
    {
        public required string ToolName { get; set; }
        public SchemaToolArgs Args { get; set; }
    }

    public class SchemaToolArgs
    {
        public string Sql { get; set; }
    }
}