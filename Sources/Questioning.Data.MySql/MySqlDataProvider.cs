using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace Questioning.Data.MySql
{
    public class MySqlDataProvider : BaseDataProvider
    {
        public MySqlDataProvider(string connnetctionString) : base(connnetctionString)
        {
        }

        protected override DbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        protected override DbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        protected override DbCommand CreateCommand(string commandText, CommandType commandType)
        {
            return new MySqlCommand(commandText)
            {
                CommandTimeout = 0,
                CommandText = commandText,
                CommandType = commandType
            };
        }
    }
}
