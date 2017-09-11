using Common;
using Common.Log;
using Common.NotifyBase;
using DYW.ImageReview.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DYW.ImageReview.Core
{
    class FaceCore : PropertyNotifyObject
    {
        private SerialDevice receiveSerial = null;
        private SerialDevice outputSerial = null;
        private OutputController output = null;
        private FuncTimeout delayCall = null;
        private const int delayIntervel = 3000;

        private KoalaWebSocket socket = new KoalaWebSocket();

        private static readonly string WaitCard = "请刷卡...";
        private static readonly string Comparing = "正在比对，请稍等...";

        const int compare_ok = 78;
        const int server_port = 9876;

        private UdpComServer server = null;

        public FaceCore()
        {
            Employee = new Employee();
            delayCall = new FuncTimeout();
            server = new UdpComServer(server_port);
        }

        public Employee Employee
        {
            get; set;
        }

        public Visibility ReadyVisibility
        {
            get { return this.GetValue(s => s.ReadyVisibility); }
            set { this.SetValue(s => s.ReadyVisibility, value); }
        }

        public string ReadyContent
        {
            get { return this.GetValue(s => s.ReadyContent); }
            set { this.SetValue(s => s.ReadyContent, value); }
        }

        public async void Init()
        {
            ReadyContent = "初始化...";

            #region 串口方式
            if (!ConfigProfile.Current.ReceiveCom.IsEmpty())
            {
                receiveSerial = new SerialDevice();
                outputSerial = new SerialDevice();
                var open = receiveSerial.Open(ConfigProfile.Current.ReceiveCom);
                if (open)
                {
                    receiveSerial.OnReadCard += OnReadCard;
                }
                receiveSerial.Start();

                outputSerial.Open(ConfigProfile.Current.OutputCom);
                output = new OutputController(outputSerial);
            }
            #endregion

            //将Nuc和摄像机Ip进行映射
            var url_in = "ws://" + ConfigProfile.Current.FaceServerIp + ":8080/video" + "?name=" + ConfigProfile.Current.VideoInName + "&url=" + ConfigProfile.Current.CameraInIp.UrlEncode();
            var taskIn = WebSocket(url_in);
            await taskIn;

            var url_out = "ws://" + ConfigProfile.Current.FaceServerIp + ":8080/video" + "?name=" + ConfigProfile.Current.VideoOutName + "&url=" + ConfigProfile.Current.CameraOutIp.UrlEncode();
            var taskOut = WebSocket(url_out);
            await taskOut;

            RunServer();

            if (socket.IsConnected)
                ReadyContent = WaitCard;
            else
                ReadyContent = "设备连接失败";
        }

        private void RunServer()
        {
            server.OnMessageInComming += Server_OnMessageInComming;
            server.Start();
        }

        private void Server_OnMessageInComming(object sender, MessageEventArgs e)
        {
            var videoName = Channel.GetVideoName(e.Ip);
            var compare = doComapre(e.CardNo, videoName);
            if (compare)
            {
                Channel.Open(e.Ip, e.CardBytes);
            }
        }

        public Task<bool> WebSocket(string url)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                if (socket == null)
                    socket = new KoalaWebSocket();

                var connect = socket.Connect(url, "");
                return connect;
            });
        }

        #region 串口方式
        public void OnReadCard(byte[] cardBuffer, int cardNo)
        {
            var compare = doComapre(cardNo, "dyw-in");
            if (compare)
            {
                //执行开闸
                output.Write(cardBuffer);
            }
        }
        #endregion

        private bool doComapre(int cardNo, string videoName)
        {
            var card = CardPhoto.FindCardByNo(cardNo);
            if (card == null)
            {
                ReadyContent = "未找到卡对应的人像信息->" + cardNo;
                LogHelper.Info(string.Format("error->未找到卡[{0}]对应的信息", cardNo));
                RestoreInit();
                return false;
            }

            Employee.EmpName = card.EmpName;
            Employee.EmpCard = card.EmpCard;
            Employee.EmpPhoto = card.EmpPhoto;
            ReadyContent = Comparing;
            ReadyVisibility = Visibility.Collapsed;

            var compare = false;
            var data = FaceService.Compare(card.EmpPhoto, videoName);
            if (data != null)
            {
                var snapPath = "";
                if (data.score >= compare_ok && data.result != null)
                {
                    snapPath = FileManager.SaveFile(cardNo, data.result.face.image);
                    //Employee.EmpSnap = snapPath;
                    compare = true;
                    //Employee.Result = "相同";
                    LogHelper.Info(string.Format("[{0}]复核通过，抓图:{1}", card.EmpCard, snapPath));
                }
                else
                {
                    compare = false;
                    //Employee.Result = "不同";
                    LogHelper.Info(string.Format("[{0}]复核失败，抓图:{1}", card.EmpCard, snapPath));
                }
            }
            RestoreInit();
            return compare;
        }

        private void RestoreInit()
        {
            delayCall.StartOnce(delayIntervel, () =>
            {
                Employee.EmpPhoto = null;
                //Employee.EmpSnap = null;
                //Employee.Result = "";
                ReadyContent = WaitCard;
                ReadyVisibility = Visibility.Visible;
            });
        }

        public void Dispose()
        {
            receiveSerial?.Close();
            outputSerial?.Close();
            socket?.Disconnect();
            server?.Stop();
        }
    }
}
