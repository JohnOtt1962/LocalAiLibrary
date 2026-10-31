using Microsoft.AspNetCore.Http;
using System.Text;

namespace LocalAiLibrary.AiLibrary.AiSessionService
{
    public class SessionStore(IHttpContextAccessor httpContextAccessor) : ISessionStore
    {
        private ISession Session => httpContextAccessor.HttpContext!.Session;

        public void SaveObject(string key, string obj)
        {
            Session.SetString(key, obj);
        }

        public string GetObject(string key) 
        {
            return Session.GetString(key);
        }

        public void ClearObject(string key)
        {
            Session.Remove(key);
        }
    }
}