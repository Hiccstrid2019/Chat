using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using ChatServer.DAL;
using System.Threading.Tasks;

namespace ChatServer
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        //private UsersContext userContext = new UsersContext();
        IRepository repository;
        public ChatHub(IRepository repositoryParam)
        {
            repository = repositoryParam;
        }
        private static readonly Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();
        private static List<string> NamesGroup = new List<string>();
        public void LogIn(string name, string password)
        {
            var user = repository.Get(name);
            if (user != null && (user.UserName == name && user.Password == password))
            {
                if (ConnectedUsers.ContainsKey(name))
                {
                    ConnectedUsers.Remove(name);
                }

                ConnectedUsers.Add(name, Context.ConnectionId);
                Clients.Caller.Logged();
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var delUser = ConnectedUsers.First(connId => connId.Value == Context.ConnectionId);
            ConnectedUsers.Remove(delUser.Key);
            return base.OnDisconnected(stopCalled);
        }
        public void Send(string name, string sender, string message)
        {
            var user = repository.Get(sender);
            if (user != null)
            {
                if (NamesGroup.Contains(name))
                {
                    Clients.Group(name, Context.ConnectionId).AddNewMessageFromGroup(name, sender, message);
                }
                else
                {
                    Clients.Client(ConnectedUsers[name]).addNewMessageToPage(sender, message);
                }
            }
        }

        public void GetAllUsers()
        {
            Clients.Caller.ShowAllUsers(repository.GetAllUserNames());
        }

        public void CreateGroup(string GroupName, List<string> Usernames)
        {
            NamesGroup.Add(GroupName);
            foreach (string name in Usernames)
            {
                if (ConnectedUsers.ContainsKey(name))
                {
                    Groups.Add(ConnectedUsers[name], GroupName);
                }
            }
            Groups.Add(Context.ConnectionId, GroupName);
        }
    }
}