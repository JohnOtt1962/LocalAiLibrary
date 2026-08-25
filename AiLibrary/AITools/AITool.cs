using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.AITools
{
    public interface IAiTool { string ToolName { get; } }

    public class AiTool<T> : IAiTool
    {
        public string ToolName { get; set; }
        public T Args { get; set; }
    }

    public static class AiToolFactory
    {
        public static IAiTool GetAiTool(string toolName, string args)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (toolName == "send_email")
            {
                return new AiTool<EmailArgs>
                {
                    ToolName = toolName,
                    Args = JsonSerializer.Deserialize<EmailArgs>(args, options)!
                };
            }

            return null;
        }
    }

    public class EmailArgs
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}