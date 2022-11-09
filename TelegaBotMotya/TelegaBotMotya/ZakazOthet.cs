using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegaBotMotya
{
    public class ZakazOthet
    {
        public int id { get; set; }
        public Zakaz Zakaz {get; set; }
        public long master { get; set; } = -1;
        public DateTime MbTimeStart { get; set; }
        public DateTime MbTimeEnd { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public bool ended {get; set; }
        public bool DR {get; set; }
        public string otchet { get; set; } = "";
        public double sum {get; set; } 
        public long otpravitel { get; set; }
    }
}
