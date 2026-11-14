using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities.FileSystem;

namespace LocalAiLibrary.AiLibrary.AITools.FileSystem
{
    public class AiFileSystemOpsTool(IFileSystemManager fileSystemManager, IOptions<AiConfig> config, IChatSessionService chatSessionService) : IAiFileSystemOpsTool
    {
        private readonly AiConfig _config = config.Value;

        public async Task<string> ReadTextFile(ToolCommand command)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ReadTextModel readTextModel = new ReadTextModel
            {
                ToolName = command.ToolName,
                Args = DeserializerHelper.DeserializeItem<ReadTextToolArgs>(command.ToolArgs, options)!
            };

            string readFilePath = ToolConfigReconciliation.GetReadFilePath(chatSessionService, _config);

            string path = Path.Combine(readFilePath, readTextModel.Args.Path);
            return await fileSystemManager.ReadTextFile(path);
        }

        public void MoveFile(ToolCommand command)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            MoveFileModel moveFileModel = new MoveFileModel
            {
                ToolName = command.ToolName,
                Args = DeserializerHelper.DeserializeItem<MoveFileToolArgs>(command.ToolArgs, options)!
            };

            string readFilePath = ToolConfigReconciliation.GetReadFilePath(chatSessionService, _config);
            string moveFilePath = ToolConfigReconciliation.GetMoveFilePath(chatSessionService, _config);

            string sourcePath = Path.Combine(readFilePath, moveFileModel.Args.Path);
            string destinationPath = Path.Combine(moveFilePath, moveFileModel.Args.Path);
            fileSystemManager.MoveFile(sourcePath, destinationPath);
        }

        public void DeleteFile(ToolCommand command)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            DeleteFileModel deleteFileModel = new DeleteFileModel
            {
                ToolName = command.ToolName,
                Args = DeserializerHelper.DeserializeItem<DeleteFileToolArgs>(command.ToolArgs, options)!
            };

            string deleteFilePath = ToolConfigReconciliation.GetDeleteFilePath(chatSessionService, _config);

            string path = Path.Combine(deleteFilePath, deleteFileModel.Args.Path);
            fileSystemManager.DeleteFile(path);
        }
    }
}