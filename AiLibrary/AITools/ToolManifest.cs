using LocalAiLibrary.AiLibrary.AI_Models;

namespace LocalAiLibrary.AiLibrary.AITools
{
    internal class ToolManifest
    {
        public static List<Tool> GetToolList()
        {
            var tools = new List<Tool>
            {
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "send_email",
                        Description: "Sends an email to a specified recipient with a subject and body.",
                        Parameters: new
                        {
                            type = "object",
                            properties = new
                            {
                                to = new { type = "string", description = "The recipient's email address." },
                                subject = new { type = "string", description = "The email subject line." },
                                body = new { type = "string", description = "The body content of the email." }
                            },
                            required = new[] { "to", "subject", "body" }
                        }
                    )
                )
            };

            return tools;
        }
    }
}