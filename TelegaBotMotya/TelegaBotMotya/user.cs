using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegaBotMotya
{
    internal class user
    {
        public long Id { get; set; }

        public List<Chat> chats { get; set; } = new List<Chat>();

        public List<int> ChatsAccess { get; set; } = new List<int>();

        public List<string> ChatStep { get; set; } = new List<string>();

        public string lastMessage { get; set; } = "";
        public int Zakaz { get; set; } = 0;
        public long Master { get; set; } = 0;
        public List<int> menu { get; set; } = new List<int>();
        public int access { get; set; } = -1;

    }
}
