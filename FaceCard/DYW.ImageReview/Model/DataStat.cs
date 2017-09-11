using Common;
using Common.NotifyBase;
using DYW.ImageReview.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DYW.ImageReview.Model
{
    class DataStat : PropertyNotifyObject
    {
        public DataStat()
        {
        }

        public int All
        {
            get { return this.GetValue(s => s.All); }
            set { this.SetValue(s => s.All, value); }
        }

        public int AllIn
        {
            get { return this.GetValue(s => s.AllIn); }
            set { this.SetValue(s => s.AllIn, value); }
        }

        public int AllOut
        {
            get { return this.GetValue(s => s.AllOut); }
            set { this.SetValue(s => s.AllOut, value); }
        }

        public int AllAlarm
        {
            get { return this.GetValue(s => s.AllAlarm); }
            set { this.SetValue(s => s.AllAlarm, value); }
        }

        public async void Init()
        {
            var t = Task.Factory.StartNew(() =>
            {
                using (var db = new DoorDBContext())
                {
                    var dayS = DateTime.Now.GetDayStart();
                    var dayE = DateTime.Now.GetDayEnd();
                    var list = db.PassHistorys.Where(s => s.PassTime >= dayS && s.PassTime <= dayE).ToList();

                    All = list.Count;
                    AllIn = list.Count(s => s.PassType == "进入" && s.Alaram == false);
                    AllOut = list.Count(s => s.PassType == "离开" && s.Alaram == false);
                    AllAlarm = list.Count(s => s.Alaram == true);
                }
            });
            await t;
        }
    }
}
