using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Common.Log;
using Common;
using System.Collections;

namespace DYW.ImageReview.Core
{
    class SerialDevice
    {
        private SerialPort _serial = null;
        private Thread _workThread = null;

        public delegate void ReadCardEventHanlder(byte[] cardArray, int cardNo);
        public event ReadCardEventHanlder OnReadCard;

        private bool open = false;

        public SerialDevice()
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

        //351805 00 28 AB C7 A0
        //351899 00 28 AB D3 60
        private void Read()
        {
            List<byte> cardBytes = new List<byte>();
            while (open)
            {
                try
                {
                    var b1 = (byte)_serial.ReadByte();
                    var b2 = (byte)_serial.ReadByte();
                    var b3 = (byte)_serial.ReadByte();
                    var b4 = (byte)_serial.ReadByte();
                    var b5 = (byte)_serial.ReadByte();

                    var cardBuffer = new byte[] { b1, b2, b3, b4, b5 };

                    //BitArray array1 = new BitArray(new byte[] { b1 });
                    //BitArray array2 = new BitArray(new byte[] { b2 });
                    //BitArray array3 = new BitArray(new byte[] { b3 });
                    //BitArray array4 = new BitArray(new byte[] { b4 });
                    //BitArray array5 = new BitArray(new byte[] { b5 });

                    //array1 = CardConverter.BitArrayRevserve(array1);
                    //array2 = CardConverter.BitArrayRevserve(array2);
                    //array3 = CardConverter.BitArrayRevserve(array3);
                    //array4 = CardConverter.BitArrayRevserve(array4);
                    //array5 = CardConverter.BitArrayRevserve(array5);

                    //var nb1 = array1.BitArrayToByte();
                    //var nb2 = array2.BitArrayToByte();
                    //var nb3 = array3.BitArrayToByte();
                    //var nb4 = array4.BitArrayToByte();
                    //var nb5 = array5.BitArrayToByte();

                    //BitArray cardTotal = new BitArray(new byte[] { nb1, nb2, nb3, nb4, nb5 });

                    //BitArray card = new BitArray(20);
                    //var j = 0;
                    //for (var i = 15; i < cardTotal.Length; i++)
                    //{
                    //    card[j] = cardTotal[i];
                    //    Console.Write((card[j] ? "1" : "0"));
                    //    j++;

                    //    if (j == 20)
                    //        break;
                    //}

                    //card = CardConverter.BitArrayRevserve(card);
                    //var cardNo = card.BitArrayToInt();

                    var cardNo = CardConverter.GetCardNo(cardBuffer);
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


        public void Write(byte[] data)
        {
            if (_serial != null && _serial.IsOpen)
            {
                _serial.Write(data, 0, data.Length);
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
