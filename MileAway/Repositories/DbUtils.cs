using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MileAway.Repositories
{
    public class DbUtils
    {
        public static IDbConnection GetDbConnection()
        {
            return new MySqlConnection(
                "Server = localhost;" +
                "Port = 3306;" +
                "Database = mileaway;" +
                "Uid = root;" +
                "Pwd =;");
        }
    }
}
