using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools.Command;
using LocalAiLibrary.AiLibrary.AITools.Db;
using LocalAiLibrary.AiLibrary.AITools.Email;
using LocalAiLibrary.AiLibrary.AITools.FileSystem;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.AITools.Sentiment;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.Extensions.Options;
using Utilities.Email.EmailOperations;
using Utilities.FileSystem;
using Utilities.Logger;
using Utilities.Repo;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LocalAiLibrary.AiLibrary.AITools
{
    internal class ToolManager(IEmail aiEmail, IRepo repo, IFileSystemManager fileSystemManager, IOptions<AiConfig> config, 
        IChatSessionService chatSessionService, IUtilLogger logger)
    {
        private readonly AiConfig _config = config.Value;
        private readonly IEmailTool _emailTool = new EmailTool(aiEmail);
        private readonly ISchemaTool _schemaTool = new SchemaTool(repo, config, chatSessionService);
        private readonly IDbReadTool _dbReadTool = new DbReadTool(repo, config, chatSessionService);
        private readonly IDbUpdateTool _dbUpdateTool = new DbUpdateTool(repo, config, chatSessionService);
        private readonly IDbInsertTool _dbInsertTool = new DbInsertTool(repo, config, chatSessionService);
        private readonly IAiFileSystemOpsTool _fileSystemOpsTool = new AiFileSystemOpsTool(fileSystemManager, config, chatSessionService);
        private readonly ISentimentAnalysis _sentimentAnalysis = new SentimentAnalysis();
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
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });

                throw;
            }

            return commands;
        }

        public string GetSentimentScore(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = _sentimentAnalysis.GetSentimentScore(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
        }

        public async Task<string> ReadTextFile(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = await _fileSystemOpsTool.ReadTextFile(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
        }

        public string MoveFile(ToolCommand command)
        {
            string retMessage = string.Empty;

            try
            {
                _fileSystemOpsTool.MoveFile(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
        }

        public string DeleteFile(ToolCommand command)
        {
            string retMessage = string.Empty;

            try
            {
                _fileSystemOpsTool.DeleteFile(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
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
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
        }


        public string GetGemmaSchema(ToolCommand command)
        {
            string retMessage;

            try
            {
                retMessage = _schemaTool.GetGemmaSchema(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
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
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
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
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
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
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
        }

        public async Task<string> InsertDatabase(ToolCommand command)
        {
            string retMessage = string.Empty;

            try
            {
                await _dbInsertTool.InsertDatabase(command);
            }
            catch (Exception ex)
            {
                retMessage = "ERROR (Summarize the error message and retry the previous operation. Include no other information): " + ex.Message;

                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure }, GetAgentId());
                });
            }

            return retMessage;
        }

        private string GetAgentId()
        {
            string agentId = string.Empty;
            OrchestrationPlan? orchestrationPlan = chatSessionService.GetOrchestration();

            if (orchestrationPlan != null)
                agentId = orchestrationPlan.Id;

            return agentId;
        }
    }
}