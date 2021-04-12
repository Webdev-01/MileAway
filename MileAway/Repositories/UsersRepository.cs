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
        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email">Email of a user</param>
        /// <returns>Selected user or NULL if no user</returns>
        public static Users GetUserByEmail(string email)
        {
            using var connect = DbUtils.GetDbConnection();

            var users = connect.QuerySingleOrDefault<Users>("SELECT * FROM users WHERE Email = @Email",
                new
                {
                    Email = email
                });
            return users;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>True if success</returns>
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

                return registerUser == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static bool UpdateUser(Users user)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var updateUser = connect.Execute("UPDATE users SET Password = @Password, First_Name = @Firstname, Last_Name = @Lastname, User_Image = @User_Image", new
                {
                    Password = user.Password,
                    Firstname = user.First_Name,
                    Lastname = user.Last_Name,
                    User_image = user.User_Image
                });

                return updateUser != 0;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
