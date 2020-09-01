using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNow.AutoJsServer
{
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public Task AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
            return Task.CompletedTask;
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out var socket);
            if (socket == null)
            {
                return;
            }

            try
            {
                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Closed by the ConnectionManager",
                    cancellationToken: CancellationToken.None);
            }
            catch
            {
                // ignored
            }
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}