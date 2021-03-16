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
            var userVars = connect.QuerySingleOrDefault<Users>("SELECT * FROM users WHERE EMAIL=@Email",
            new { 
                Email = user.Email
            });
            return userVars;
        }

        public static bool RegisterUser(Users user)
        {
            using var connect = Connect();
            try
            {
                var registerUser = connect.Execute("INSERT INTO users (Email, Password, FirstName, LastName, User_Image) VALUES (@Email, @Password, @Firstname, @Lastname, @User_image)", new
                {
                    Email = user.Email,
                    Password = user.Password,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    User_image = user.User_Image
                });

            return true;
            } catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
