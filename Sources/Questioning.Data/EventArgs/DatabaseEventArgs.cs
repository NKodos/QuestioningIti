using System.Security.AccessControl;

namespace Questioning.Data.EventArgs
{
    public class DatabaseEventArgs : System.EventArgs { }

    public class CommandExecuteArgs : DatabaseEventArgs
    {
        public string Sql { get; set; }

        public long Duration { get; set; }  
    }
}
