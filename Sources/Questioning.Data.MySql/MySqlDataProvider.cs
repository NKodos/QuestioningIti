using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Questioning.Data.MySql
{
    public class MySqlDataProvider : IDataProvider
    {
        public MySqlDataProvider(string connnetctionString)
        {
            ConnectionString = connnetctionString;
        }

        public string ConnectionString { get; }

        #region PublicMethods
        public void ExecuteNoQuery(string query, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNoQuery(string query, ParameterSetupFunction parameterSetupFunction)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNoQuery(string query, CommandType commandType, ParameterSetupFunction parameterSetupFunction)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string query, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string query, ParameterSetupFunction parameterSetupFunction)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string query, CommandType commandType, ParameterSetupFunction parameterSetupFunction)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region ProtectedMethods
        protected MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        protected MySqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            return new MySqlCommand(commandText)
            {
                CommandTimeout = 0,
                CommandText = commandText,
                CommandType = commandType
            };
        }
        #endregion
    }
}
