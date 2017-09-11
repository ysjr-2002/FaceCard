using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Core
{
    public class Face
    {
        public string image { get; set; }

        public Rect rect { get; set; }
    }

    public class Person
    {
        public int feature_Id { get; set; }

        public double confidence { get; set; }

        public string tag { get; set; }

        public string id { get; set; }
    }

    public class Data
    {
        public string status { get; set; }

        public attr attr { get; set; }

        public long track { get; set; }

        public long timestamp { get; set; }

        public Face face { get; set; }

        public Person person { get; set; }

        public double quality { get; set; }
    }

    public class attr
    {
        public double age { get; set; }

        public double male { get; set; }

        public double female { get; set; }
    }

    public class Screen
    {
        public string camera_address { get; set; }

        public object[] allowed_subject_ids { get; set; }

        public string network_switcher_status { get; set; }

        public string box_token { get; set; }

        public string description { get; set; }

        public long box_heartbeat { get; set; }

        public string network_switcher { get; set; }

        public string camera_name { get; set; }

        public string camera_status { get; set; }

        public bool allow_visitor { get; set; }

        public string screen_token { get; set; }

        public object network_switcher_token { get; set; }

        public string box_status { get; set; }

        public bool allow_all_subjects { get; set; }

        public int type { get; set; }

        public int id { get; set; }

        public string camera_position { get; set; }

        public string box_address { get; set; }
    }

    public class Employeekoala
    {
        public string src { get; set; }

        public string remark { get; set; }

        public int subject_type { get; set; }

        public string description { get; set; }

        public string title { get; set; }

        public long timestamp { get; set; }

        public int start_time { get; set; }

        public string avatar { get; set; }

        public string job_number { get; set; }

        public object birthday { get; set; }

        public object entry_date { get; set; }

        public string department { get; set; }

        public int end_time { get; set; }

        public int id { get; set; }

        public string name { get; set; }
    }

    public class RecognizedResult
    {
        public Employeekoala person { get; set; }

        public bool open_door { get; set; }

        public string type { get; set; }

        public Data data { get; set; }

        public string error { get; set; }
    }

    public class CheckResult
    {
        public class checkdata
        {
            public int person_id { get; set; }
        }

        public int code { get; set; }

        public checkdata data { get; set; }
    }
}
