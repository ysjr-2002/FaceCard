using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYW.ImageReview.Entitys
{
    class PageInfo
    {
        public PageInfo()
        {
            PageIndex = 1;
            PageSize = 30;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int RecordTotal { get; set; }

        public int PageTotal
        {
            get
            {
                if (RecordTotal == 0)
                    return 1;
                else
                {
                    var p = RecordTotal / PageSize;
                    var m = RecordTotal % PageSize;
                    if (m != 0)
                        return (p + 1);
                    else
                        return p;
                }
            }
        }
    }
}
