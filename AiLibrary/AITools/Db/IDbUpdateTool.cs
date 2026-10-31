using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public interface IDbUpdateTool
    {
        Task<string> UpdateDatabase(ToolCommand command);
    }
}