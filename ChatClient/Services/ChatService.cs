using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ChatClient.Services
{
    class ChatService : IChatService
    {
        private IHubProxy hubProxy;
        private HubConnection connection;
        private string Url = "https://localhost:44397";
        public event Action<string, string> NewTextMessageRecived;
        public event Action<string, string, string> NewTextMessageFromGroup;
        public event Action Logged;
        public event Action<List<string>> ShowAllUsers;
        public void Connect()
        {
            connection = new HubConnection(Url);
            
            hubProxy = connection.CreateHubProxy("chatHub");
            hubProxy.On("Logged", () => Logged());
            hubProxy.On<List<string>>("ShowAllUsers", (allUsers) => ShowAllUsers(allUsers));
            hubProxy.On<string, string>("addNewMessageToPage", (name, message) => NewTextMessageRecived(name, message));
            hubProxy.On<string, string, string>("AddNewMessageFromGroup", (groupname, sender, message) => NewTextMessageFromGroup(groupname, sender, message));
            
            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    MessageBox.Show("Сервер врменно недоступен", "Сообщение");
                }
            }).Wait();
        }
        public void SendTextMessage(string name, string sender, string message)
        {
            hubProxy.Invoke("Send", name, sender, message);
        }
        public void LogIn(string name, string password)
        {
            hubProxy.Invoke("LogIn", name, password).Wait();
        }

        public void GetAllUsers()
        {
            hubProxy.Invoke("GetAllUsers").Wait();
        }

        public void CreateGroup(string groupName, IList<string> usernames)
        {
            hubProxy.Invoke("CreateGroup", groupName, usernames).Wait();
        }
    }
}
