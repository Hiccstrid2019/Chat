﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

public class ChatHub : Hub
{
    private readonly UserService _service;
    
    public ChatHub(UserService service)
    {
        _service = service;
    }
    
    private static readonly Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();
    private static readonly List<string> NamesGroup = new List<string>();
    
    public async Task LogIn(string name, string password)
    {
        var user = _service.GetUser(name);
        if (user != null && (user.UserName == name && user.Password == password))
        {
            if (ConnectedUsers.ContainsKey(name))
            {
                ConnectedUsers.Remove(name);
            }

            ConnectedUsers.Add(name, Context.ConnectionId);
            await Clients.Caller.SendAsync("Logged");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var delUser = ConnectedUsers.First(connId => connId.Value == Context.ConnectionId);
        ConnectedUsers.Remove(delUser.Key);
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task Send(string name, string sender, string message)
    {
        var user = _service.GetUser(sender);
        if (user != null)
        {
            if (NamesGroup.Contains(name))
            {
                await Clients.Group(name).SendAsync("AddNewMessageFromGroup", name, sender, message);
            }
            else
            {
                await Clients.Client(ConnectedUsers[name]).SendAsync("addNewMessageToPage", sender, message);
            }
        }
    }

    public void GetAllUsers()
    {
        Clients.Caller.SendAsync("ShowAllUsers", _service.GetUserNames());
    }

    public async Task CreateGroup(string groupName, List<string> usernames)
    {
        NamesGroup.Add(groupName);
        foreach (string name in usernames)
        {
            if (ConnectedUsers.ContainsKey(name))
            {
                await Groups.AddToGroupAsync(ConnectedUsers[name], groupName);
            }
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}