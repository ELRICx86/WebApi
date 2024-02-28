using Backend.Models;

namespace Backend.Repository.Auth
{
    public interface IAuth
    {
        Task<bool> Login(string username, string password);
        Task<bool> Registration(User user);
        
    }
}
