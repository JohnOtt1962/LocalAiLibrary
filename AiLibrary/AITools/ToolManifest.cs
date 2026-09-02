using LocalAiLibrary.AiLibrary.ChatService.Models;

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
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "get_person_address_schema",
                        Description: "This tool will return all table names together with the foreign key constraints that exist between persons and addresses. There is nothing the agent needs to do here besides call the tool.",
                        Parameters: new {}
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "get_human_resources_schema",
                        Description: "This tool will return all table names together with the foreign key constraints that exist between persons and addresses. There is nothing the agent needs to do here besides call the tool.",
                        Parameters: new {}
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "get_sales_customer_detail_schema",
                        Description: "This tool will return all table names together with the foreign key constraints that exist between persons and addresses. There is nothing the agent needs to do here besides call the tool.",
                        Parameters: new {}
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "read_database",
                        Description: "You will generate query using TSQL. Execute read-only SQL. Every table in your query must be represented in the following way: [SchemaName].[TableName].",
                        Parameters: new
                        {
                            type = "object",
                            properties = new
                            {
                                sql = new { type = "string", description = "A read-only select against a database." },
                            },
                            required = new[] { "sql" }
                        }
                    )
                )
            };

            return tools;
        }
    }
}