using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Server
{
    public class MessageEventArgs : EventArgs
    {
        public string Ip { get; set; }

        public byte[] CardBytes { get; set; }

        public int CardNo { get; set; }
    }
}
