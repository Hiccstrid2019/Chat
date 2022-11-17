using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Services
{
    class ChatService : IChatService
    {
        private HubConnection _connection;
        private readonly string _url = "https://localhost:44366/chatHub";
        public event Action<string, string> NewTextMessageRecived;
        public event Action<string, string, string> NewTextMessageFromGroup;
        public event Action Logged;
        public event Action<List<string>> ShowAllUsers;
        public void Connect(string myAccessToken)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(_url, options =>
                {
                    options.AccessTokenProvider = (() => Task.FromResult(myAccessToken));
                }).Build();

            if (Logged != null) Logged();
            _connection.On<List<string>>("ShowAllUsers", (allUsers) => ShowAllUsers(allUsers));
            _connection.On<string, string>("addNewMessageToPage", (name, message) => NewTextMessageRecived(name, message));
            _connection.On<string, string, string>("AddNewMessageFromGroup", (groupname, sender, message) => NewTextMessageFromGroup(groupname, sender, message));
            
            _connection.StartAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    MessageBox.Show("Сервер врменно недоступен", "Сообщение");
                }
            }).Wait();
        }
        public async void SendTextMessage(string name, string sender, string message)
        {
            await _connection.InvokeAsync("Send", name, sender, message);
        }
        public async void LogIn(string name, string password)
        {
            var client = new HttpClient();
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", name),
                new KeyValuePair<string, string>("password", password)
            });
            var result = await client.PostAsync(" https://localhost:44366/gettoken", formData);
            string token = await result.Content.ReadAsStringAsync();
            Connect(token);
        }

        public async void GetAllUsers()
        {
            await _connection.InvokeAsync("GetAllUsers");
        }

        public async void CreateGroup(string groupName, IList<string> usernames)
        {
            await _connection.InvokeAsync("CreateGroup", groupName, usernames);
        }
    }
}
