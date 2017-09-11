using Common;
using Common.Log;
using Common.NotifyBase;
using DYW.ImageReview.Core;
using DYW.ImageReview.Entitys;
using DYW.ImageReview.Model;
using DYW.ImageReview.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DYW.ImageReview.Core
{
    class MainViewModelNew : PropertyNotifyObject
    {
        private FuncTimeout delayCall = null;
        private const int delayIntervel = 2000;

        private KoalaWebSocket webSocketIn = null;
        private KoalaWebSocket webSocketOut = null;

        private static readonly string WaitCard = "请刷卡...";
        private static readonly string Comparing = "正在比对，请稍等...";

        const int compare_ok = 78;
        const int server_port = 9876;

        private const int MAX_HISTORY_COUNT = 20;

        private UdpComServer server = null;
        private DispatcherTimer _dispatchTimer = null;

        private SerialDevice receiveSerial = null;
        private SerialDevice outputSerial = null;
        private OutputController output = null;

        public MainViewModelNew()
        {
            PassHistoryCollection = new ObservableCollection<Compare>();
            Current = new Compare();
            delayCall = new FuncTimeout();
            server = new UdpComServer(server_port);
            DataStat = new DataStat();
            CompareVisibility = Visibility.Collapsed;
            ReadyVisibility = Visibility.Visible;
        }

        public ObservableCollection<Compare> PassHistoryCollection
        {
            get { return this.GetValue(s => s.PassHistoryCollection); }
            set { this.SetValue(s => s.PassHistoryCollection, value); }
        }

        public Compare Current
        {
            get; set;
        }

        public DataStat DataStat
        {
            get; set;
        }

        public Visibility ReadyVisibility
        {
            get { return this.GetValue(s => s.ReadyVisibility); }
            set { this.SetValue(s => s.ReadyVisibility, value); }
        }

        public Visibility CompareVisibility
        {
            get { return this.GetValue(s => s.CompareVisibility); }
            set { this.SetValue(s => s.CompareVisibility, value); }
        }

        public string ReadyContent
        {
            get { return this.GetValue(s => s.ReadyContent); }
            set { this.SetValue(s => s.ReadyContent, value); }
        }

        public string DateTimeInfo
        {
            get { return this.GetValue(s => s.DateTimeInfo); }
            set { this.SetValue(s => s.DateTimeInfo, value); }
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

            DataStat.Init();

            ShowTimeInfo(DateTime.Now);
            StartTimer();

            //将Nuc和摄像机Ip进行映射
            var url_in = "ws://" + ConfigProfile.Current.FaceServerIp + ":8080/video" + "?url=" + ConfigProfile.Current.CameraInIp.UrlEncode();
            var taskIn = WebSocket(url_in);
            await taskIn;
            webSocketIn = taskIn.Result;

            var url_out = "ws://" + ConfigProfile.Current.FaceServerIp + ":8080/video" + "?url=" + ConfigProfile.Current.CameraOutIp.UrlEncode();
            var taskOut = WebSocket(url_out);
            await taskOut;
            webSocketOut = taskOut.Result;

            RunServer();

            if (webSocketIn.IsConnected && webSocketOut.IsConnected)
                ReadyContent = WaitCard;
            else
                ReadyContent = "设备连接失败";
        }

        public void StartTimer()
        {
            _dispatchTimer = new DispatcherTimer();
            _dispatchTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatchTimer.Tick += (s, e) =>
            {
                ShowTimeInfo(DateTime.Now);
            };
            _dispatchTimer.Start();
        }

        private void ShowTimeInfo(DateTime dateTime)
        {
            var date = dateTime.ToShortDate();
            var week = (int)dateTime.DayOfWeek;
            var weekStr = Utility.GetWeekOfDay();
            var time = dateTime.ToShortTime();
            DateTimeInfo = string.Format("{0} {1} {2}", date, weekStr, time);
        }

        private void RunServer()
        {
            server.OnMessageInComming += Server_OnMessageInComming;
            server.Start();
        }

        #region 串口方式
        public void OnReadCard(byte[] cardBuffer, int cardNo)
        {
            var compare = doComapre(cardNo, "dyw-in", "进入");
            if (!compare)
            {
                //执行开闸
                output.Write(cardBuffer);
            }
        }
        #endregion

        public void Server_OnMessageInComming(object sender, MessageEventArgs e)
        {
            delayCall.Stop();
            var videoUrl = Channel.GetVideoUrl(e.Ip);
            var psssType = Channel.GetPassType(e.Ip);
            var alarm = doComapre(e.CardNo, videoUrl, psssType);
            if (!alarm)
            {
                Channel.Open(e.Ip, e.CardBytes);
            }
        }

        public Task<KoalaWebSocket> WebSocket(string url)
        {
            return Task.Factory.StartNew<KoalaWebSocket>(() =>
            {
                var socket = new KoalaWebSocket();
                var connect = socket.Connect(url, "");
                return socket;
            });
        }

        public bool doComapre(int cardNo, string videoUrl, string passType)
        {
            Employee employee = null;
            using (var db = new DoorDBContext())
            {
                employee = db.Find(cardNo.ToString());
            }
            if (employee == null)
            {
                ReadyContent = "未找到卡对应的人像信息->" + cardNo;
                LogHelper.Info(string.Format("error->未找到卡[{0}]对应的信息", cardNo));
                RestoreInit();
                return true;
            }

            Current.Name = employee.EmpName;
            Current.Card = employee.EmpCard;
            Current.Duty = employee.EmpDuty;
            Current.Phone = employee.EmpPhone;
            Current.Photo = employee.EmpPhoto;
            Current.Snap = "";
            ReadyContent = Comparing;
            ReadyVisibility = Visibility.Collapsed;
            CompareVisibility = Visibility.Visible;
            var alarm = true;

            FaceCompare data = FaceService.Compare(employee.EmpPhoto, videoUrl);
            if (data != null)
            {
                var snapPath = "";
                if (data.result != null && data.result.face != null)
                {
                    snapPath = FileManager.SaveFile(cardNo, data.result.face.image);
                    Current.Snap = snapPath;
                }

                if (data.score >= compare_ok || data.recognized)
                {
                    alarm = false;
                    Current.Score = Convert.ToInt32(data.score);
                    Current.Result = "相同";
                    if (passType == "进入")
                        DataStat.AllIn++;
                    else
                        DataStat.AllOut++;

                    LogHelper.Info(string.Format("[{0}]复核通过，抓图:{1}", employee.EmpCard, snapPath));
                }
                else
                {
                    alarm = true;
                    Current.Result = "不同";
                    DataStat.AllAlarm++;
                    LogHelper.Info(string.Format("[{0}]复核失败，抓图:{1}", employee.EmpCard, snapPath));
                }
            }
            else
            {
                Current.Result = "不同";
                DataStat.AllAlarm++;
                LogHelper.Info(string.Format("[{0}]复核失败，抓图:{1}", employee.EmpCard, "人脸检测为空"));
            }

            DataStat.All++;
            SaveHistory(Current, passType, alarm);
            RestoreInit();
            return alarm;
        }

        private void SaveHistory(Compare entity, string passType, bool alarm)
        {
            PassHistory pass = new PassHistory();
            pass.RecrodID = Utility.GUID();
            pass.Name = entity.Name;
            pass.Card = entity.Card;
            pass.Duty = entity.Duty;
            pass.Score = entity.Score;
            pass.Address = entity.Address;
            pass.Alaram = alarm;
            pass.Phone = entity.Phone;
            pass.PassTime = DateTime.Now;
            pass.PassType = passType;
            pass.Photo = entity.Photo;
            pass.Snap = entity.Snap;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (PassHistoryCollection.Count >= MAX_HISTORY_COUNT)
                    PassHistoryCollection.RemoveAt(PassHistoryCollection.Count - 1);
                PassHistoryCollection.Insert(0, entity.Copy());
            }));

            using (var db = new DoorDBContext())
            {
                db.PassHistorys.Add(pass);
                db.SaveChanges();
            }
        }

        private void RestoreInit()
        {
            delayCall.StartOnce(delayIntervel, () =>
            {
                Current.Name = string.Empty;
                Current.Card = string.Empty;
                Current.Phone = string.Empty;
                Current.Duty = string.Empty;
                Current.Photo = string.Empty;
                Current.Snap = string.Empty;
                Current.Score = 0;
                Current.Result = "";
                ReadyContent = WaitCard;
                CompareVisibility = Visibility.Collapsed;
                ReadyVisibility = Visibility.Visible;
            });
        }

        public void Dispose()
        {
            _dispatchTimer?.Stop();
            receiveSerial?.Close();
            outputSerial?.Close();
            webSocketIn?.Disconnect();
            webSocketOut?.Disconnect();
            server?.Stop();
        }
    }
}

