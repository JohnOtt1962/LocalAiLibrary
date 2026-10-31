namespace LocalAiLibrary.AiLibrary.AiSessionService
{
    public interface ISessionStore
    {
        void SaveObject(string obj, string key);
        string GetObject(string key);
        void ClearObject(string key);
    }
}