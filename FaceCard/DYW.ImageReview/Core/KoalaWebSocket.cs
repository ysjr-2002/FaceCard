using Common;
using Common.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace DYW.ImageReview.Core
{
    class KoalaWebSocket
    {
        private WebSocket socket = null;
        private AutoResetEvent reset = new AutoResetEvent(false);
        public event EventHandler<RecognizedResult> OnRecognizedResult;

        public KoalaWebSocket()
        {
        }

        public bool IsConnected
        {
            get;
            set;
        }

        public bool Connect(string url, string session)
        {
            socket = new WebSocket(url);
            socket.WaitTime = TimeSpan.FromSeconds(5);
            socket.OnOpen += Socket_OnOpen;
            socket.OnError += Socket_OnError;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.Connect();
            reset.WaitOne();
            return IsConnected;
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            IsConnected = false;
            LogHelper.Info("Socket_OnClose");
            reset.Set();
        }

        public void Disconnect()
        {
            IsConnected = false;
            socket?.Close();
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            IsConnected = false;
            LogHelper.Info("Socket_OnError");
            reset.Set();
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                //var entity = e.Data.Deserialize<RecognizedResult>();
                //if (entity.type == RecognizeState.recognized.ToString())
                //{
                //    OnRecognizedResult?.Invoke(this, entity);
                //}
                //else
                //{
                //    Console.WriteLine(entity.type);
                //}
                return;
            }
            else if (e.IsBinary)
            {
                return;
            }
            else if (e.IsPing)
            {
                return;
            }
        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
            IsConnected = true;
            LogHelper.Info("Socket_OnOpen");
            reset.Set();
        }
    }

    /// <summary>
    /// 识别状态
    /// </summary>
    public enum RecognizeState
    {
        recognizing,

        recognized,

        unrecognized,

        gone,
    }
}
