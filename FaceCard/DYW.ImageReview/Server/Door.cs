using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Server
{
    public static class Door
    {
        private const int port = 9875;

        /// <summary>
        /// 输入卡号
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="cardBytes"></param>
        public static void Open(string ip, byte[] cardBytes)
        {
            UdpClient udp = new UdpClient();
            udp.Send(cardBytes, cardBytes.Length, new IPEndPoint(IPAddress.Parse(ip), port));
        }
    }
}
