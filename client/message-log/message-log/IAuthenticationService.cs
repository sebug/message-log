using System;
namespace message_log
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(string username, string password);
        bool ChangePassword(string username, string newPassword);
    }
}
