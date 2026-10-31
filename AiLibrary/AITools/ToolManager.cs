using LocalAiLibrary.AiLibrary.AITools.Command;
using LocalAiLibrary.AiLibrary.AITools.Db;
using LocalAiLibrary.AiLibrary.AITools.Email;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;
using Utilities.Email.EmailOperations;
using Utilities.Logger;
using Utilities.Repo;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LocalAiLibrary.AiLibrary.AITools
{
    internal class ToolManager(IEmail aiEmail, IRepo repo, IOptions<AiConfig> config, IUtilLogger logger)
    {
        private readonly AiConfig _config = config.Value;
        private readonly IEmailTool _emailTool = new EmailTool(aiEmail);
        private readonly ISchemaTool _schemaTool = new SchemaTool(repo, config);
        private readonly IDbReadTool _dbReadTool = new DbReadTool(repo, config);
        private readonly IDbUpdateTool _dbUpdateTool = new DbUpdateTool(repo, config);
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
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });

                throw;
            }

            return commands;
        }

        public async Task<string> SendEmail(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = await _emailTool.SendEmail(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return retMessage;
        }

        public string GetSchema(ToolCommand command, string filter)
        {
            string retMessage;

            try
            {
                retMessage = _schemaTool.GetSchema(command, filter);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return retMessage;
        }

        public async Task<string> ReadDatabase(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = await _dbReadTool.ReadDatabase(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return retMessage;
        }

        public async Task<string> UpdateDatabase(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = await _dbUpdateTool.UpdateDatabase(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return retMessage;
        }
    }
}