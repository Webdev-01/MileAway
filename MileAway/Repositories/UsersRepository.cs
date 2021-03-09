using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using MileAway.Models;

namespace MileAway.Repositories
{
    public class UsersRepository
    {
        private static IDbConnection Connect()
        {
            return new MySqlConnection(
                "Server = localhost;" +
                "Port = 3306;" +
                "Database = mileaway;" +
                "Uid = root;" +
                "Pwd =;");
        }

        public static List<Users> GetUsers()
        {
            using var connect = Connect();

            var users = connect.Query<Users>("SELECT * FROM users").ToList();
            return users;
        }

        public static Users GetUserByLogin(Users user)
        {
            using var connect = Connect();
            var userVars = connect.QuerySingleOrDefault<Users>("SELECT * FROM users WHERE EMAIL=@Email AND PASSWORD=@Password",
                new { Email = user.EMAIL,
                Password = user.PASSWORD });

            return userVars;
        }
    }
}
