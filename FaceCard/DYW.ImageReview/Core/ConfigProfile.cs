using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using System.Configuration;
using Common;

namespace DYW.ImageReview.Core
{
    class ConfigProfile
    {
        private static ConfigProfile _current = new ConfigProfile();

        public string ReceiveCom { get; set; }

        public string OutputCom { get; set; }

        public string FaceServerIp { get; set; }

        public string CameraInIp { get; set; }

        public string CameraOutIp { get; set; }

        public bool AutoRun { get; set; }

        public int Timeout { get; set; }

        public string VideoInName { get; set; }

        public string VideoOutName { get; set; }

        private ConfigProfile()
        {
            VideoInName = "dyw-in";
            VideoOutName = "dyw-out";
        }

        public static ConfigProfile Current
        {
            get
            {
                return _current;
            }
        }

        public void ReadConfig()
        {
            ReceiveCom = GetKey("receivecom");
            OutputCom = GetKey("outputcom");
            FaceServerIp = GetKey("faceServerIp");
            CameraInIp = GetKey("cameraInIp");
            CameraOutIp = GetKey("cameraOutIp");
            AutoRun = (GetKey("auto").ToInt32() == 1);
            Timeout = GetKey("timeout").ToInt32();
        }

        private string GetKey(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                var val = ConfigurationManager.AppSettings[key];
                LogHelper.Info(string.Format("参数[{0}]={1}", key, val));
                return val;
            }
            else
            {
                LogHelper.Info(string.Format("参数[{0}]未读取", key));
                return string.Empty;
            }
        }
    }
}
