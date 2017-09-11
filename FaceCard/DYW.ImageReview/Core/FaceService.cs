using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Diagnostics;
using Common.Log;

namespace DYW.ImageReview.Core
{
    class FaceService
    {
        static string old_suffix = "video_verify";
        static string new_suffix = "video/verify";
        public static FaceCompare Compare(string photo, string videoName)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("name", videoName);
            param.Add("timeout", ConfigProfile.Current.Timeout.ToString());
            //param.Add("url", ConfigProfile.Current.CameraInIp.UrlEncode());

            var url = string.Concat("http://" + ConfigProfile.Current.FaceServerIp + ":8080", "/"+ old_suffix);
            var data = photo.FileToByte();
            var error = string.Empty;
            Stopwatch sw = Stopwatch.StartNew();
            var responsestr = HttpMethod.Post(url, data, param, out error);
            sw.Stop();
            LogHelper.Info("比对->" + sw.ElapsedMilliseconds);
            if (!error.IsEmpty())
            {
                LogHelper.Info("调用服务异常->" + error);
            }
            FaceCompare face = responsestr.Deserialize<FaceCompare>();
            return face;
        }
    }
}
