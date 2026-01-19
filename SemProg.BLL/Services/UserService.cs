using SemProg.BLL.Interfaces;
using SemProg.DAL;
using Microsoft.EntityFrameworkCore;

namespace SemProg.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _ctx;
        public UserService(AppDbContext ctx) => _ctx = ctx;

        public bool Validate(string username, string password)
            => _ctx.Users.Any(u => u.Username == username && u.Password == password);

        public bool IsAdmin(string username)
            => _ctx.Users.Any(u => u.Username == username && u.Role == "admin");

        public int GetId(string username)
            => _ctx.Users.First(u => u.Username == username).Id;
    }
}