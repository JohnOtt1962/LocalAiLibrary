using LocalAiLibrary.AiLibrary.AITools.Models;
using System.Text.Json;
using Utilities.Email;

namespace LocalAiLibrary.AiLibrary.AITools.Email
{
    public class EmailTool(IEmail aiEmail) : IEmailTool
    {
        public string SendEmail(ToolCommand command)
        {
            string returnMessage = string.Empty;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EmailModel aiEmailModel = new EmailModel
            {
                ToolName = command.ToolName,
                Args = JsonSerializer.Deserialize<ToolArgs>(command.ToolArgs, options)!
            };

            bool isSuccess = aiEmail.SendMail(aiEmailModel).GetAwaiter().GetResult();

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