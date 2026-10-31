namespace LocalAiLibrary.AiLibrary.Orchestration.Models
{
    public class DataChat
    {
        public string Prompt { get; set; } = string.Empty;
        public List<string> PayloadItems { get; set; } = new List<string>();
    }
}