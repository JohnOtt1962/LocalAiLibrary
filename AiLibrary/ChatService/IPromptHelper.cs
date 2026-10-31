using LocalAiLibrary.AiLibrary.ChatService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalAiLibrary.AiLibrary.ChatService
{
    public interface IPromptHelper
    {
        string GetPromptObject(string userPrompt);
        Task InsertConversationFragment(string role, string userPrompt);
        Task AddOrchestrationPromptFragment(string userPrompt);
        void AddPromptToActiveContext(List<ChatRequestMessage> conversationHistory, string role, string userPrompt,
            string toolCallId);
        void RemovePromptFromActiveContext(List<ChatRequestMessage> conversationHistory);
        Task AddAssistantToConversation(List<ChatRequestMessage> conversationHistory,
            IReadOnlyList<ChatChoice> choices);
    }
}