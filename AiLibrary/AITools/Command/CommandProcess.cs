using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Command
{
    public class CommandProcess : ICommandProcess
    {
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
    }
}