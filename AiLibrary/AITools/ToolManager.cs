using LocalAiLibrary.AiLibrary.AI_Models;
using System.Text.Json;
using Utilities.Email;

namespace LocalAiLibrary.AiLibrary.AITools
{
    internal class ToolManager(IEmail aiEmail)
    {
        private readonly IEmail _aiEmail = aiEmail;

        public List<ToolCommand> GetToolCommands(ChatCompletionResponse response)
        {
            List<ToolCommand> commands = new List<ToolCommand>();

            string finishReason = response.Choices[0].FinishReason;
            bool hasToolCalls = response.Choices[0].Message.ToolCalls != null &&
                                response.Choices[0].Message.ToolCalls!.Count > 0;

            if (finishReason == "tool_calls" && hasToolCalls)
            {
                foreach (var item in response.Choices[0].Message.ToolCalls!)
                {
                    ToolCommand command = new ToolCommand()
                    {
                        ToolName = item.Function.Name,
                        ToolArgs = item.Function.Arguments,
                        ToolCallId = item.Id
                    };

                    commands.Add(command);
                }
            }

            return commands;
        }

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

            bool isSuccess = _aiEmail.SendMail(aiEmailModel).GetAwaiter().GetResult();

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