using System.Data.SqlClient;
using System.Text;

namespace Questioning.Data.MySql
{
    public sealed class ConnectionSettings
    {
        #region Constructors

        public ConnectionSettings(string server, string databaseName)
        {
            Server = server ?? string.Empty;
            DatabaseName = databaseName;
            Application = string.Empty;
            NetworkLibrary = string.Empty;
            Port = string.Empty;
        }

        public ConnectionSettings(string databaseName)
            : this("localhost", databaseName) { }

        public ConnectionSettings(string server,string databaseName, string user, string password)
            : this(server, databaseName)
        {
            User = user;
            Password = password;
        }

        #endregion

        #region Properties

        public string Server { get; private set; }
        public string Instance { get; private set; }
        public string DatabaseName { get; private set; }
        public string Application { get; private set; }
        public string NetworkLibrary { get; private set; }
        public string Port { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public int MaxPoolSize { get; private set; }
        public int MinPoolSize { get; private set; }
        public string DatabaseConnectionString => GetDatabaseConnectionString(DatabaseName);

        #endregion

        #region Static methods

        public static ConnectionSettings FromConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            string server;
            var instance = string.Empty;
            var values = builder.DataSource.Split(new[] { '\\' });

            if (values.Length > 1)
            {
                server = values[0];
                instance = values[1];
            }
            else
            {
                server = values[0];
            }

            var settings = new ConnectionSettings(builder.InitialCatalog)
            {
                Server = server,
                Instance = instance,
                Application = builder.ApplicationName,
                NetworkLibrary = builder.NetworkLibrary,
                User = builder.UserID,
                Password = builder.Password,
                MaxPoolSize = builder.MaxPoolSize,
                MinPoolSize = builder.MinPoolSize
            };
            return settings;
        }

        #endregion

        #region Methods

        private string GetDatabaseConnectionString(string catalog)
        {
            var resultBuilder = new StringBuilder(80);
            string databaseServer = Server.Length > 0 ? Server : ".";

            resultBuilder.AppendFormat("Server={0};", databaseServer);

            resultBuilder.AppendFormat("Database={0};", catalog);

            if (!string.IsNullOrEmpty(User))
            {
                resultBuilder.AppendFormat("Uid={0};Pwd={1};", User, Password);
            }

            if (!string.IsNullOrEmpty(Application))
            {
                resultBuilder.AppendFormat("Application Name={0};", Application);
            }

            if (!string.IsNullOrEmpty(NetworkLibrary))
            {
                resultBuilder.AppendFormat("Network Library={0};", NetworkLibrary);
            }

            return resultBuilder.ToString();
        }

        public override string ToString()
        {
            return DatabaseConnectionString;
        }

        #endregion
    }
}
