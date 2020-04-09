using MySql.Data.MySqlClient;
using System.Data;

namespace Questioning.Data.MySql
{
    public class Database : Data.DataBase
    {
        #region Constructors

        public Database(string connectionString)
            :this(ConnectionSettings.FromConnectionString(connectionString)) { }

        public Database(ConnectionSettings connectionSettings)
        {
            ConnectionSettings = connectionSettings;
            DataProvider = new MySqlDataProvider(connectionSettings.ToString());
        }

        #endregion


        #region Properties

        public override IDataProvider DataProvider { get; }

        public ConnectionSettings ConnectionSettings { get; }

        #endregion

        #region Repositories



        #endregion
    }
}
