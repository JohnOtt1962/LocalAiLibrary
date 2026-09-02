using LocalAiLibrary.AiLibrary.AITools.Command;
using LocalAiLibrary.AiLibrary.AITools.Db;
using LocalAiLibrary.AiLibrary.AITools.Email;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;
using Utilities.Email;
using Utilities.Repo;

namespace LocalAiLibrary.AiLibrary.AITools
{
    internal class ToolManager(IEmail aiEmail, IRepo repo, IOptions<AiConfig> config)
    {
        private readonly IEmailTool _emailTool = new EmailTool(aiEmail);
        private readonly ISchemaTool _schemaTool = new SchemaTool(repo, config);
        private readonly IDbReadTool _dbReadTool = new DbReadTool(repo, config);
        private readonly ICommandProcess _commandProcess = new CommandProcess();

        public List<ToolCommand> GetToolCommands(ChatCompletionResponse response)
        {
            List<ToolCommand> commands = new List<ToolCommand>();

            try
            {
                commands = _commandProcess.GetToolCommands(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetToolCommands failed: {ex}");
                throw;
            }

            return commands;
        }

        public string SendEmail(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = _emailTool.SendEmail(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendEmail failed: {ex}");
                throw;
            }

            return retMessage;
        }

        public string GetSchema(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = _schemaTool.GetSchema(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetSchema failed: {ex}");
                throw;
            }

            return retMessage;
        }

        public string ReadDatabase(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = _dbReadTool.ReadDatabase(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ReadDatabase failed: {ex}");
                throw;
            }

            return retMessage;
        }
    }
}