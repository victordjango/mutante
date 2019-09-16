using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using myMicroservice.Configuration;

namespace myMicroservice.Repostories
{
    public static class DBConnection
    {
        private static SqlConnection _connection;

        public static SqlConnection ConnectedInstance {
            get
            {

                if (_connection == null)
                {
                    _connection = new SqlConnection(AppConfiguration.Instance.SqlDataConnection);
                }

                lock (_connection)
                {

                    if (_connection.State == System.Data.ConnectionState.Broken)
                    {
                        _connection = new SqlConnection(AppConfiguration.Instance.SqlDataConnection);
                    }
                    if (_connection.State == System.Data.ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                }

                return _connection;
            }
        }
        public static SqlConnection GetNewConnection()
        {
            return new SqlConnection(AppConfiguration.Instance.SqlDataConnection);
        }
    }
}
