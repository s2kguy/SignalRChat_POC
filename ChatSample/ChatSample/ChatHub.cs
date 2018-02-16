
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRChatSample
{
    
    public class ChatHub : Hub {


        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("broadcastMessage", name, message, Context.ConnectionId);
            // Clients.Group.
            Console.Write("Message Sent");
        }

        public void SendDM(string name, string message, string groupName)
        {
            Clients.Group(groupName).InvokeAsync("broadcastMessage", name, message);

        }

        public void GetMyId(string name)
        {
            Clients.Client(Context.ConnectionId).InvokeAsync("broadcastMessage", name, Context.ConnectionId);
        }

        public string MyId()
        {
            return Context.ConnectionId;
        }

        public void AddToGroup(string connectionId)
        {
            var groupName = GroupName();
            Groups.AddAsync(connectionId, groupName);
            Groups.AddAsync(Context.ConnectionId, groupName);
            // Clients.Group(groupName).InvokeAsync("groupJoined",groupName);
            Console.Write("GroupName: " + groupName);
        }

       /* public void AddToGroup(string name)
        {
            
            var groupName = GroupName();
            //Groups.AddAsync(connectionId, groupName);
            Groups.AddAsync(Context.ConnectionId, groupName);
            Console.WriteLine("GroupName: " + groupName);
        }*/

        public Task OnConnection => base.OnConnectedAsync();

        // Generates a random string to be used 
        // as a group name
        private string GroupName()
        {
            var randy = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 9)
                .Select(x => pool[randy.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}