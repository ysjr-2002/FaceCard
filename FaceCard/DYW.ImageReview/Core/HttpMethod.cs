using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Web.Script.Serialization;

namespace DYW.ImageReview.Core
{
    class HttpMethod
    {
        private const int http_timeout = 15000;

        public static HttpWebRequest PostImage(string url, string boundary)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.Timeout = 150000;
            wr.KeepAlive = false;
            return wr;
        }

        public static string Post(string url, byte[] data, Dictionary<string, string> param, out string error)
        {
            error = string.Empty;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)PostImage(url, boundary);
            WebResponse response = null;

            StringBuilder sb = new StringBuilder();
            try
            {
                var rs = request.GetRequestStream();
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in param.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, param[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }

                //文件开始
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                //图片
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, "image", "image.jpg", "text/plain");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);
                rs.Write(data, 0, data.Length);
                //文件结束
                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    var responseStream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(responseStream);
                    var content = sr.ReadToEnd();
                    sr.Close();
                    return content;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return string.Empty;
            }
        }
    }
}
