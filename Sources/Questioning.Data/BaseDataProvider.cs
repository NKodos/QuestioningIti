using System;
using System.Data;
using System.Data.Common;

namespace Questioning.Data
{
    public abstract class BaseDataProvider : IDataProvider
    {
        public BaseDataProvider(string connnetctionString)
        {
            ConnectionString = connnetctionString;
        }

        public string ConnectionString { get; }


        protected abstract DbConnection CreateConnection();
        protected abstract DbCommand CreateCommand(string query, CommandType commandType);


        public void ExecuteNoQuery(string query, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNoQuery(string query, ParameterSetupFunction parameterSetupFunction)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNoQuery(string query, CommandType commandType, ParameterSetupFunction parameterSetup)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var cmd = CreateCommand(query, commandType))
                {
                    cmd.Connection = connection;
                    parameterSetup(cmd);
                    cmd.ExecuteNonQuery();
                }
            }
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
    }
}
