using Common.NotifyBase;
using DYW.ImageReview.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Entitys
{
    class DoorDBContext : DbContext
    {
        public DbSet<PassHistory> PassHistorys { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DoorDBContext() : base("SqliteTest")
        {
        }

        public Employee Find(string card)
        {
            var emp = Employees.FirstOrDefault(s => s.EmpCard == card);
            return emp;
        }

       
    }

    [Table("PassHistory")]
    class PassHistory : PropertyNotifyObject
    {
        [Key]
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
    }
}
