using Common.Log;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DYW.ImageReview.Core
{
    class SerialDeviceID
    {
        private SerialPort _serial = null;
        private Thread _workThread = null;

        public delegate void ReadCardEventHanlder(byte[] cardArray, int cardNo);
        public event ReadCardEventHanlder OnReadCard;

        private bool open = false;

        public SerialDeviceID()
        {
        }

        public bool Open(string portName, int baud = 9600)
        {
            _serial = new SerialPort(portName, baud, Parity.None, 8, StopBits.One);
            try
            {
                _serial.Open();
                open = true;
            }
            catch (Exception ex)
            {
                LogHelper.Info(string.Format("串口打开失败->{0}", ex.Message));
            }
            return open;
        }

        public void Start()
        {
            _workThread = new Thread(Read);
            _workThread.Start();
        }

        public void Write(byte[] buffer)
        {
            if (_serial != null && _serial.IsOpen)
            {
                Array.Reverse(buffer);
                _serial.Write(buffer, 0, buffer.Length);
            }
        }

        private void Read()
        {
            while (open)
            {
                try
                {
                    var b1 = (byte)_serial.ReadByte();
                    var b2 = (byte)_serial.ReadByte();
                    var b3 = (byte)_serial.ReadByte();
                    var b4 = (byte)_serial.ReadByte();

                    var cardBuffer = new byte[] { b1, b2, b3, b4 };
                    Array.Reverse(cardBuffer);
                    var cardNo = BitConverter.ToInt32(cardBuffer, 0);
                    if (OnReadCard != null)
                    {
                        OnReadCard(cardBuffer, cardNo);
                    }
                }
                catch
                {
                }
            }
        }

        public void Close()
        {
            open = false;
            _serial?.Close();
            _serial = null;

            _workThread?.Abort();
            _workThread = null;
        }
    }
}
