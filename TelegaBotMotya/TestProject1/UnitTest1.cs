using NUnit.Framework;
using TelegaBotMotya;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;
using System.Text.Json;

namespace TelegaBotMotya
{
    public class Tests
    {
        
        public static globals globals = new globals();
        public static List<user> users = new List<user>();
        public static List<ZakazOthet> zakazList = new List<ZakazOthet>();
        public static List<master> masters = new List<master>();
        [SetUp]
        public void Setup()
        {
            string fileName = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\zakazList.txt";
            string jsonString = System.IO.File.ReadAllText(fileName);
            zakazList = JsonSerializer.Deserialize<List<ZakazOthet>>(jsonString)!;

            string fileName1 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\users.txt";
            string jsonString1 = System.IO.File.ReadAllText(fileName1);
            users = JsonSerializer.Deserialize<List<user>>(jsonString1)!;

            string fileName2 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\masters.txt";
            string jsonString2 = System.IO.File.ReadAllText(fileName2);
            masters = JsonSerializer.Deserialize<List<master>>(jsonString2)!;
            string fileName3 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\masters.txt";
            string jsonString3 = System.IO.File.ReadAllText(fileName3);
            globals = JsonSerializer.Deserialize<List<globals>>(jsonString3)!;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
            int[] uc = Program.UC(1759163276, 1759163276);
            int USER = uc[0];
            int CHAT = uc[1];

        }
    }
}