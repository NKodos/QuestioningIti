using Questioning.Data.EventArgs;
using System;
using System.Data;
using System.Data.Common;

namespace Questioning.Data
{
    public abstract class BaseDataProvider : IDataProvider
    {
        protected BaseDataProvider(string connnetctionString)
        {
            ConnectionString = connnetctionString ?? throw new ArgumentNullException(nameof(connnetctionString));
        }

        public delegate void LogSetupFunction(DbCommand command);
        public delegate void CommandExecuteHandler(object sender, CommandExecuteArgs eventArgs);
        public event CommandExecuteHandler CommandExecute;

        public string ConnectionString { get; }

        protected abstract DbConnection CreateConnection();
        protected abstract DbDataAdapter CreateDataAdapter();
        protected abstract DbCommand CreateCommand(string query, CommandType commandType);


        public void ExecuteNoQuery(string query, CommandType commandType)
        {
            ExecuteNoQuery(query, commandType, command => { });
        }

        public void ExecuteNoQuery(string query, ParameterSetupFunction parameterSetupFunction)
        {
            ExecuteNoQuery(query, CommandType.StoredProcedure, parameterSetupFunction);
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
            return ExecuteScalar(query, commandType, command => { });
        }

        public object ExecuteScalar(string query, ParameterSetupFunction parameterSetupFunction)
        {
            return ExecuteScalar(query, CommandType.StoredProcedure, parameterSetupFunction);
        }

        public object ExecuteScalar(string query, CommandType commandType, ParameterSetupFunction parameterSetupFunction)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            object value;

            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var cmd = CreateCommand(query, commandType))
                {
                    cmd.Connection = connection;
                    parameterSetupFunction(cmd);
                    value = cmd.ExecuteScalar();
                }
            }

            return value;
        }

        private void OnCommandExecute(CommandExecuteArgs eventArgs)
        {
            var handler = CommandExecute;
            handler?.Invoke(this, eventArgs);
        }
    }
}
