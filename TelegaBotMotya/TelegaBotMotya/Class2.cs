using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegaBotMotya
{
    internal class Zakaz
    {
        public int id { get; set; }       
        public string adress  { get; set; }
        public string fio { get; set; }
        public string type { get; set; }
        public string phone { get; set; }
        public string hyinya { get; set; }
        public string Voice { get; set; }


    }
}
