using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public interface IDbReadTool
    {
        Task<string> ReadDatabase(ToolCommand command);
    }
}