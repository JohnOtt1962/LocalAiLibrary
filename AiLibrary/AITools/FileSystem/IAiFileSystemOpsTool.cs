using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.FileSystem
{
    public interface IAiFileSystemOpsTool
    {
        Task<string> ReadTextFile(ToolCommand command);
        void MoveFile(ToolCommand command);
        void DeleteFile(ToolCommand command);
    }
}