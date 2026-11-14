namespace LocalAiLibrary.AiLibrary.ChatService.Models
{
    public class ChatApplications
    {
        public List<ChatApplication> Categories { get; set; } = new List<ChatApplication>();
    }

    public class ChatApplication
    {
        public required int Id { get; set; }

        public required string Name { get; set; }
    }
}
