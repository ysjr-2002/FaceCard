using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DYW.ImageReview.Core
{
    /// <summary>
    /// 主板输出一个卡号
    /// </summary>
    class OutputController
    {
        private SerialDeviceID serial = null;

        public OutputController(SerialDeviceID serial)
        {
            this.serial = serial;
        }

        public void Write(byte[] data)
        {
            serial.Write(data);
        }
    }
}
