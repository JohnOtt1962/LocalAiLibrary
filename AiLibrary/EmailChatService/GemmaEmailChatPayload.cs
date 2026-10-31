using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.EmailChatService
{
    public class GemmaEmailChatPayload
    {
        public string Prompt { get; set; } = string.Empty;
        public List<ChatRequestMessage>? Payload { get; set; }
        public string Html { get; set; }
    }
}