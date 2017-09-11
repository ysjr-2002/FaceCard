using DYW.ImageReview.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.NotifyBase;
using System.ComponentModel.DataAnnotations.Schema;

namespace DYW.ImageReview.Model
{
    class Compare : PropertyNotifyObject
    {
        public string Result
        {
            get { return this.GetValue(s => s.Result); }
            set { this.SetValue(s => s.Result, value); }
        }

        public string RecrodID
        {
            get { return this.GetValue(s => s.RecrodID); }
            set { this.SetValue(s => s.RecrodID, value); }
        }

        public string Name
        {
            get { return this.GetValue(s => s.Name); }
            set { this.SetValue(s => s.Name, value); }
        }

        public string Duty
        {
            get { return this.GetValue(s => s.Duty); }
            set { this.SetValue(s => s.Duty, value); }
        }

        public string Card
        {
            get { return this.GetValue(s => s.Card); }
            set { this.SetValue(s => s.Card, value); }
        }

        public string Photo
        {
            get { return this.GetValue(s => s.Photo); }
            set { this.SetValue(s => s.Photo, value); }
        }

        public string Snap
        {
            get { return this.GetValue(s => s.Snap); }
            set { this.SetValue(s => s.Snap, value); }
        }

        public float Score
        {
            get { return this.GetValue(s => s.Score); }
            set { this.SetValue(s => s.Score, value); }
        }

        public string Phone { get; set; }

        public string Address { get; set; }

        public bool? Alaram
        {
            get { return this.GetValue(s => s.Alaram); }
            set { this.SetValue(s => s.Alaram, value); }
        }

        public DateTime PassTime
        {
            get { return this.GetValue(s => s.PassTime); }
            set { this.SetValue(s => s.PassTime, value); }
        }

        public string PassType
        {
            get { return this.GetValue(s => s.PassType); }
            set { this.SetValue(s => s.PassType, value); }
        }

        public Compare Copy()
        {
            Compare copy = new Compare();
            copy.RecrodID = this.RecrodID;
            copy.Name = this.Name;
            copy.Card = this.Card;
            copy.Duty = this.Duty;
            copy.Photo = this.Photo;
            copy.Snap = this.Snap;
            copy.Phone = this.Phone;
            copy.Score = this.Score;
            copy.Address = this.Address;
            copy.Alaram = this.Alaram;
            copy.PassTime = this.PassTime;
            copy.PassType = this.PassType;
            return copy;
        }
    }
}
