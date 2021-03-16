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
        public static List<Users> GetUsers()
        {
            using var connect = DbUtils.GetDbConnection();

            var users = connect.Query<Users>("SELECT * FROM users").ToList();
            return users;
        }

        public static Users GetUserByLogin(Users user)
        {
            using var connect = DbUtils.GetDbConnection();
            var userVars = connect.QuerySingleOrDefault<Users>("SELECT * FROM users WHERE Email=@Email",
            new
            {
                Email = user.Email
            });
            return userVars;
        }

        public static Users GetUserByID(int userid)
        {
            using var connect = DbUtils.GetDbConnection();

            var users = connect.QuerySingleOrDefault<Users>("SELECT * FROM users WHERE User_ID = @user_id", 
                new { 
                    user_id = userid
                }

                );
            return users;
        }

        public static bool RegisterUser(Users user)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var registerUser = connect.Execute("INSERT INTO users (Email, Password, First_Name, Last_Name, User_Image) VALUES (@Email, @Password, @Firstname, @Lastname, @User_image)", new
                {
                    Email = user.Email,
                    Password = user.Password,
                    Firstname = user.First_Name,
                    Lastname = user.Last_Name,
                    User_image = user.User_Image
                });

                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
