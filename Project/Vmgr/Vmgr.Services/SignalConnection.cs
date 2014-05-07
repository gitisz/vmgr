using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Vmgr.Services
{
    public class SignalConnection : PersistentConnection
    {
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            // Broadcast data to all clients
            return Connection.Broadcast(data);
        }
    }
}
