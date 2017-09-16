using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Core
{
    public class Rect
    {
        public int left { get; set; }
        public int top { get; set; }
        public int right { get; set; }
        public int bottom { get; set; }
    }

    public class FaceInfo
    {
        public Rect rect { get; set; }
        public double quality { get; set; }
        public double brightness { get; set; }
        public double std_deviation { get; set; }
    }

    public class Result
    {
        public int track { get; set; }
        public double quality { get; set; }
        public Rect rect { get; set; }
        public string image { get; set; }
    }

    public class ResultRoot
    {
        public double confidence { get; set; }
        public Result result { get; set; }
        public bool recognized { get; set; }
    }

    public class FaceCompare
    {
        public FaceInfo face_info { get; set; }
        public ResultRoot result { get; set; }
    }
}
