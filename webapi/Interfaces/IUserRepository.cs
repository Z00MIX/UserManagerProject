using webapi.Models;

namespace webapi.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int idUser);
        IEnumerable<User>GetAllUsers();
        User GetUserById(int id);
    }
}