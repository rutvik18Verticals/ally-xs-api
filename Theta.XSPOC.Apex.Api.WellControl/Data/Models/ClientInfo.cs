using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Models
{
    /// <summary>
    /// This class represents the global list of websockets and operates on the <seealso cref="WebSocket"/>
    /// to send message to client.
    /// </summary>
    public static class ClientInfo
    {

        /// <summary>
        /// The global list of websocket with socketid.
        /// </summary>
        public static readonly ConcurrentDictionary<string, WebSocket> Sockets = new();

        #region Public Methods

        /// <summary>
        /// Add the socketid to the list.
        /// </summary>
        /// <param name="socketId">The socket guid.</param>
        /// <param name="socket">The web socket.</param>
        /// <returns></returns>
        public static string AddSocket(string socketId, WebSocket socket)
        {
            Sockets.TryAdd(socketId, socket);

            return socketId;
        }

        /// <summary>        
        /// Send the message to the client identified by socketid.
        /// </summary>
        /// <param name="socketId">The socket guid.</param>
        /// <param name="message">The message to be sent.</param>
        public static async void Broadcast(string socketId, string message)
        {
            if (socketId == null)
            {
                return;
            }

            socketId = socketId.Replace("\"", "");

            if (Sockets.TryGetValue(socketId, out var websocket))
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        /// <summary>
        /// Removes the socketid from the list.
        /// </summary>
        /// <param name="socketId">The socket guid.</param>
        public static void RemoveSocketId(string socketId)
        {
            if (socketId == null)
            {
                return;
            }
            socketId = socketId.Replace("\"", "");

            Sockets.TryRemove(socketId, out _);
        }

        #endregion

    }
}
