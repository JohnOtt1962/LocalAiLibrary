using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AiDbService
{
    public class DbOps
    {
        public EntityOps GetEntityOps(string commandText, string connectionString)
        {
            return new EntityOps
            {
                CommandText = commandText,
                ConnectionString = connectionString,
                IsStoredProc = true
            };
        }

        public ParamItem GetParamItem(string name, string value, bool isOutput, string type, int size)
        {
            ParamItem parm = new ParamItem()
            {
                Name = name,
                Value = value,
                IsOutput = isOutput
            };

            if (isOutput)
            {
                parm.Size = size;
                parm.Type = type;
            }

            return parm;
        }
    }
}