using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegaBotMotya
{
    public class master
    {
        public long id { get; set; }
        public string fio { get; set; }
        public string profil { get; set; }
        public int rayting { get; set; }
    
        public List<int> listZakaz { get; set; } = new List<int>();
        public List<int> mblistZakaz { get; set; } = new List<int>();
        //public List<Zakaz> next = new List<Zakaz>();

        
    }
    
}
