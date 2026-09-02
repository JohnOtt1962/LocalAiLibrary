using LocalAiLibrary.AiLibrary.AITools.Models;

namespace LocalAiLibrary.AiLibrary.AITools.Db
{
    public interface ISchemaTool
    {
        string GetSchema(ToolCommand command);
    }
}