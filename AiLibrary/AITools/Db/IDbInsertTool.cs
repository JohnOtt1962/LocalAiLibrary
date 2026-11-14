using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public interface IDbInsertTool
    {
        Task InsertDatabase(ToolCommand command);
    }
}