using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questioning.Data.MySql
{
    public class Database
    {
        private MySqlConnection _connection = new MySqlConnection();
        private MySqlCommand _command = new MySqlCommand();

        public void ExecuteStoreProcedure()
        {
            _connection.Open();
            _command.Connection = _connection;

            _command.CommandText = "add_emp";
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.AddWithValue("@lname", "Jones");
            _command.Parameters["@lname"].Direction = ParameterDirection.Input;

            _command.Parameters.AddWithValue("@fname", "Tom");
            _command.Parameters["@fname"].Direction = ParameterDirection.Input;

            _command.Parameters.AddWithValue("@bday", "1940-06-07");
            _command.Parameters["@bday"].Direction = ParameterDirection.Input;

            _command.Parameters.AddWithValue("@empno", MySqlDbType.Int32);
            _command.Parameters["@empno"].Direction = ParameterDirection.Output;

            _command.ExecuteNonQuery();
        }
    }
}
