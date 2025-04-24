using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Models
{
    /// <summary>
    /// The web socket server class to send message to web client after async call to rabbit mq.
    /// </summary>
    public class WebSocketServer : IDisposable
    {

        private HttpListener _httpListener;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly IConfiguration _config;
        private readonly string WEB_SOCKET_URL = "http://localhost:5000/";

        #region Constructor

        /// <summary>
        /// Creates an instance of web socket server.
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WebSocketServer(IConfiguration config)
        {
            _config = config ??
                throw new ArgumentNullException(nameof(config));
        }

        #endregion

        /// <summary>
        /// Creates a web socket server context and sends a message to client.
        /// </summary>
        public async Task StartAsync()
        {
            string websocketURL = _config.GetSection("WebSocketURL").Value ??
                WEB_SOCKET_URL;

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(websocketURL);
            _httpListener.Start();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var context = await _httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        var webSocketContext = await context.AcceptWebSocketAsync(null);
                        var webSocket = webSocketContext.WebSocket;

                        var correlationId = Guid.NewGuid().ToString();
                        var buffer = Encoding.UTF8.GetBytes($"socketId:{correlationId}");

                        ClientInfo.AddSocket(correlationId, webSocket);

                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// The web socket server stop method.
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _httpListener.Close();
        }

        /// <summary>
        /// The web socket server dispose method.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
