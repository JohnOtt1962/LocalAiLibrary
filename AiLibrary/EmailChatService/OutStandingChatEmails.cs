using FluentValidation;
using LocalAiLibrary.AiLibrary.AI_Models.ModelValidation;
using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Network;
using LocalAiLibrary.AiLibrary.Validation.ModelValidation;
using MailKit;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities.Email;
using Utilities.Email.EmailOperations;
using Utilities.Email.Models;

namespace LocalAiLibrary.AiLibrary.EmailChatService
{
    public class OutStandingChatEmails(IValidator<ChatRequest> requestValidator, IOptions<AiConfig> config, 
        IAiNetwork aiNetwork, IValidator<ChatCompletionResponse> responseValidator, IEmail email,
        IOptions<EmailConfig> emailConfig, IAiRepo aiRepo, IEmailManager emailManager) : IOutstandingChatEmails
    {
        private readonly AiConfig _config = config.Value;
        private readonly EmailConfig _emailConfig = emailConfig.Value;

        public async Task GetOutstandingChatEmails()
        {
            List<SenderDetails> senderDetails = await emailManager.GetSenderDetails();

            foreach (SenderDetails item in senderDetails)
            {
                await ProcessSenderItem(item);
            }
        }

        public async Task ProcessSenderItem(SenderDetails item)
        {
            await aiRepo.InsertGemmaEmailChatItem(item.From, item.Content, "user");

            List<ChatRequestMessage> conversationHistory = await PrepareConversationHistoryForUser(item.Content, item.From);

            await SendPromptToModel(conversationHistory, item.From);

            string replyBody = await PrepareEmailContentsForModelReply(item.From);

            await SendReplyToSender(replyBody, item.From, item.EmailId);
        }

        private async Task SendReplyToSender(string reply, string fromAddress, UniqueId currentEmailId)
        {
            EmailModel emailModel = new EmailModel
            {
                ToolName = "Email",
                Args = new ToolArgs
                {
                    To = fromAddress,
                    Subject = "Model reply to " + fromAddress,
                    Body = reply
                }
            };

            await emailManager.SendMail(emailModel);
            await emailManager.DeleteEmail(currentEmailId);
        }

        private async Task<string> PrepareEmailContentsForModelReply(string fromAddress)
        {
            string replyBody = string.Empty;

            //  I DO NOT NEED THIS CALL. I ALREADY HAVE THE CONVERSATION HISTORY. JUST ADD THE MODEL REPLY TO THE TOP
            List<ChatRequestMessage> conversationHistory = await aiRepo.GetGemmaEmailChatItem(fromAddress);

            foreach (ChatRequestMessage item in conversationHistory)
            {
                replyBody += "<b>" + item.Role + "</b><p></p>" + item.Content + "<p></p><p></p>";
            }

            return replyBody;
        }

        private async Task SendPromptToModel(List<ChatRequestMessage> conversationHistory, string fromAddress)
        {
            string reply = string.Empty;
            ChatCompletionResponse? response = await MakeModelCall(conversationHistory);

            if (response?.Choices is { Count: > 0 } choices && choices[0].Message != null &&
                !string.IsNullOrEmpty(choices[0].Message.Content))
            {
                reply = choices[0].Message.Content;
            }

            if(!string.IsNullOrEmpty(reply))
                await aiRepo.InsertGemmaEmailChatItem(fromAddress, reply, "assistant");
        }

        private async Task<List<ChatRequestMessage>> PrepareConversationHistoryForUser(string content, string fromAddress)
        {
            GemmaEmailChatModel gemmaChat = new GemmaEmailChatModel
            {
                Instructions =
                    "The chat is not from John, me, but an unknown user who is a part of your fan club. You can talk about me, but do not address any of your response to me.",
                Prompt = content
            };

            string prompt = JsonSerializer.Serialize(gemmaChat);

            List<ChatRequestMessage> conversationHistory = await aiRepo.GetGemmaEmailChatItem(fromAddress);
            conversationHistory.Add(new ChatRequestMessage("user", prompt, string.Empty));

            return conversationHistory;
        }

        private string GetReply(ChatCompletionResponse? response)
        {
            string reply = string.Empty;

            if (response?.Choices is { Count: > 0 } choices && choices[0].Message != null &&
                !string.IsNullOrEmpty(choices[0].Message.Content))
            {
                reply = choices[0].Message.Content;
            }

            return reply;
        }

        private async Task<ChatCompletionResponse?> MakeModelCall(List<ChatRequestMessage> conversationHistory)
        {
            var request = ChatRequestBuilder.BuildAndValidate(requestValidator, _config.ModelName,
                _config.MaxTokens, conversationHistory, ChatRequestBuilder.ToolOperation.IncludeNoTools);

            ChatCompletionResponse? response =
                await aiNetwork.GetResponse(_config.ApiKey, _config.Url, request);

            ResponseReturnChecker.CheckValidation(response, responseValidator);

            return response;
        }
    }
}