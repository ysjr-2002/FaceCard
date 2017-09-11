using Common.NotifyBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DYW.ImageReview.Core
{
    [Table("employee")]
    public class Employee : PropertyNotifyObject
    {
        [Key]
        public string EmpID
        {
            get { return this.GetValue(s => s.EmpID); }
            set { this.SetValue(s => s.EmpID, value); }
        }

        public string EmpName
        {
            get { return this.GetValue(s => s.EmpName); }
            set { this.SetValue(s => s.EmpName, value); }
        }

        public string EmpDuty
        {
            get { return this.GetValue(s => s.EmpDuty); }
            set { this.SetValue(s => s.EmpDuty, value); }
        }

        public string EmpPhone
        {
            get { return this.GetValue(s => s.EmpPhone); }
            set { this.SetValue(s => s.EmpPhone, value); }
        }

        public string EmpAddress
        {
            get { return this.GetValue(s => s.EmpAddress); }
            set { this.SetValue(s => s.EmpAddress, value); }
        }

        public string EmpCard
        {
            get { return this.GetValue(s => s.EmpCard); }
            set { this.SetValue(s => s.EmpCard, value); }
        }

        public string EmpPhoto
        {
            get { return this.GetValue(s => s.EmpPhoto); }
            set { this.SetValue(s => s.EmpPhoto, value); }
        }

        //public string EmpSnap
        //{
        //    get { return this.GetValue(s => s.EmpSnap); }
        //    set { this.SetValue(s => s.EmpSnap, value); }
        //}

        public int FaceScore
        {
            get { return this.GetValue(s => s.FaceScore); }
            set { this.SetValue(s => s.FaceScore, value); }
        }

        //public string Result
        //{
        //    get { return this.GetValue(s => s.Result); }
        //    set { this.SetValue(s => s.Result, value); }
        //}

        public DateTime CreateTime
        {
            get { return this.GetValue(s => s.CreateTime); }
            set { this.SetValue(s => s.CreateTime, value); }
        }

        public string EmpRemark
        {
            get { return this.GetValue(s => s.EmpRemark); }
            set { this.SetValue(s => s.EmpRemark, value); }
        }

        public string EmpRemark1
        {
            get { return this.GetValue(s => s.EmpRemark1); }
            set { this.SetValue(s => s.EmpRemark1, value); }
        }

        public string EmpRemark2
        {
            get { return this.GetValue(s => s.EmpRemark2); }
            set { this.SetValue(s => s.EmpRemark2, value); }
        }

        public string EmpRemark3
        {
            get { return this.GetValue(s => s.EmpRemark3); }
            set { this.SetValue(s => s.EmpRemark3, value); }
        }
    }
}
