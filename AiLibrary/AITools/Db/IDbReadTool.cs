using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public interface IDbReadTool
    {
        string ReadDatabase(ToolCommand command);
    }
}