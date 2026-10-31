using LocalAiLibrary.AiLibrary.AITools.Models;
using System.Text.Json;
using LocalAiLibrary.AiLibrary.Helpers;
using Utilities.Email.EmailOperations;
using Utilities.Email.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Email
{
    public class EmailTool(IEmail aiEmail) : IEmailTool
    {
        public async Task<string> SendEmail(ToolCommand command)
        {
            string returnMessage = string.Empty;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EmailModel aiEmailModel = new EmailModel
            {
                ToolName = command.ToolName,
                Args = DeserializerHelper.DeserializeItem<ToolArgs>(command.ToolArgs, options)!
            };

            bool isSuccess = await aiEmail.SendMail(aiEmailModel);

            if (isSuccess)
            {
                string template =
                    "The email to $ToAddress$ has been successfully sent. Please briefly and cheerfully confirm to the user that the email has been delivered.";
                returnMessage = template.Replace("$ToAddress$", aiEmailModel.Args.To);
            }

            return returnMessage;
        }
    }
}