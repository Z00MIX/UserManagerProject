using System.Data;
using System.Data.SQLite;
using webapi.Interfaces;
using webapi.Models;

namespace webapi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public void CreateUser(User user)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                var query = "INSERT INTO User (UserName, Password, Email, Rol) VALUES (@UserName, @Password, @Email, @Rol)";
                using (var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@UserName",user.UserName));
                    command.Parameters.Add(new SQLiteParameter("@Password",user.Password));
                    command.Parameters.Add(new SQLiteParameter("@Email",user.Email));
                    command.Parameters.Add(new SQLiteParameter("@Rol",user.Rol));
                    command.ExecuteNonQuery();
                }
            }
        }
        public bool DeleteUser(int idUser)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                var query = "DELETE FROM User where @Id = Id";
                using (var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("Id",idUser));
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; 
                }
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                var query = "SELECT * FROM User";
                using (var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var users = new List<User>();
                        while (reader.Read())
                        {
                            users.Add(new User{
                                Id = Convert.ToInt32(reader["Id"]),
                                UserName = reader["UserName"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString(),
                                Rol = reader["Rol"].ToString()
                            });
                        }
                        return users;
                    }
                }
            }
        }
        public User GetUserById(int id)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                var query = "SELECT * FROM User WHERE Id = @Id";
                using (var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Id",id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User{
                                Id = Convert.ToInt32(reader["Id"]),
                                UserName = reader["UserName"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString(),
                                Rol = reader["Rol"].ToString()
                            };
                        }
                        return null; 
                    }
                }
            }
        }
        public bool UpdateUser(User user)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                var query = "UPDATE User set UserName = @UserName, Password = @Password, Email = @Email, Rol = @Rol WHERE Id = @Id";
                using (var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Id",user.Id));
                    command.Parameters.Add(new SQLiteParameter("@UserName",user.UserName));
                    command.Parameters.Add(new SQLiteParameter("@Password",user.Password));
                    command.Parameters.Add(new SQLiteParameter("@Email",user.Email));
                    command.Parameters.Add(new SQLiteParameter("@Rol",user.Rol));
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}