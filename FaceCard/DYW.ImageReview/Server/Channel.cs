using Common;
using DYW.ImageReview.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Server
{
    public class Channel
    {
        static Channel()
        {
            Channels = new ChannelCollection();
        }

        public string CardReaderIp { get; set; }

        public string OutputIp { get; set; }

        public static ChannelCollection Channels { get; set; }

        public static void Init()
        {
            var channel1 = ConfigurationManager.AppSettings["channel1"];
            var channel2 = ConfigurationManager.AppSettings["channel2"];

            Channel entity1 = GetChannel(channel1);
            Channel entity2 = GetChannel(channel2);

            Channels.Add(entity1);
            Channels.Add(entity2);
        }

        private static Channel GetChannel(string val)
        {
            if (val == null)
                return null;

            var split = val.Split('|');
            if (split.Length < 2)
                return null;

            Channel channel = new Channel
            {
                CardReaderIp = split[0],
                OutputIp = split[1],
            };
            return channel;
        }

        /// <summary>
        /// 获取视频流名称
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetVideoUrl(string ip)
        {
            if (Channels.First().CardReaderIp == ip)
            {
                return ConfigProfile.Current.CameraInIp;
            }
            else
            {
                return ConfigProfile.Current.CameraOutIp;
            }
        }

        public static string GetPassType(string ip)
        {
            if (Channels[0].CardReaderIp == ip)
                return "进入";
            else
                return "离开";
        }

        public static void Open(string readerIp, byte[] data)
        {
            var c = Channels.FirstOrDefault(s => (s != null && s.CardReaderIp == readerIp));
            if (c != null)
            {
                Door.Open(c.OutputIp, data);
            }
        }
    }

    public class ChannelCollection : List<Channel>
    {
    }
}
