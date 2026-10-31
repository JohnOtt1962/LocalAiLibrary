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

                        Description:
                            "Category: Communication. " +
                            "Sends an email to a specified recipient. " +
                            "You MUST provide all three arguments: to, subject, and body. " +
                            "Never omit an argument. " +
                            "Extract the recipient and subject from the user's request. " +
                            "If the user requests an HTML email, place the complete HTML " +
                            "content in the body argument. " +
                            "Do not call this tool if the recipient, subject, or body " +
                            "cannot be determined.",

                        Parameters: new
                        {
                            type = "object",

                            properties = new
                            {
                                to = new
                                {
                                    type = "string",
                                    description =
                                        "REQUIRED. The recipient's email address. " +
                                        "Copy the complete email address from the user's " +
                                        "request into this argument. Never omit this property."
                                },

                                subject = new
                                {
                                    type = "string",
                                    description =
                                        "REQUIRED. The email subject line. " +
                                        "Use the subject specified by the user. " +
                                        "Never omit this property."
                                },

                                body = new
                                {
                                    type = "string",
                                    description =
                                        "REQUIRED. The complete email body. " +
                                        "If the user requests HTML, provide the complete " +
                                        "HTML content as a string. " +
                                        "Never omit this property."
                                }
                            },

                            required = new[] { "to", "subject", "body" },

                            additionalProperties = false
                        }
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "get_person_address_schema",
                        Description: "Category: Database Interaction. This tool will return all table names together with the foreign key constraints that exist between persons and addresses. There is nothing the agent needs to do here besides call the tool.",
                        Parameters: new {}
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "get_human_resources_schema",
                        Description: "Category: Database Interaction. This tool will return all table names together with the foreign key constraints that exist between persons and addresses. There is nothing the agent needs to do here besides call the tool.",
                        Parameters: new {}
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "get_sales_customer_detail_schema",
                        Description: "Category: Database Interaction. This tool will return all table names together with the foreign key constraints that exist between persons and addresses. There is nothing the agent needs to do here besides call the tool.",
                        Parameters: new {}
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "read_database",
                        Description:
                        "Category: Database Interaction. " +
                        "Generate and execute read-only T-SQL SELECT queries. " +
                        "Use the schema tools before generating SQL. " +
                        "Only use tables, columns, and JOIN relationships verified by the returned schema. " +
                        "Verify that every selected column belongs to its referenced table. " +
                        "Use [SchemaName].[TableName] for every table. " +
                        "If the schema is insufficient, request additional schema information." +
                        "SQL Server dialect only; do not use syntax from other SQL dialects",
                        Parameters: new
                        {
                            type = "object",
                            properties = new
                            {
                                sql = new
                                {
                                    type = "string",
                                    description =
                                        "A read-only T-SQL SELECT query using only verified " +
                                        "tables, columns, and relationships from the database schema."
                                }
                            },
                            required = new[] { "sql" }
                        }
                    )
                ),
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "update_database",
                        Description: "Category: Database Interaction. You will generate query using TSQL. Every table in your query must be represented in the following way: [SchemaName].[TableName]. Ensure that column names you use in your sql statement belong to the tables you associate them with.",
                        Parameters: new
                        {
                            type = "object",
                            properties = new
                            {
                                sql = new { type = "string", description = "An update statement against a database." },
                            },
                            required = new[] { "sql" }
                        }
                    )
                )
            };

            return tools;
        }

        public static List<Tool> GetOrchToolList()
        {
            var tools = new List<Tool>
            {
                new Tool(
                    Type: "function",
                    Function: new FunctionDefinition(
                        Name: "orchestrate_tool_calls",

                        Description:
                            "Decompose the user's compound request into a complete, ordered " +
                            "list of atomic operations. " +
                            "Each operation must represent exactly one action. " +
                            "Do not execute, solve, or implement any operation. " +
                            "Do not add information that was not present in the user's request. " +
                            "Represent dependencies between operations using depends_on_indices. " +
                            "IMPORTANT: Every operation must be represented by a complete object " +
                            "in the items array. Never substitute an operation's call_index " +
                            "for its object. Do not abbreviate, summarize, or omit operations.",

                        Parameters: new
                        {
                            type = "object",

                            properties = new
                            {
                                items = new
                                {
                                    type = "array",

                                    description =
                                        "The complete ordered list of operations required to " +
                                        "satisfy the user's request. " +
                                        "Every array element MUST be an object containing " +
                                        "call_index, prompt, and depends_on_indices. " +
                                        "Do not place integers, strings, null values, or " +
                                        "abbreviated representations directly in this array. " +
                                        "Include one complete object for every operation.",

                                    minItems = 1,

                                    items = new
                                    {
                                        type = "object",

                                        description =
                                            "One complete atomic operation. " +
                                            "Every operation requires its own call_index, " +
                                            "prompt, and depends_on_indices. " +
                                            "Do not replace this object with its call_index.",

                                        properties = new
                                        {
                                            call_index = new
                                            {
                                                type = "integer",
                                                minimum = 1,

                                                description =
                                                    "The execution order of this operation, " +
                                                    "starting at 1. " +
                                                    "Use consecutive integers without gaps."
                                            },

                                            prompt = new
                                            {
                                                type = "string",
                                                minLength = 1,

                                                description =
                                                    "The user's instruction for this operation only. " +
                                                    "Preserve the user's intent and wording as " +
                                                    "closely as possible. " +
                                                    "Do not execute or solve the operation. " +
                                                    "Do not add SQL, code, commands, arguments, " +
                                                    "data, results, examples, implementation " +
                                                    "details, or information that was not " +
                                                    "provided by the user."
                                            },

                                            depends_on_indices = new
                                            {
                                                type = "array",

                                                items = new
                                                {
                                                    type = "integer",
                                                    minimum = 1
                                                },

                                                uniqueItems = true,

                                                description =
                                                    "The call_index values of earlier operations " +
                                                    "whose results are required before this " +
                                                    "operation can execute. " +
                                                    "Use an empty array when the operation " +
                                                    "has no dependencies. " +
                                                    "Every dependency must reference an " +
                                                    "operation with a lower call_index."
                                            }
                                        },

                                        required = new[]
                                        {
                                            "call_index",
                                            "prompt",
                                            "depends_on_indices"
                                        },

                                        additionalProperties = false
                                    }
                                }
                            },

                            required = new[]
                            {
                                "items"
                            },

                            additionalProperties = false
                        }
                    )
                ),
            };

            return tools;
        }
    }
}