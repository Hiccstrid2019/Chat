using System.Collections.Generic;

namespace ChatClient.Services
{
    public interface IChatService
    {
        void Connect();
        void SendTextMessage(string name, string sender, string message);
        void LogIn(string name, string password);
        void GetAllUsers();
        void CreateGroup(string groupName, IList<string> usernames);
    }
}
