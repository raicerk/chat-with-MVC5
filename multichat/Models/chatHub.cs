using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace multichat.Models
{
    public class ChatHub : Hub
    {
        public static List<Client> ConnectedUsers { get; set; } = new List<Client>();

        public void Connect(string username, string nombre)
        {
            Client c = new Client()
            {
                Username = username,
                ID = Context.ConnectionId,
                Nombre = nombre
            };

            ConnectedUsers.Add(c);
            Clients.All.updateUsers(ConnectedUsers.Count(), ConnectedUsers.Select(u => u));
        }


        public void Send(string IdUsuario, string message)
        {
            List<string> usu = new List<string>();
            var sender = ConnectedUsers.First(u => u.ID.Equals(Context.ConnectionId));
            //Clients.All.broadcastMessage(sender.Username, sender.ID , message);
            usu.Add(sender.ID);
            usu.Add(IdUsuario);
            Clients.Clients(usu).broadcastMessage(sender.Username, sender.ID ,message);
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var disconnectedUser = ConnectedUsers.FirstOrDefault(c => c.ID.Equals(Context.ConnectionId));
            ConnectedUsers.Remove(disconnectedUser);
            Clients.All.updateUsers(ConnectedUsers.Count(), ConnectedUsers.Select(u => u.Username));
            return base.OnDisconnected(stopCalled);
        }
    }

    public class Client
    {
        public string Username { get; set; }
        public string ID { get; set; }
        public string Nombre { get; set; }
    }
}