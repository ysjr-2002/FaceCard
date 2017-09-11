using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Core
{
    class FaceCompare
    {
        public bool recognized { get; set; }

        public result result { get; set; }

        public float score { get; set; }
    }

    class result
    {
        public long track { get; set; }

        public face face { get; set; }

        public float quality { get; set; }
    }

    class face
    {
        public Rect rect { get; set; }

        public string image { get; set; }
    }

    public class Rect
    {
        public int top { get; set; }

        public int right { get; set; }

        public int bottom { get; set; }

        public int left { get; set; }
    }
}
