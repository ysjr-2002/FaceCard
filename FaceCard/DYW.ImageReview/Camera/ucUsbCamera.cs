using AForge.Video.DirectShow;
using Common.Dialog;
using Common.Log;
using Common.WebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DYW.ImageReview.Core;

namespace DYW.ImageReview.Camera
{
    /// <summary>
    /// USB摄像头
    /// </summary>
    public partial class ucUsbCamera : UserControl
    {
        private VideoCaptureDevice currentDevice = null;
        public ucUsbCamera()
        {
            InitializeComponent();
            this.Load += UcUsbCamera_Load;
        }

        private void UcUsbCamera_Load(object sender, EventArgs e)
        {
        }

        public bool IsConnected { get; set; }

        public bool Connect()
        {
            IsConnected = false;
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                return false;
            }
            // create video source
            currentDevice = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSourcePlayer1.VideoSource = currentDevice;
            var videoCapabilities = currentDevice.VideoCapabilities;
            //foreach (var video in videoCapabilities)
            //{
            //    LogHelper.Info("预览分辨率->" + video.FrameSize.Width + "*" + video.FrameSize.Height);
            //}
            if (videoCapabilities.Count() > 0)
                currentDevice.VideoResolution = currentDevice.VideoCapabilities.Last();

            var snapVabalities = currentDevice.SnapshotCapabilities;
            //foreach (var snap in snapVabalities)
            //{
            //    LogHelper.Info("抓拍分辨率->" + snap.FrameSize.Width + "*" + snap.FrameSize.Height);
            //}
            if (snapVabalities.Count() > 0)
                currentDevice.SnapshotResolution = currentDevice.SnapshotCapabilities.Last();

            currentDevice.Start();
            IsConnected = true;
            return true;
        }

        /// <summary>
        /// 抓拍人脸图片
        /// </summary>
        /// <returns></returns>
        public string Snap()
        {
            var filepath = Path.Combine(FileManager.GetFolder(), DateTime.Now.Ticks + ".jpg");
            var bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
            bitmap.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return filepath;
        }

        public void StopCamera()
        {
            if (currentDevice != null)
            {
                currentDevice.SignalToStop();
            }
        }
    }
}
