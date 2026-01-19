namespace SemProg.BLL.Interfaces
{
    public interface IUserService
    {
        bool Validate(string username, string password);
        bool IsAdmin(string username);
        int GetId(string username);
    }
}