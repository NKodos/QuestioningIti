using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questioning.Data
{
    public interface IDataProvider
    {
        string ConnectionString { get; }

        void ExecuteNoQuery(string query, CommandType commandType);
        void ExecuteNoQuery(string query, ParameterSetupFunction parameterSetupFunction);
        void ExecuteNoQuery(string query, CommandType commandType, ParameterSetupFunction parameterSetupFunction);

        object ExecuteScalar(string query, CommandType commandType);
        object ExecuteScalar(string query, ParameterSetupFunction parameterSetupFunction);
        object ExecuteScalar(string query, CommandType commandType, ParameterSetupFunction parameterSetupFunction);
    }
}
