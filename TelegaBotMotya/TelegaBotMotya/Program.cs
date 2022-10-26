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



    class Program
    {

        public static globals globals=new globals();
        public static List<user> users = new List<user>();
        public static List<Client> clients = new List<Client>();
        public static List<ZakazOthet> zakazList = new List<ZakazOthet>();
        public static List<master> masters = new List<master>();

        static ITelegramBotClient bot = new TelegramBotClient("5793343259:AAHn_52qS37gw79qOcUGfqJP2n-Oe9kn8QA");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
            

            Console.WriteLine();
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                
                //await HandleMessage(botClient, update.Message);
                var message = update.Message;


                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(message));           
                int[] uc = UC(update.Message.From.Id, update.Message.Chat);
                int USER = uc[0];
                int CHAT = uc[1];
                Console.WriteLine(update.Message.From.Id);
                Console.WriteLine(USER + " " +CHAT);
                await saveusers();
                switch (users[USER].access)
                {
                    case 1:
                        {
                            await menu1(message, USER, CHAT, botClient);
                            //await acse1(message, USER, CHAT, botClient);
                            return;
                        }
                    case 2:
                        {
                            await menu2(message, USER, CHAT, botClient);
                            return;
                        }
                    default:
                        {
                            await acse_1(message, USER, CHAT, botClient);
                            return;
                        }
                }      
            }
            await saveusers();
        }

        public static async Task acse1(Message message, int USER, int CHAT, ITelegramBotClient botClient)
        {
            switch (users[USER].ChatStep[CHAT])
            {
                case "0":
                    {
                        switch (message.Text)
                        {

                            case "Подсказка":
                                {
                                    


                                    {


                                        keybord(0, message, botClient, USER);


                                    }
                                    //await botClient.SendTextMessageAsync(message.Chat, );

                                    return;

                                }

                            case "Все заказы":
                                {
                                    keybord(5, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "6";
                                    
                                    return;
                                }
                            case "Незаконченные":
                                {
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        //zakazList[i].master = -1;
                                        //Console.WriteLine(zakazList[i].MbTimeStart.DayOfYear + " " + DateTime.Now.DayOfYear);
                                        //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice);
                                        //Console.WriteLine(DateTime.Now + " "+ zakazList[i].MbTimeStart.AddHours(3) + " " + zakazList[i].master +" "+ zakazList[i].otchet);
                                        if (DateTime.Now>=zakazList[i].MbTimeStart.AddHours(3) & zakazList[i].master != -1 & zakazList[i].otchet == "")
                                        {


                                            InlineKeyboardMarkup keyboard = new(new[]
                                                    {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+zakazList[i].id)
                                                    },
                                                });
                                            await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya, replyMarkup: keyboard);

                                        }
                                        
                                    }
                                    keybord(0, message, botClient, USER);

                                    return;
                                }
                            case "Законченные":
                                {
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {
                                        //zakazList[i].otchet = "";
                                        //zakazList[i].master = -1;
                                        //Console.WriteLine(zakazList[i].MbTimeStart.DayOfYear + " " + DateTime.Now.DayOfYear);
                                        //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice);
                                        //Console.WriteLine(zakazList[i].MbTimeStart  +" "+ zakazList[i].MbTimeStart.AddHours(3) + " " + zakazList[i].master +" "+ zakazList[i].otchet);
                                        if (DateTime.Now >= zakazList[i].MbTimeStart.AddHours(3) & zakazList[i].master != -1 & zakazList[i].otchet != "")
                                        {


                                            InlineKeyboardMarkup keyboard = new(new[]
                                                    {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+zakazList[i].id)
                                                    },
                                                });
                                            await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya, replyMarkup: keyboard);

                                        }

                                    }
                                    keybord(0, message, botClient, USER);
                                    return;

                                    return;
                                }

                            case "Все мастера":
                                {
                                    for (int i = 0; i < masters.Count; i++)
                                    {

                                        await botClient.SendTextMessageAsync(message.Chat, "Мастер № " + masters[i].id + "\n ФИО " + masters[i].fio + "\n рэйтинг " + masters[i].rayting + "\n профиль " + masters[i].profil);

                                        //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice);
                                        //await botClient.SendTextMessageAsync(message.Chat, "Ваш заказ №" + i + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya);
                                    }
                                    return;
                                }
                            case "Выбрать заказ":
                                {
                                    if (zakazList.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказы отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите ID заказа");
                                        users[USER].ChatStep[CHAT] = "1";
                                    }

                                    return;
                                }
                            case "Выбрать мастера":
                                {
                                    if (masters.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Мастера отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите ID мастера");
                                        users[USER].ChatStep[CHAT] = "3";
                                    }

                                    return;
                                }
                            case "Удалить заказ":
                                {
                                    if (zakazList.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказы отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Для удаления введите  номер заказа до " + (zakazList.Count - 1) + "\n или -1 для выхода");
                                        users[USER].ChatStep[CHAT] = "2";
                                    }

                                    return;
                                }


                            case "/kill":
                                {
                                    Environment.Exit(0);
                                    return;
                                }

                            default:
                                {
                                    // if (message )
                                    shablon(message,USER,CHAT,botClient);
                                    break;

                                }
                        }
                        return;
                    }



                case "1":
                    {

                        if (message.Text=="Назад")
                        {
                            users[USER].ChatStep[CHAT] = "0";
                            keybord(0, message, botClient, USER);
                            return;
                        }    
                        int numericValue;
                        int i;
                        if (int.TryParse(message.Text, out numericValue))
                        {
                            int zm= IDtoNUM_Z(numericValue);
                            if (zm != -1)
                            {


                                //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice);
                                //await botClient.SendTextMessageAsync(message.Chat, "Ваш заказ №" + i + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya);

                                

                                //await botClient.SendTextMessageAsync(message.Chat, " Удалить заказ \n Поручить заказ \n Назад ");
                                users[USER].lastMessage = zm.ToString();
                                users[USER].ChatStep[CHAT] = "5";
                                keybord(1, message, botClient, USER);
                            }
                            else
                            {
                                keybord(-1, message, botClient, USER);
                                //await botClient.SendTextMessageAsync(message.Chat, "Вы ошиблись при вводе 1");
                            }
                        }
                        else
                        {
                            keybord(-1, message, botClient, USER);
                            //await botClient.SendTextMessageAsync(message.Chat, "Вы ошиблись при вводе 2     ");
                            users[USER].ChatStep[CHAT] = "1";
                        }

                        {


                           


                        }

                        return;

                    }
                case "5":
                    {
                        users[USER].ChatStep[CHAT] = message.Text;
                       // users[USER].ChatStep[CHAT] = message.Text;
                        switch (message.Text)
                        {
                            case "Поручить заказ":
                                {
                                    if (zakazList[Convert.ToInt32(users[USER].lastMessage)].master==-1)
                                    await botClient.SendTextMessageAsync(message.Chat, "Введите № мастера");
                                    else
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите № мастера если хотите заменить текущего");
                                    return;

                                }
                            case "Удалить заказ":
                                {
                                    zakazList.RemoveAt(Convert.ToInt32(users[USER].lastMessage));
                                    await savezakazlist();
                                    await botClient.SendTextMessageAsync(message.Chat, "Заказ № " + Convert.ToInt32(users[USER].lastMessage) + " удалён");
                                    
                                    return;
                                }
                            case "Назад":
                                {
                                    keybord(0, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";


                                    return;

                                }
                            case "Снять мастера":
                                {
                                    int i = Convert.ToInt32(users[USER].lastMessage);
                                    Console.WriteLine("gfdshsdh "+i);
                                    zakazList[i].master = -1;
                                    int MASTER = -1;
                                    for (int iJ = 0; iJ < masters.Count; iJ++)
                                    {

                                            for (int iJ1 = 0; iJ1 < masters[iJ].listZakaz.Count; iJ1++)
                                            {
                                                if (masters[iJ].listZakaz[iJ1] == zakazList[i].id)
                                                {
                                                    masters[iJ].listZakaz.RemoveAt(iJ1);
                                                }
                                            }
                                        for (int iJ1 = 0; iJ1 < masters[iJ].mblistZakaz.Count; iJ1++)
                                        {
                                            if (masters[iJ].mblistZakaz[iJ1] == zakazList[i].id)
                                            {
                                                masters[iJ].mblistZakaz.RemoveAt(iJ1);
                                            }
                                        }

                                    }
                                    
                                    keybord(1, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "5";
                                    return;

                                }
                        }
                        return;
                    }
                case "День":
                    {
                        users[USER].ChatStep[CHAT] = "6";return;
                    }
                case "6":
                    {
                        users[USER].ChatStep[CHAT] = message.Text;
                        
                        switch (message.Text)
                        {
                            case "День":
                                {

                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 1, message, botClient);


                                    }
                                    users[USER].ChatStep[CHAT] = "6";
                                    return;

                                }
                            case "Неделя":
                                {     
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 7, message, botClient);

                                    }
                                    users[USER].ChatStep[CHAT] = "6";
                                    return;

                                }
                            case "Месяц":
                                {

                                    
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 30, message, botClient);

                                    }
                                    users[USER].ChatStep[CHAT] = "6";
                                    return;

                                }
                            case "3 Месяца":
                                {

                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 90, message, botClient);

                                    }
                                    users[USER].ChatStep[CHAT] = "6";
                                    return;

                                }
                            case "Пол года":
                                {

                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 180, message, botClient);

                                    }
                                    users[USER].ChatStep[CHAT] = "6";
                                    return;

                                }
                            case "Назад":
                                {
                                    keybord(0, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";


                                    return;

                                }
                            
                        }
                        return;
                    }
                case "Поручить заказ":
                    {
                        int numericValue;
                        //long i = -1;
                        if (message.Text == "Назад")
                        {
                            users[USER].ChatStep[CHAT] = "5";
                            keybord(0, message, botClient, USER);
                            return;
                        }
                        int zm = -1;
                        long i= Convert.ToInt64(message.Text); ;
                        for (int j = 0; j < users.Count; j++)
                        {
                            if (users[j].Id == i)
                            {
                                zm = j;
                            }
                        }
                        //zm;
                        //if (int.TryParse(message.Text, out numericValue))
                        {
                            //long i = 
                            //int zm = IDtoNUM_Z(i);
                            Console.WriteLine(" " + zm+ " " + i);
                            if (zm!=-1)
                            {
                                if (zakazList[Convert.ToInt32(users[USER].lastMessage)].master != -1)
                                {
                                    for (int i1 = 0; i1 < masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].listZakaz.Count; i1++)
                                    {
                                        if (masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].listZakaz[i1]== zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                        {
                                            masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].listZakaz.RemoveAt(i1);
                                        }
                                    }
                                    for (int i1 = 0; i1 < masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].mblistZakaz.Count; i1++)
                                    {
                                        if (masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].mblistZakaz[i1] == zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                        {
                                            masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].mblistZakaz.RemoveAt(i1);
                                        }
                                    }
                                }
                                    masters[IDtoNUM_M(i)].mblistZakaz.Add(zakazList[Convert.ToInt32(users[USER].lastMessage)].id);
                                zakazList[Convert.ToInt32(users[USER].lastMessage)].otpravitel = users[USER].Id;
                                zakazList[Convert.ToInt32(users[USER].lastMessage)].master = i;

                                await savemasters();

                                await savezakazlist();

                                
                                //Console.WriteLine(users[masters[i].id].Id + " " + masters[i].id + " "+i);
                                await botClient.SendTextMessageAsync(message.Chat, "Порученно мастеру № " + i);
                                await botClient.SendTextMessageAsync(users[zm].chats[0], "Вам пришёл заказ");
                                keybord(1, message, botClient, USER);

                                users[USER].ChatStep[CHAT] = "5";
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(message.Chat, "Неверный номер мастера  " + (masters.Count - 1));
                            }
                        }

                        return;
                    }
                




                case "2":
                    {
                        int i = Convert.ToInt32(message.Text);
                        if (i < zakazList.Count & i >= 0)
                        {
                            zakazList.RemoveAt(i);
                            await savezakazlist();
                            users[USER].ChatStep[CHAT] = "0";
                            await botClient.SendTextMessageAsync(message.Chat, "Заказ удалён осталось " + (zakazList.Count - 1) + " заказов");
                        }
                        else
                        {
                            if (i == -1)
                            {
                                users[USER].ChatStep[CHAT] = "0";
                                await botClient.SendTextMessageAsync(message.Chat, "Возврат к прошлому пункту меню");
                            }
                            else
                                await botClient.SendTextMessageAsync(message.Chat, "Неверный номер заказа, максимальный номер заказа " + (zakazList.Count - 1));

                        }

                        return;

                    }
                case "3":
                    {
                        int numericValue;
                        int i;
                        if (message.Text == "Назад")
                        {
                            users[USER].ChatStep[CHAT] = "0";
                            keybord(0, message, botClient, USER);
                            return;
                        }
                        if (int.TryParse(message.Text, out numericValue))
                        {
                            i = Convert.ToInt32(message.Text);
                            int zm = IDtoNUM_M(i);
                            if (zm!=-1)
                            {

                            

                                
                                //await botClient.SendTextMessageAsync(message.Chat, "Мастер № " + i + "\n ФИО " + masters[i].fio + "\n рэйтинг " + masters[i].rayting + "\n профиль " + masters[i].profil);

                                //await botClient.SendTextMessageAsync(message.Chat, " /delete \n Переименовать \n Изменить рейтинг \n Изменить профиль \n Назад ");
                                users[USER].lastMessage = zm.ToString();
                                users[USER].ChatStep[CHAT] = "4";
                                keybord(2, message, botClient, USER);

                            }
                            else
                            {
                                keybord(-1, message, botClient, USER);
                            }
                        }
                        else
                        {
                            keybord(-1, message, botClient, USER);
                            users[USER].ChatStep[CHAT] = "3";
                        }

                        return;

                    }
                case "4":
                    {
                        users[USER].ChatStep[CHAT] = message.Text;
                        switch (message.Text)
                        {
                            case "Переименовать":
                                {
                                    ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                    {
                                        ResizeKeyboard = true
                                    };


                                    await botClient.SendTextMessageAsync(message.Chat, "Введите ФИО мастера", replyMarkup: keyboard);
                                    //await botClient.SendTextMessageAsync(message.Chat, "Введите ФИО мастера");

                                    return;

                                }
                            case "Изменить рейтинг":
                                {
                                    ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                    {
                                        ResizeKeyboard = true
                                    };


                                    await botClient.SendTextMessageAsync(message.Chat, "Введите рэйтинг мастера", replyMarkup: keyboard);
                                    //await botClient.SendTextMessageAsync(message.Chat, "Введите рэйтинг мастера");
                                    return;

                                }
                            case "Изменить профиль":
                                {
                                    ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                    {
                                        ResizeKeyboard = true
                                    };


                                    await botClient.SendTextMessageAsync(message.Chat, "Введите профиль мастера", replyMarkup: keyboard);

                                    //await botClient.SendTextMessageAsync(message.Chat, "Введите профиль мастера");
                                    return;

                                }
                            case "Назад":
                                {
                                    keybord(0, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";


                                    return;

                                }
                        }
                        return;

                    }
                
                
                default:
                    {
                        keybord(0, message, botClient, USER);
                        users[USER].ChatStep[CHAT] = "0";
                        
                        return;
                    }



            }
        }
        public static async Task acse2(Message message,int USER, int CHAT, ITelegramBotClient botClient)
        {
            Console.WriteLine(USER+" "+CHAT);
            Console.WriteLine(users[USER].ChatStep[CHAT]);
            //Console.WriteLine(message.Text);
            //Console.WriteLine("AAA1");
            //users[USER].ChatStep[CHAT] = "0";
            //users[USER].ChatStep[CHAT] = "5";
            int MASTER = -1;
            for (int i = 0; i < masters.Count; i++)
            {
                if (masters[i].id == users[USER].Id)
                {
                    MASTER = i;
                }
            }
            switch (users[USER].ChatStep[CHAT])
            {
                case "0":
                    {
                        
                        switch (message.Text)
                        {
                            case "Подсказка":
                                {
                                    //await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, slaves! \n Список доступных комманд \n Подсказка \n /AllmyZakazs \n Все заказы \n /notends \n Выбрать заказ");
                                    keybord(3, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";
                                    return;

                                }
                            case "Назад":
                                {
                                    //await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, slaves! \n Список доступных комманд \n Подсказка \n /AllmyZakazs \n Все заказы \n /notends \n Выбрать заказ");
                                    keybord(3, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";
                                    return;

                                }
                            
                            case "Все мои заказы":
                                {
                                    keybord(5, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "6";
                                    


                                    return;

                                }
                            case "Не завершённые":
                                {
                                    if (masters[MASTER].listZakaz.Count > 0)
                                    {
                                        for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                        {
                                            
                                            int zm = IDtoNUM_Z(masters[MASTER].listZakaz[i]);
                                            Console.WriteLine(i + " " + zm + " " + zakazList[zm].ended);
                                            if (!zakazList[zm].ended)
                                            {
                                                InlineKeyboardMarkup keyboard = new(new[]
                                                    {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+masters[MASTER].listZakaz[i])
                                                    },
                                                });
                                                await botClient.SendAudioAsync(message.Chat, zakazList[zm].Zakaz.Voice, "Ваш заказ №" + zakazList[zm].id + "\n Время " + zakazList[zm].TimeStart + "\n Адресс " + zakazList[zm].Zakaz.adress + "\n телефон " + zakazList[zm].Zakaz.phone + "\n ФИО " + zakazList[zm].Zakaz.fio + "\n ТИП " + zakazList[zm].Zakaz.type + "\n Другое " + zakazList[zm].Zakaz.hyinya+" \n ended" +zakazList[zm].ended, replyMarkup: keyboard);
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(message.Chat, "У вас нет незавершённых заказов");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "У вас нет заказов");
                                    }


                                    return;

                                }
                            case "Входящие":
                                {
                                    //Console.WriteLine(masters[MASTER].id+" "+ masters[MASTER].mblistZakaz.Count);
                                    if (masters[MASTER].mblistZakaz.Count > 0)
                                    {
                                        for (int i = 0; i < masters[MASTER].mblistZakaz.Count; i++)
                                        {
                                            int zm = IDtoNUM_Z(masters[MASTER].mblistZakaz[i]);
                                            InlineKeyboardMarkup keyboard = new(new[]
                                                {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+masters[MASTER].mblistZakaz[i])
                                                    },
                                                });
                                            await botClient.SendAudioAsync(message.Chat, zakazList[zm].Zakaz.Voice, "Ваш заказ №" + zakazList[zm].id + "\n Время " + zakazList[zm].TimeStart + "\n Адресс " + zakazList[zm].Zakaz.adress + "\n телефон " + zakazList[zm].Zakaz.phone + "\n ФИО " + zakazList[zm].Zakaz.fio + "\n ТИП " + zakazList[zm].Zakaz.type + "\n Другое " + zakazList[zm].Zakaz.hyinya, replyMarkup: keyboard);
                                        }
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "У вас нет заказов");
                                    }


                                    return;

                                }
                            case "/notends":
                                {
                                    users[USER].access = 1;

                                    await botClient.SendTextMessageAsync(message.Chat, "Вы мотя");
                                    return;

                                }
                            case "Выбрать заказ":
                                {
                                    if (masters[MASTER].listZakaz.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказы отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите номер заказа до " + (masters[MASTER].listZakaz.Count - 1));
                                        users[USER].ChatStep[CHAT] = "1";
                                    }
                                    return;
                                }
                        }
                        return;
                    }
                case "5":
                    {
                        //Console.WriteLine("AAA2");
                        //new KeyboardButton[] { "Принять заказ", "Отказаться от заказа", "Назад" },
                        //    new KeyboardButton[] { "ДР", "Перенос даты ремонта", "Отчёт" }
                        
                            // users[USER].ChatStep[CHAT] = message.Text;
                            //Console.WriteLine(message.Text);
                            switch (message.Text)
                        {
                            case "Принять заказ":
                                {
                                    //Console.WriteLine(zakazList[Convert.ToInt32(users[USER].lastMessage)].master+ " "+ message.From.Id);
                                    int zzz = 0;
                                    users[USER].ChatStep[CHAT] = message.Text;
                                    Console.WriteLine(masters[MASTER].mblistZakaz.Count + "  "+ masters[MASTER].listZakaz.Count);
                                    for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                    {

                                                //Console.WriteLine(zakazList[Convert.ToInt32(users[USER].lastMessage)].id + "   "+ masters[MASTER].listZakaz[i1]);
                                                if (zakazList[Convert.ToInt32(users[USER].lastMessage)].id == masters[MASTER].listZakaz[i])
                                                {
                                                    //Console.WriteLine("FFFFFFFFFFF");
                                                    zzz = 1;
                                                }
                                            
                                        
                                    }
                                    Console.WriteLine(zzz + " KKKKK");
                                    if (zzz!=1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказ принят");

                                        int xxx = 0;
                                        for (int i = 0; i < users.Count; i++)
                                        {
                                            if (users[i].Id == zakazList[Convert.ToInt32(users[USER].lastMessage)].otpravitel)
                                            {
                                               xxx = i;
                                            }
                                        }

                                        await botClient.SendTextMessageAsync(users[xxx].chats[0], "От мастера " + masters[MASTER].fio + "\nЗаказа № " + zakazList[Convert.ToInt32(users[USER].lastMessage)].id+" принят");

                                            for (int i = 0; i < masters[MASTER].mblistZakaz.Count; i++)
                                    {
                                        //Console.WriteLine(IDtoNUM_Z(masters[MASTER].mblistZakaz[i]) + " " + Convert.ToInt32(users[USER].lastMessage));
                                        if (masters[MASTER].mblistZakaz[i] == zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                        {
                                            masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                            masters[MASTER].mblistZakaz.RemoveAt(i);
                                        }
                                    }
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказ уже принят");
                                    }
                                    users[USER].ChatStep[CHAT] = "5";

                                    await savemasters();
                                    await savezakazlist();
                                    return;

                                }
                            case "Отказаться от заказа":
                                {
                                    //zakazList.RemoveAt(Convert.ToInt32(users[USER].lastMessage));
                                    users[USER].ChatStep[CHAT] = "Причина отказа";
                                    ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                    {
                                        ResizeKeyboard = true
                                    };


                                    await botClient.SendTextMessageAsync(message.Chat, "Заказу № " + zakazList[Convert.ToInt32(users[USER].lastMessage)].id + " отказан. \nОпишите причину", replyMarkup: keyboard);

                                    await savemasters();
                                    await savezakazlist();

                                    return;
                                }
                            case "Назад":
                                {
                                    //Console.WriteLine("AAA3");
                                    keybord(3, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";
                                    //Console.WriteLine("aaaaaaaaaaaa");

                                    return;

                                }
                            case "ДР":
                                {
                                    int i = Convert.ToInt32(users[USER].lastMessage);
                                    await botClient.SendTextMessageAsync(message.Chat, "Отмеченно ка ДР");
                                    //zakazList[i].master = -1;

                                    zakazList[Convert.ToInt32(users[USER].lastMessage)].DR = true;
                                    keybord(4, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "5";

                                    await savemasters();
                                    await savezakazlist();
                                    return;

                                }
                            case "Перенос даты ремонта":
                                {
                                    await botClient.SendTextMessageAsync(message.Chat, "Введите двае даты в формате день месяц год пример:\n20 02 2020 14 50 - 21 02 2020 16 20");
                                    users[USER].ChatStep[CHAT] = "Перенос даты ремонта";

                                    await savemasters();
                                    await savezakazlist();
                                    return;

                                }
                            case "Отчёт":
                                {
                                    await botClient.SendTextMessageAsync(message.Chat, "Опишите заказ в отчёте");
                                    users[USER].ChatStep[CHAT] = "Отчёт";

                                    await savemasters();
                                    await savezakazlist();
                                    return;

                                }
                            default:
                                {
                                    await botClient.SendTextMessageAsync(message.Chat, "ошибка");
                                    users[USER].ChatStep[CHAT] = "5";
                                    
                                    return;
                                }
                        }
                        return;
                    }

                case "6":
                    {
                        if(message.Text== "Назад")
                    {
                        keybord(3, message, botClient, USER);
                        users[USER].ChatStep[CHAT] = "0";


                        return;

                    }
                    if (masters[MASTER].listZakaz.Count > 0)
                        {
                            
                        
                        //users[USER].ChatStep[CHAT] = message.Text;

                        // users[USER].ChatStep[CHAT] = message.Text;
                        switch (message.Text)
                        {
                            case "День":
                                {

                                       
                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 1, message, botClient);
                                        }
                                        users[USER].ChatStep[CHAT] = "6";
                                        return;

                                }
                            case "Неделя":
                                    {

                                       
                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 7, message, botClient);
                                        }
                                        users[USER].ChatStep[CHAT] = "6";
                                        return;

                                    }
                                case "Месяц":
                                    {

                                        
                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);                                           
                                            await ZakazForDateBefore(i, 30, message, botClient);
                                        }
                                        users[USER].ChatStep[CHAT] = "6";
                                        return;

                                    }
                                case "3 Месяца":
                                    {

                                       
                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);                                         
                                            await ZakazForDateBefore(i, 90, message, botClient);
                                        }
                                        users[USER].ChatStep[CHAT] = "6";
                                        return;

                                    }
                                case "Пол года":
                                    {


                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 180, message, botClient);
                                        }
                                        users[USER].ChatStep[CHAT] = "6";
                                        return;

                                    }
                                case "Назад":
                                {
                                    keybord(3, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "0";


                                    return;

                                }
                                    
                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "У вас нет заказов");
                        }
                        return;
                        
                    }
                case "Причина отказа":
                    {
                        if (message.Text=="Назад")
                        {
                            users[USER].ChatStep[CHAT] = "5";
                            keybord(4, message, botClient, USER);
                            return;
                        }    
                        for(int ji=0;ji<users.Count;ji++)
                        {
                            if (users[ji].Id == zakazList[Convert.ToInt32(users[USER].lastMessage)].otpravitel)
                            {
                                await botClient.SendTextMessageAsync(users[ji].chats[0],"От мастера "+masters[MASTER].fio +"\nОтказ от заказа № "+ zakazList[Convert.ToInt32(users[USER].lastMessage)].id+" комментарий:\n"+  message.Text);
                                //await botClient.SendTextMessageAsync(message.Chat, "Заказ принят");
                                for (int i = 0; i < masters[MASTER].mblistZakaz.Count; i++)
                                {
                                    //Console.WriteLine(masters[MASTER].mblistZakaz[i].id + " " + Convert.ToInt32(users[USER].lastMessage));
                                    if (masters[MASTER].mblistZakaz[i] == zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                    {
                                        //masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                        masters[MASTER].mblistZakaz.RemoveAt(i);
                                    }
                                }
                                for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                {
                                    //Console.WriteLine(masters[MASTER].listZakaz[i].id + " " + Convert.ToInt32(users[USER].lastMessage));
                                    if (masters[MASTER].listZakaz[i] == zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                    {
                                        //masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                        masters[MASTER].listZakaz.RemoveAt(i);
                                    }
                                }
                                users[USER].ChatStep[CHAT] = "0";
                                return;

                            }
                        }
                        return;
                    }
                case "Отчёт":
                    {
                        if (message.Text == "Назад")
                        {
                            users[USER].ChatStep[CHAT] = "0";
                            return;
                        }
                        else
                        {
                            string[] subs = (message.Text.ToString()).Split(' ');
                            string str = "";
                            for (int jj = 0; jj < subs.Length; jj++)
                            {
                                if (subs[jj] == "Чистыми" | subs[jj] == "чистыми" | subs[jj] == "\nЧистыми" | subs[jj] == "\nчистыми")
                                {
                                    for (int jjj = jj+1; jjj < subs.Length; jjj++)
                                    {
                                        int numericValue;
                                        if (int.TryParse(subs[jjj], out numericValue))
                                        {
                                            str = str + subs[jjj];
                                        }
                                    }
                                }
                            }
                            if (str != "")
                            {
                                for (int ji = 0; ji < users.Count; ji++)
                                {
                                    if (users[ji].Id == zakazList[Convert.ToInt32(users[USER].lastMessage)].otpravitel)
                                    {

                                        await botClient.SendTextMessageAsync(users[ji].chats[0], "От мастера " + masters[MASTER].fio + "\nОтчёт по заказу № " + zakazList[Convert.ToInt32(users[USER].lastMessage)].id + " комментарий:\n" + message.Text);
                                        int zm = -1;


                                        //masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].listZakaz[zm].ended = true;
                                        zakazList[Convert.ToInt32(users[USER].lastMessage)].ended = true;
                                        zakazList[Convert.ToInt32(users[USER].lastMessage)].otchet = message.Text;
                                        zakazList[Convert.ToInt32(users[USER].lastMessage)].sum= Convert.ToInt32(str);
                                        //await botClient.SendTextMessageAsync(message.Chat, "Заказ принят");

                                        //for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                        //{
                                        //    //Console.WriteLine(masters[MASTER].listZakaz[i].id + " " + Convert.ToInt32(users[USER].lastMessage));
                                        //    if (masters[MASTER].listZakaz[i].id == zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                        //    {
                                        //        //masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                        //        //masters[MASTER].listZakaz.RemoveAt(i);
                                        //    }
                                        //}
                                        users[USER].ChatStep[CHAT] = "0";
                                        return;

                                    }
                                }
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(message.Chat, "В отчёте не найденно слово \"чистыми\" \nВведите заново");
                                users[USER].ChatStep[CHAT] = "Отчёт";
                            }
                        }
                        return;
                    }
                case "Перенос даты ремонта":
                    {
                        if (message.Text == "Назад")
                        {
                            users[USER].ChatStep[CHAT] = "0";
                            return;
                        }
                        string[] subs = (message.Text.ToString()).Split(' ');
                        if (subs.Length!=10)
                        {

                            {
                                await botClient.SendTextMessageAsync(message.Chat, "ошибка при вводе");
                                keybord(4, message, botClient, USER);
                                users[USER].ChatStep[CHAT] = "5";
                                return;
                            }
                        }
                        for (int i = 0; i < subs.Length; i++)
                        {
                            //Console.WriteLine(subs[i]);
                            if (subs[i] != "-")
                            {
                                int numericValue;
                                if (int.TryParse(subs[i], out numericValue))
                                {

                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(message.Chat, "ошибка при вводе");
                                    keybord(4, message, botClient, USER);
                                    users[USER].ChatStep[CHAT] = "5";
                                    return;
                                }
                            }
                        }
                        int zg = Convert.ToInt32(subs[1]);
                        DateTime date1 = new DateTime(Convert.ToInt32(subs[2]), Convert.ToInt32(subs[1]), Convert.ToInt32(subs[0]), Convert.ToInt32(subs[3]), Convert.ToInt32(subs[4]), 0);
                        zakazList[Convert.ToInt32(users[USER].lastMessage)].TimeStart = date1;
                        DateTime date2 = new DateTime(Convert.ToInt32(subs[8]), Convert.ToInt32(subs[7]), Convert.ToInt32(subs[6]), Convert.ToInt32(subs[9]), Convert.ToInt32(subs[10]), 0);
                        zakazList[Convert.ToInt32(users[USER].lastMessage)].TimeStart = date2;
                        keybord(4, message, botClient, USER);
                        users[USER].ChatStep[CHAT] = "5";
                        await botClient.SendTextMessageAsync(message.Chat, "Даты ремонта перенесены");
                        return;
                    }
            }
        }
        public static async Task acse_1(Message message, int USER, int CHAT, ITelegramBotClient botClient)
        {
            switch (message.Text)
            {
                case "63821":
                    {
                        users[USER].access = 1;
                        await saveusers();
                        await botClient.SendTextMessageAsync(message.Chat, "Вы мотя");
                        keybord(0, message, botClient, USER);
                        return;

                    }
                case "63822":
                    {
                        users[USER].access = 2;
                        master master = new master();
                        master.id = users[USER].Id;
                        masters.Add(master);
                        await botClient.SendTextMessageAsync(message.Chat, "Вы мастер");
                        await saveusers();
                        await savemasters();
                        keybord(0, message, botClient, USER);



                        return;
                    }

            }
        }




        public static async Task menu2(Message message, int USER, int CHAT, ITelegramBotClient botClient)
        {
            if (users[USER].menu.Count < 1)
            {
                users[USER].menu.Add(0);
            }
            else
                if (message.Text == "Назад")
            {

                users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                keybord(3, message, botClient, USER);


                return;

            }
            int MASTER = -1;
            for (int i = 0; i < masters.Count; i++)
            {
                if (masters[i].id == users[USER].Id)
                {
                    MASTER = i;
                }
            }
            int[] m = users[USER].menu.ToArray();
            for (int i = 0; i < m.Length; i++)
            {
                Console.WriteLine(m[i] + "  lk");
            }
            switch (m[0])
            {
                case 0:
                    {

                        switch (message.Text)
                        {
                            case "Подсказка":
                                {
                                    //await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, slaves! \n Список доступных комманд \n Подсказка \n /AllmyZakazs \n Все заказы \n /notends \n Выбрать заказ");
                                    keybord(3, message, botClient, USER);

                                    return;

                                }
                            case "Назад":
                                {
                                    //await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, slaves! \n Список доступных комманд \n Подсказка \n /AllmyZakazs \n Все заказы \n /notends \n Выбрать заказ");
                                    keybord(3, message, botClient, USER);
                     
                                    return;

                                }

                            case "Все мои заказы":
                                {
                                    keybord(5, message, botClient, USER);
                                    users[USER].menu.Clear();
                                    users[USER].menu.Add(6);



                                    return;

                                }
                            case "Не завершённые":
                                {
                                    if (masters[MASTER].listZakaz.Count > 0)
                                    {
                                        for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                        {

                                            int zm = IDtoNUM_Z(masters[MASTER].listZakaz[i]);
                                            Console.WriteLine(i + " " + zm + " " + zakazList[zm].ended);
                                            if (!zakazList[zm].ended)
                                            {
                                                InlineKeyboardMarkup keyboard = new(new[]
                                                    {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+masters[MASTER].listZakaz[i])
                                                    },
                                                });
                                                await botClient.SendAudioAsync(message.Chat, zakazList[zm].Zakaz.Voice, "Ваш заказ №" + zakazList[zm].id + "\n Время " + zakazList[zm].TimeStart + "\n Адресс " + zakazList[zm].Zakaz.adress + "\n телефон " + zakazList[zm].Zakaz.phone + "\n ФИО " + zakazList[zm].Zakaz.fio + "\n ТИП " + zakazList[zm].Zakaz.type + "\n Другое " + zakazList[zm].Zakaz.hyinya + " \n ended" + zakazList[zm].ended, replyMarkup: keyboard);
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(message.Chat, "У вас нет незавершённых заказов");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "У вас нет заказов");
                                    }


                                    return;

                                }
                            case "Входящие":
                                {
                                    //Console.WriteLine(masters[MASTER].id+" "+ masters[MASTER].mblistZakaz.Count);
                                    if (masters[MASTER].mblistZakaz.Count > 0)
                                    {
                                        for (int i = 0; i < masters[MASTER].mblistZakaz.Count; i++)
                                        {
                                            int zm = IDtoNUM_Z(masters[MASTER].mblistZakaz[i]);
                                            InlineKeyboardMarkup keyboard = new(new[]
                                                {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+masters[MASTER].mblistZakaz[i])
                                                    },
                                                });
                                            await botClient.SendAudioAsync(message.Chat, zakazList[zm].Zakaz.Voice, "Ваш заказ №" + zakazList[zm].id + "\n Время " + zakazList[zm].TimeStart + "\n Адресс " + zakazList[zm].Zakaz.adress + "\n телефон " + zakazList[zm].Zakaz.phone + "\n ФИО " + zakazList[zm].Zakaz.fio + "\n ТИП " + zakazList[zm].Zakaz.type + "\n Другое " + zakazList[zm].Zakaz.hyinya, replyMarkup: keyboard);
                                        }
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "У вас нет заказов");
                                    }


                                    return;

                                }
                            case "/notends":
                                {
                                    users[USER].access = 1;

                                    await botClient.SendTextMessageAsync(message.Chat, "Вы мотя");
                                    return;

                                }
                            case "Выбрать заказ":
                                {
                                    if (masters[MASTER].listZakaz.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказы отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите номер заказа до " + (masters[MASTER].listZakaz.Count - 1));
                                        users[USER].ChatStep[CHAT] = "1";
                                    }
                                    return;
                                }
                        }
                        return;
                    }
                case 1:
                    {
                        keybord(0, message, botClient, USER);
                        return;
                    }
                case 5:
                    {
                        int zakaz = IDtoNUM_Z(users[USER].Zakaz);
                        if (zakaz == -1)
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "Заказ удалён");
                        }
                        else
                        if (m.Length == 1)
                        {
                            switch (message.Text)
                            {
                                case "Принять заказ":
                                    {
                                        //Console.WriteLine(zakazList[Convert.ToInt32(users[USER].lastMessage)].master+ " "+ message.From.Id);
                                        int zzz = 0;
                                        users[USER].ChatStep[CHAT] = message.Text;
                                        Console.WriteLine(masters[MASTER].mblistZakaz.Count + "  " + masters[MASTER].listZakaz.Count);
                                        for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                        {

                                            //Console.WriteLine(zakazList[Convert.ToInt32(users[USER].lastMessage)].id + "   "+ masters[MASTER].listZakaz[i1]);
                                            if (zakazList[zakaz].id == masters[MASTER].listZakaz[i])
                                            {
                                                //Console.WriteLine("FFFFFFFFFFF");
                                                zzz = 1;
                                            }


                                        }
                                        Console.WriteLine(zzz + " KKKKK");
                                        if (zzz != 1)
                                        {
                                            await botClient.SendTextMessageAsync(message.Chat, "Заказ принят");

                                            int xxx = 0;
                                            for (int i = 0; i < users.Count; i++)
                                            {
                                                if (users[i].Id == zakazList[zakaz].otpravitel)
                                                {
                                                    xxx = i;
                                                }
                                            }

                                            await botClient.SendTextMessageAsync(users[xxx].chats[0], "От мастера " + masters[MASTER].fio + "\nЗаказа № " + zakazList[zakaz].id + " принят");

                                            for (int i = 0; i < masters[MASTER].mblistZakaz.Count; i++)
                                            {
                                                //Console.WriteLine(IDtoNUM_Z(masters[MASTER].mblistZakaz[i]) + " " + Convert.ToInt32(users[USER].lastMessage));
                                                if (masters[MASTER].mblistZakaz[i] == zakazList[zakaz].id)
                                                {
                                                    masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                                    masters[MASTER].mblistZakaz.RemoveAt(i);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(message.Chat, "Заказ уже принят");
                                        }
                                        users[USER].ChatStep[CHAT] = "5";

                                        await savemasters();
                                        await savezakazlist();
                                        return;

                                    }
                                case "Отказаться от заказа":
                                    {
                                        users[USER].menu.Add(1);
                                        //zakazList.RemoveAt(Convert.ToInt32(users[USER].lastMessage));
                                        users[USER].ChatStep[CHAT] = "Причина отказа";
                                        ReplyKeyboardMarkup keyboard = new(new[]
                              {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                        {
                                            ResizeKeyboard = true
                                        };


                                        await botClient.SendTextMessageAsync(message.Chat, "Заказу № " + zakazList[zakaz].id + " отказан. \nОпишите причину", replyMarkup: keyboard);

                                        await savemasters();
                                        await savezakazlist();

                                        return;
                                    }
                                case "Назад":
                                    {
                                        //Console.WriteLine("AAA3");
                                        keybord(3, message, botClient, USER);
                                        users[USER].ChatStep[CHAT] = "0";
                                        //Console.WriteLine("aaaaaaaaaaaa");

                                        return;

                                    }
                                case "ДР":
                                    {
                                        int i = Convert.ToInt32(users[USER].lastMessage);
                                        await botClient.SendTextMessageAsync(message.Chat, "Отмеченно ка ДР");
                                        //zakazList[i].master = -1;

                                        zakazList[zakaz].DR = true;
                                        keybord(4, message, botClient, USER);
                                        users[USER].ChatStep[CHAT] = "5";

                                        await savemasters();
                                        await savezakazlist();
                                        return;

                                    }
                                case "Перенос даты ремонта":
                                    {
                                        users[USER].menu.Add(3);
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите двае даты в формате день месяц год пример:\n20 02 2020 14 50 - 21 02 2020 16 20");
                                        users[USER].ChatStep[CHAT] = "Перенос даты ремонта";

                                        await savemasters();
                                        await savezakazlist();
                                        return;

                                    }
                                case "Отчёт":
                                    {
                                        users[USER].menu.Add(4);
                                        await botClient.SendTextMessageAsync(message.Chat, "Опишите заказ в отчёте");
                                        users[USER].ChatStep[CHAT] = "Отчёт";

                                        await savemasters();
                                        await savezakazlist();
                                        return;

                                    }
                                default:
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "ошибка");
                                        users[USER].ChatStep[CHAT] = "5";

                                        return;
                                    }
                            }

                        }
                        else
                           if (m.Length == 2)
                        { switch (m[1])
                            {
                                case 1:
                                    {

                                        for (int ji = 0; ji < users.Count; ji++)
                                        {
                                            if (users[ji].Id == zakazList[zakaz].otpravitel)
                                            {
                                                await botClient.SendTextMessageAsync(users[ji].chats[0], "От мастера " + masters[MASTER].fio + "\nОтказ от заказа № " + zakazList[zakaz].id + " комментарий:\n" + message.Text);
                                                //await botClient.SendTextMessageAsync(message.Chat, "Заказ принят");
                                                for (int i = 0; i < masters[MASTER].mblistZakaz.Count; i++)
                                                {
                                                    //Console.WriteLine(masters[MASTER].mblistZakaz[i].id + " " + Convert.ToInt32(users[USER].lastMessage));
                                                    if (masters[MASTER].mblistZakaz[i] == zakazList[zakaz].id)
                                                    {
                                                        //masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                                        masters[MASTER].mblistZakaz.RemoveAt(i);
                                                    }
                                                }
                                                for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                                {
                                                    //Console.WriteLine(masters[MASTER].listZakaz[i].id + " " + Convert.ToInt32(users[USER].lastMessage));
                                                    if (masters[MASTER].listZakaz[i] == zakazList[zakaz].id)
                                                    {
                                                        //masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                                        masters[MASTER].listZakaz.RemoveAt(i);
                                                    }
                                                }
                                                users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                                return;

                                            }
                                        }
                                        return;
                                    }
                                case 4:
                                    {

                                        {
                                            string[] subs = (message.Text.ToString()).Split(' ');
                                            string str = "";
                                            for (int jj = 0; jj < subs.Length; jj++)
                                            {
                                                if (subs[jj] == "Чистыми" | subs[jj] == "чистыми" | subs[jj] == "\nЧистыми" | subs[jj] == "\nчистыми")
                                                {
                                                    for (int jjj = jj + 1; jjj < subs.Length; jjj++)
                                                    {
                                                        int numericValue;
                                                        if (int.TryParse(subs[jjj], out numericValue))
                                                        {
                                                            str = str + subs[jjj];
                                                        }
                                                    }
                                                }
                                            }
                                            if (str != "")
                                            {
                                                for (int ji = 0; ji < users.Count; ji++)
                                                {
                                                    if (users[ji].Id == zakazList[zakaz].otpravitel)
                                                    {

                                                        await botClient.SendTextMessageAsync(users[ji].chats[0], "От мастера " + masters[MASTER].fio + "\nОтчёт по заказу № " + zakazList[zakaz].id + " комментарий:\n" + message.Text);
                                                        int zm = -1;


                                                        //masters[IDtoNUM_M(zakazList[Convert.ToInt32(users[USER].lastMessage)].master)].listZakaz[zm].ended = true;
                                                        zakazList[zakaz].ended = true;
                                                        zakazList[zakaz].otchet = message.Text;
                                                        zakazList[zakaz].sum = Convert.ToInt32(str);
                                                        //await botClient.SendTextMessageAsync(message.Chat, "Заказ принят");

                                                        //for (int i = 0; i < masters[MASTER].listZakaz.Count; i++)
                                                        //{
                                                        //    //Console.WriteLine(masters[MASTER].listZakaz[i].id + " " + Convert.ToInt32(users[USER].lastMessage));
                                                        //    if (masters[MASTER].listZakaz[i].id == zakazList[Convert.ToInt32(users[USER].lastMessage)].id)
                                                        //    {
                                                        //        //masters[MASTER].listZakaz.Add(masters[MASTER].mblistZakaz[i]);
                                                        //        //masters[MASTER].listZakaz.RemoveAt(i);
                                                        //    }
                                                        //}
                                                        users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                                        return;

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(message.Chat, "В отчёте не найденно слово \"чистыми\" \nВведите заново");
                                                users[USER].ChatStep[CHAT] = "Отчёт";
                                            }
                                        }
                                        return;
                                    }
                                case 3:
                                    {
                                        Console.WriteLine(message.Text);
                                        
                                        string[] subs = (message.Text.ToString()).Split(' ');
                                        if (subs.Length != 11)
                                        {

                                            {
                                                Console.WriteLine(message.Text+"FFFFFF "+subs.Length);
                                                await botClient.SendTextMessageAsync(message.Chat, "ошибка при вводе");
                                                keybord(4, message, botClient, USER);
                                                users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                                return;
                                            }
                                        }
                                        for (int i = 0; i < subs.Length; i++)
                                        {
                                            //Console.WriteLine(subs[i]);
                                            if (subs[i] != "-")
                                            {
                                                int numericValue;
                                                if (int.TryParse(subs[i], out numericValue))
                                                {

                                                }
                                                else
                                                {
                                                    Console.WriteLine(message.Text + "AAAAAAAA");
                                                    await botClient.SendTextMessageAsync(message.Chat, "ошибка при вводе");
                                                    keybord(4, message, botClient, USER);
                                                    users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                                    return;
                                                }
                                            }
                                        }
                                        int zg = Convert.ToInt32(subs[1]);
                                        DateTime date1 = new DateTime(Convert.ToInt32(subs[2]), Convert.ToInt32(subs[1]), Convert.ToInt32(subs[0]), Convert.ToInt32(subs[3]), Convert.ToInt32(subs[4]), 0);
                                        zakazList[zakaz].TimeStart = date1;
                                        DateTime date2 = new DateTime(Convert.ToInt32(subs[8]), Convert.ToInt32(subs[7]), Convert.ToInt32(subs[6]), Convert.ToInt32(subs[9]), Convert.ToInt32(subs[10]), 0);
                                        zakazList[zakaz].TimeStart = date2;
                                        keybord(4, message, botClient, USER);
                                        users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                        await botClient.SendTextMessageAsync(message.Chat, "Даты ремонта перенесены");
                                        return;
                                    }
                            }
                        }
                        return;
                    }
                case 6:
                    {

                        if (masters[MASTER].listZakaz.Count > 0)
                        {


                            //users[USER].ChatStep[CHAT] = message.Text;

                            // users[USER].ChatStep[CHAT] = message.Text;
                            switch (message.Text)
                            {
                                case "День":
                                    {


                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 1, message, botClient);
                                        }

                                        return;

                                    }
                                case "Неделя":
                                    {


                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 7, message, botClient);
                                        }

                                        return;

                                    }
                                case "Месяц":
                                    {


                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 30, message, botClient);
                                        }

                                        return;

                                    }
                                case "3 Месяца":
                                    {


                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 90, message, botClient);
                                        }

                                        return;

                                    }
                                case "Пол года":
                                    {


                                        for (int zm = 0; zm < masters[MASTER].listZakaz.Count; zm++)
                                        {
                                            int i = IDtoNUM_Z(masters[MASTER].listZakaz[zm]);
                                            await ZakazForDateBefore(i, 180, message, botClient);
                                        }

                                        return;

                                    }


                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "У вас нет заказов");
                        }
                        return;

                    }


                default:
                    {
                        users[USER].menu = new List<int>();
                        users[USER].menu.Add(0);
                        keybord(0, message, botClient, USER);
                        return;
                    }
            }
        }

        public static async Task menu1(Message message, int USER, int CHAT, ITelegramBotClient botClient)
        {
            if (users[USER].menu.Count < 1)
            {
                users[USER].menu.Add(0);
            }
            else
                if (message.Text == "Назад")
            {

                users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                keybord(0, message, botClient, USER);


                return;

            }
            int[] m = users[USER].menu.ToArray();
            for (int i = 0; i < m.Length; i++)
            {
                Console.WriteLine(m[i] + "  lk");
            }
            switch (m[0])
            {
                case 0:
                    {
                        switch (message.Text)
                        {

                            case "Подсказка":
                                {
                                    keybord(0, message, botClient, USER);
                                    return;
                                }

                            case "Все заказы":
                                {
                                    keybord(5, message, botClient, USER);
                                    users[USER].menu.Clear();
                                    users[USER].menu.Add(6);

                                    return;
                                }
                            case "Незаконченные":
                                {
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        if (DateTime.Now >= zakazList[i].MbTimeStart.AddHours(3) & zakazList[i].master != -1 & zakazList[i].otchet == "")
                                        {


                                            InlineKeyboardMarkup keyboard = new(new[]
                                                    {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+zakazList[i].id)
                                                    },
                                                });
                                            await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya, replyMarkup: keyboard);

                                        }

                                    }
                                    keybord(0, message, botClient, USER);

                                    return;
                                }
                            case "Законченные":
                                {
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {
                                        //zakazList[i].otchet = "";
                                        //zakazList[i].master = -1;
                                        //Console.WriteLine(zakazList[i].MbTimeStart.DayOfYear + " " + DateTime.Now.DayOfYear);
                                        //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice);
                                        //Console.WriteLine(zakazList[i].MbTimeStart  +" "+ zakazList[i].MbTimeStart.AddHours(3) + " " + zakazList[i].master +" "+ zakazList[i].otchet);
                                        if (DateTime.Now >= zakazList[i].MbTimeStart.AddHours(3) & zakazList[i].master != -1 & zakazList[i].otchet != "")
                                        {


                                            InlineKeyboardMarkup keyboard = new(new[]
                                                    {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+zakazList[i].id)
                                                    },
                                                });
                                            await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya, replyMarkup: keyboard);

                                        }

                                    }
                                    keybord(0, message, botClient, USER);
                                    return;

                                    return;
                                }

                            case "Все мастера":
                                {
                                    for (int i = 0; i < masters.Count; i++)
                                    {

                                        InlineKeyboardMarkup keyboard = new(new[]
                                                {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на мастера № "+masters[i].id)
                                                    },
                                                });
                                        await botClient.SendTextMessageAsync(message.Chat, "Мастер № " + masters[i].id + "\n ФИО " + masters[i].fio + "\n рэйтинг " + masters[i].rayting + "\n профиль " + masters[i].profil, replyMarkup: keyboard);
                                        //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya, replyMarkup: keyboard);



                                        //await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice);
                                        //await botClient.SendTextMessageAsync(message.Chat, "Ваш заказ №" + i + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya);
                                    }
                                    users[USER].menu.Clear();
                                    users[USER].menu.Add(7);
                                    return;
                                }
                            case "Выбрать заказ":
                                {
                                    if (zakazList.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказы отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите ID заказа");
                                        users[USER].menu.Clear();
                                        users[USER].menu.Add(1);
                                    }

                                    return;
                                }
                            case "Выбрать мастера":
                                {
                                    if (masters.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Мастера отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Введите ID мастера");
                                        users[USER].menu.Clear();
                                        users[USER].menu.Add(3);
                                    }

                                    return;
                                }
                            case "Удалить заказ":
                                {
                                    if (zakazList.Count < 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказы отсутствуют");
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, "Для удаления введите  номер заказа до " + (zakazList.Count - 1) + "\n или -1 для выхода");
                                        users[USER].menu.Clear();
                                        users[USER].menu.Add(2);
                                    }

                                    return;
                                }


                            case "/kill":
                                {
                                    Environment.Exit(0);
                                    return;
                                }

                            default:
                                {
                                    // if (message )
                                    shablon(message, USER, CHAT, botClient);
                                    break;

                                }
                        }
                        return;
                    }
                case 1:
                    {
                        keybord(0, message, botClient, USER);
                        return;
                    }
                case 5:
                    {
                        //users[USER].ChatStep[CHAT] = message.Text;
                        int zakaz = IDtoNUM_Z(users[USER].Zakaz);
                        if (zakaz == -1)
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "Заказ удалён");
                        }
                        else
                        if (m.Length == 1)
                        {
                            switch (message.Text)
                            {
                                case "Поручить заказ":
                                    {
                                        users[USER].menu.Add(0);
                                        if (zakazList[zakaz].master == -1)
                                            await botClient.SendTextMessageAsync(message.Chat, "Введите № мастера");
                                        else
                                            await botClient.SendTextMessageAsync(message.Chat, "Введите № мастера если хотите заменить текущего");
                                        return;

                                    }
                                case "Удалить заказ":
                                    {
                                        //users[USER].menu.Add(1);
                                        for (int i1 = 0; i1 < masters[IDtoNUM_M(zakazList[zakaz].master)].listZakaz.Count; i1++)
                                        {
                                            if (masters[IDtoNUM_M(zakazList[zakaz].master)].listZakaz[i1] == zakazList[zakaz].id)
                                            {
                                                masters[IDtoNUM_M(zakazList[zakaz].master)].listZakaz.RemoveAt(i1);
                                            }
                                        }
                                        for (int i1 = 0; i1 < masters[IDtoNUM_M(zakazList[zakaz].master)].mblistZakaz.Count; i1++)
                                        {
                                            if (masters[IDtoNUM_M(zakazList[zakaz].master)].mblistZakaz[i1] == zakazList[zakaz].id)
                                            {
                                                masters[IDtoNUM_M(zakazList[zakaz].master)].mblistZakaz.RemoveAt(i1);
                                            }
                                        }
                                        zakazList.RemoveAt(zakaz);
                                        await savezakazlist();
                                        await botClient.SendTextMessageAsync(message.Chat, "Заказ № " + Convert.ToInt32(users[USER].lastMessage) + " удалён");

                                        return;
                                    }
                                case "Снять мастера":
                                    {
                                        //users[USER].menu.Add(2);
                                        int i = zakaz;
                                        //Console.WriteLine("gfdshsdh " + i);
                                        zakazList[i].master = -1;
                                        int MASTER = -1;
                                        for (int iJ = 0; iJ < masters.Count; iJ++)
                                        {

                                            for (int iJ1 = 0; iJ1 < masters[iJ].listZakaz.Count; iJ1++)
                                            {
                                                if (masters[iJ].listZakaz[iJ1] == zakazList[i].id)
                                                {
                                                    masters[iJ].listZakaz.RemoveAt(iJ1);
                                                }
                                            }
                                            for (int iJ1 = 0; iJ1 < masters[iJ].mblistZakaz.Count; iJ1++)
                                            {
                                                if (masters[iJ].mblistZakaz[iJ1] == zakazList[i].id)
                                                {
                                                    masters[iJ].mblistZakaz.RemoveAt(iJ1);
                                                }
                                            }

                                        }

                                        keybord(1, message, botClient, USER);
                                        return;

                                    }

                            }
                        }
                        else if (m.Length == 2)
                        {
                            switch (m[1])
                            {
                                //поручить
                                case 0:
                                    {
                                        int numericValue;
                                        //long i = -1;
                                        if (message.Text == "Назад")
                                        {
                                            users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                            keybord(0, message, botClient, USER);
                                            return;
                                        }
                                        int zm = -1;
                                        Console.WriteLine(message.Text);


                                        //long i = Convert.ToInt64(message.Text);
                                        long i = 0;

                                        //zm;
                                        //if (int.TryParse(message.Text, out numericValue))
                                        if (long.TryParse(message.Text, out i))
                                        {
                                            for (int j = 0; j < users.Count; j++)
                                            {
                                                if (users[j].Id == i)
                                                {
                                                    zm = j;
                                                }
                                            }
                                            //long i = 
                                            //int zm = IDtoNUM_Z(i);
                                            Console.WriteLine(" " + zm + " " + i);
                                            if (zm != -1)
                                            {
                                                if (zakazList[zakaz].master != -1)
                                                {
                                                    for (int i1 = 0; i1 < masters[IDtoNUM_M(zakazList[zakaz].master)].listZakaz.Count; i1++)
                                                    {
                                                        if (masters[IDtoNUM_M(zakazList[zakaz].master)].listZakaz[i1] == zakazList[zakaz].id)
                                                        {
                                                            masters[IDtoNUM_M(zakazList[zakaz].master)].listZakaz.RemoveAt(i1);
                                                        }
                                                    }
                                                    for (int i1 = 0; i1 < masters[IDtoNUM_M(zakazList[zakaz].master)].mblistZakaz.Count; i1++)
                                                    {
                                                        if (masters[IDtoNUM_M(zakazList[zakaz].master)].mblistZakaz[i1] == zakazList[zakaz].id)
                                                        {
                                                            masters[IDtoNUM_M(zakazList[zakaz].master)].mblistZakaz.RemoveAt(i1);
                                                        }
                                                    }
                                                }
                                                masters[IDtoNUM_M(i)].mblistZakaz.Add(zakazList[zakaz].id);
                                                zakazList[zakaz].otpravitel = users[USER].Id;
                                                zakazList[zakaz].master = i;

                                                await savemasters();

                                                await savezakazlist();


                                                //Console.WriteLine(users[masters[i].id].Id + " " + masters[i].id + " "+i);
                                                await botClient.SendTextMessageAsync(message.Chat, "Порученно мастеру № " + i);
                                                await botClient.SendTextMessageAsync(users[zm].chats[0], "Вам пришёл заказ");
                                                keybord(1, message, botClient, USER);

                                                users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(message.Chat, "Неверный номер мастера  " + (masters.Count - 1));
                                            }
                                        }
                                        else
                                        {
                                            await botClient.SendTextMessageAsync(message.Chat, "Неверный номер мастера  " + (masters.Count - 1));
                                        }

                                        return;
                                    }
                                //удалить
                                case 1:
                                    {
                                        keybord(0, message, botClient, USER);
                                        return;
                                    }
                                //снять мастера
                                case 2:
                                    {
                                        keybord(0, message, botClient, USER);
                                        return;
                                    }
                            }
                        }
                        return;
                    }
                case 6:
                    {
                        switch (message.Text)
                        {
                            case "День":
                                {

                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 1, message, botClient);


                                    }
                                    return;

                                }
                            case "Неделя":
                                {
                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 7, message, botClient);

                                    }
                                    return;

                                }
                            case "Месяц":
                                {


                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 30, message, botClient);

                                    }
                                    return;

                                }
                            case "3 Месяца":
                                {

                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 90, message, botClient);

                                    }
                                    return;

                                }
                            case "Пол года":
                                {

                                    for (int i = 0; i < zakazList.Count; i++)
                                    {

                                        await ZakazForDateBefore(i, 180, message, botClient);

                                    }
                                    return;

                                }

                        }
                        return;
                    }
                case 7:
                    {
                    int master = IDtoNUM_M(users[USER].Master);
                        if (master == -1)
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "Заказ удалён");
                        }
                        else
                        if (m.Length == 1)
                        {
                            switch (message.Text)
                            {
                                case "Переименовать":
                                    {
                                        users[USER].menu.Add(0);
                                        ReplyKeyboardMarkup keyboard = new(new[]
                              {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                        {
                                            ResizeKeyboard = true
                                        };


                                        await botClient.SendTextMessageAsync(message.Chat, "Введите ФИО мастера", replyMarkup: keyboard);
                                        //await botClient.SendTextMessageAsync(message.Chat, "Введите ФИО мастера");

                                        return;

                                    }
                                case "Изменить рейтинг":
                                    {
                                        users[USER].menu.Add(1);
                                        ReplyKeyboardMarkup keyboard = new(new[]
                              {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                        {
                                            ResizeKeyboard = true
                                        };


                                        await botClient.SendTextMessageAsync(message.Chat, "Введите рэйтинг мастера", replyMarkup: keyboard);
                                        //await botClient.SendTextMessageAsync(message.Chat, "Введите рэйтинг мастера");
                                        return;

                                    }
                                case "Изменить профиль":
                                    {
                                        users[USER].menu.Add(2);
                                        ReplyKeyboardMarkup keyboard = new(new[]
                              {
                                        new KeyboardButton[] {"Назад" }
                                        })
                                        {
                                            ResizeKeyboard = true
                                        };


                                        await botClient.SendTextMessageAsync(message.Chat, "Введите профиль мастера", replyMarkup: keyboard);

                                        //await botClient.SendTextMessageAsync(message.Chat, "Введите профиль мастера");
                                        return;

                                    }

                            }
                        }
                        else
                        {
                            switch (m[1])
                            {
                                case 0:
                                    {

                                        {
                                            int i = master;
                                            masters[i].fio = message.Text;

                                            keybord(2, message, botClient, USER);

                                            //users[USER].ChatStep[CHAT] = "4";
                                            users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                            await savemasters();
                                            return;
                                        }

                                    }
                                case 1:
                                    {

                                        {
                                            int i = master;
                                            masters[i].rayting = Convert.ToInt32(message.Text);

                                            keybord(2, message, botClient, USER);

                                            //users[USER].ChatStep[CHAT] = "4";
                                            users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                            await savemasters();
                                            return;
                                        }

                                    }
                                case 2:
                                    {

                                        {
                                            int i = master;
                                            masters[i].profil = message.Text;

                                            keybord(2, message, botClient, USER);

                                            //users[USER].ChatStep[CHAT] = "4";
                                            users[USER].menu.RemoveAt(users[USER].menu.Count - 1);
                                            await savemasters();

                                            return;
                                        }

                                    }
                            }
            }
                        return;

            }
        
                default:
                    {
                       users[USER].menu = new List<int>();
                       users[USER].menu.Add(0);
                       keybord(0, message, botClient, USER);
                        return;
                    }
            }
        }





        public static async Task savemasters()
        {
            string fileName1 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\masters.txt";
            using FileStream createStream1 = System.IO.File.Create(fileName1);
            await JsonSerializer.SerializeAsync(createStream1, masters);
            await createStream1.DisposeAsync();
        }
        public static async Task saveusers()
        {
            string fileName1 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\users.txt";
            using FileStream createStream1 = System.IO.File.Create(fileName1);
            await JsonSerializer.SerializeAsync(createStream1, users);
            await createStream1.DisposeAsync();
        }
        public static async Task savezakazlist()
        {
            string fileName = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\zakazList.txt";
            using FileStream createStream = System.IO.File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, zakazList);
            await createStream.DisposeAsync();
        }
        public static async Task saveglobals()
        {
            string fileName = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\globals.txt";
            using FileStream createStream = System.IO.File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, globals);
            await createStream.DisposeAsync();
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async Task shablon(Message message, int USER, int CHAT, ITelegramBotClient botClient)
        {
            if (message.Caption != null)
            //if (message.Text.Length >10)
            {


                Zakaz z = new Zakaz();
                ZakazOthet zo = new ZakazOthet();
                zo.Zakaz = z;
                if (message.Voice != null)
                    z.Voice = message.Voice.FileId;
                else
                    z.Voice = message.Audio.FileId;
                int zz = 0;
                string[] subs = (message.Caption.ToString()).Split(' ');
                if (subs.Length > 6)
                {
                    for (int i = 0; i < subs.Length; i++)
                    {
                        if (subs[i].Length > 0)
                        {
                            if (zz == 0)
                            {

                                if (subs[i] == "На" | subs[i] == "на")
                                {

                                    string[] subs1 = subs[i + 1].Split('-');
                                    DateTime date1;
                                    if (DateTime.Now.Hour < Convert.ToInt32(subs1[0]))
                                    {
                                        date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(subs1[0]), Convert.ToInt32(subs1[1]), 0);
                                        zo.TimeStart = date1;
                                        zo.MbTimeStart = date1;
                                    }
                                    else
                                    {
                                        date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(subs1[0]), Convert.ToInt32(subs1[1]), 0);
                                        ;
                                        zo.TimeStart = date1.AddDays(1);
                                        zo.MbTimeStart = date1.AddDays(1);
                                    }
                                    i = i + 2;

                                }
                                else
                                {
                                    DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                                    zo.TimeStart = date1.AddHours(1);
                                    zo.MbTimeStart = date1.AddHours(1);
                                }
                                zo.TimeEnd = zo.TimeStart.AddHours(3);
                                zo.MbTimeEnd = zo.TimeStart.AddHours(3);

                                zz++;
                            }
                            if (zz == 1)
                            {

                                int numericValue;
                                if (int.TryParse(subs[i], out numericValue))
                                {
                                    int numericValue1;
                                    if (int.TryParse(subs[i + 1], out numericValue1))
                                    {
                                        if ((numericValue == 8 | numericValue == 7) & (subs[i + 1].Length > 2) & (numericValue1 / 100 == 9))
                                        {
                                            if (numericValue == 7)
                                            {
                                                z.phone = z.phone + "+" + subs[i];
                                            }
                                            else
                                            {
                                                z.phone = z.phone + subs[i];
                                            }

                                            zz++;
                                            continue;
                                        }
                                        else
                                        {
                                            z.adress = z.adress + " " + subs[i];
                                        }
                                    }
                                    else
                                    {
                                        z.adress = z.adress + " " + subs[i];
                                    }
                                }
                                else
                                {
                                    z.adress = z.adress + " " + subs[i];
                                }
                            }
                            if (zz == 2)
                            {
                                int numericValue;
                                if (int.TryParse(subs[i], out numericValue))
                                {
                                    z.phone = z.phone + subs[i];
                                }
                                else
                                {
                                    string[] subs2 = subs[i].Split('-');
                                    for (int j = 0; j < subs2.Length; j++)
                                    {
                                        if (int.TryParse(subs2[j], out numericValue) & subs2[j] != " ")
                                        {
                                            z.phone = z.phone + subs2[j].ToString();
                                        }
                                        else
                                        {
                                            zz++;
                                            continue;
                                        }

                                    }
                                }
                            }
                            if (zz == 3)
                            {
                                if (subs[i] != " ")
                                {
                                    z.fio = subs[i];
                                    zz++;
                                    continue;
                                }
                            }
                            if (zz == 4)
                            {
                                if (subs[i] != " ")
                                {
                                    z.type = subs[i];
                                    zz++;
                                    continue;
                                }
                            }
                            if (zz == 5)
                            {
                                if (subs[i] != " ")
                                {
                                    z.hyinya = subs[i];
                                    zz++;
                                    continue;
                                }
                            }
                        }
                    }
                    zo.id = globals.IdZakazSchet;
                    globals.IdZakazSchet++;
                    saveglobals();
                    if (z.phone != "" & z.adress != "" & z.fio != "" & z.type != "" & z.hyinya != "")
                    {
                        zakazList.Add(zo);

                        await savezakazlist();



                        //System.IO.File.WriteAllText(, Newtonsoft.Json.JsonConvert.SerializeObject());
                        for (int i = 0; i < users[USER].chats.Count; i++)
                        {
                            if (users[USER].ChatsAccess[i] == 1)
                            {
                                await botClient.SendAudioAsync(users[USER].chats[i], zakazList[i].Zakaz.Voice);
                                await botClient.SendTextMessageAsync(users[USER].chats[i], "Ваш заказ №" + i + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya);
                            }
                        }
                    }
                }
            }
        }
        public static async Task ZakazForDateBefore(int i,int day, Message message, ITelegramBotClient botClient)
        {
            if (zakazList[i].MbTimeStart.Year == DateTime.Now.Year & zakazList[i].MbTimeStart.DayOfYear >= DateTime.Now.DayOfYear - day)
            {


                InlineKeyboardMarkup keyboard = new(new[]
                        {
                                                new[]
                                                    {
                                                    InlineKeyboardButton.WithCallbackData("Перейти на заказ № "+zakazList[i].id)
                                                    },
                                                });
                await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya, replyMarkup: keyboard);
            }
            return;
        }
        public static async Task keybord(int fi,Message message, ITelegramBotClient botClient, int USER)
        {
            switch (fi)
            {
                case 0:
                    {
                        ReplyKeyboardMarkup keyboard = new(new[]
                                                    {
                                        new KeyboardButton[] { "Выбрать заказ", "Все заказы","Незаконченные" },
                                        new KeyboardButton[] { "Выбрать мастера", "Все мастера","Законченные" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, slaves! ", replyMarkup: keyboard);
                        return;
                    }
                case 1:
                    {
                        int i = Convert.ToInt32(users[USER].lastMessage);
                        ReplyKeyboardMarkup keyboard = new(new[]
                             {
                                        new KeyboardButton[] { "Удалить заказ", "Поручить заказ","Назад", "Снять мастера" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        string master;
                        //Console.WriteLine(zakazList[i].master);
                        if (zakazList[i].master==-1)
                        { master = "Неназначен"; }
                        else
                        {
                            int MASTER = -1;
                            for (int iJ = 0; iJ < masters.Count; iJ++)
                            {
                                if (masters[iJ].id == zakazList[i].master)
                                {
                                    MASTER = iJ;
                                }
                            }
                            master = masters[MASTER].fio;
                        }
                        //Console.WriteLine(master);
                        await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya + "\n Мастер "+master, replyMarkup: keyboard);

                        return;
                    }
                case 2:
                    {
                        int i = IDtoNUM_M(users[USER].Master);
                        ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] { "Переименовать", "Изменить рейтинг", "Изменить профиль"},
                                        new KeyboardButton[] { "/deletes", "Назад" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(message.Chat, "Мастер № " + masters[i].id + "\n ФИО " + masters[i].fio + "\n рэйтинг " + masters[i].rayting + "\n профиль " + masters[i].profil, replyMarkup: keyboard);
                        return;
                    }
                case 3:
                    {
                        ReplyKeyboardMarkup keyboard = new(new[]
                                                    {
                                        new KeyboardButton[] { "Все мои заказы", "Не завершённые" },
                                        new KeyboardButton[] { "Входящие" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, slaves! ", replyMarkup: keyboard);
                        return;
                    }
                case 4:
                    {
                        int i = Convert.ToInt32(users[USER].lastMessage);
                        ReplyKeyboardMarkup keyboard = new(new[]
                             {
                                        new KeyboardButton[] { "Принять заказ", "Отказаться от заказа","Назад" },
                                        new KeyboardButton[] {  "ДР","Перенос даты ремонта","Отчёт" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendAudioAsync(message.Chat, zakazList[i].Zakaz.Voice, "Ваш заказ №" + zakazList[i].id + "\n Время " + zakazList[i].TimeStart + "\n Адресс " + zakazList[i].Zakaz.adress + "\n телефон " + zakazList[i].Zakaz.phone + "\n ФИО " + zakazList[i].Zakaz.fio + "\n ТИП " + zakazList[i].Zakaz.type + "\n Другое " + zakazList[i].Zakaz.hyinya + "\n Мастер ууу"   + "\n отчёт " +zakazList[i].ended+"\n"+ zakazList[i].TimeStart+" "+ zakazList[i].TimeEnd, replyMarkup: keyboard);

                        return;
                    }
                case 5:
                    {
                        //int i = Convert.ToInt32(users[USER].lastMessage);
                        ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] {"День","Неделя","Месяц" },
                                        new KeyboardButton[] {"3 Месяца","Пол года","Назад" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(message.Chat, "Выберите период", replyMarkup: keyboard);
                        return;
                    }
                case -1:
                    {
                        //int i = Convert.ToInt32(users[USER].lastMessage);
                        ReplyKeyboardMarkup keyboard = new(new[]
                          {
                                        new KeyboardButton[] {"Назад" }
                                        })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(message.Chat, "Вы ошиблись при Вводе \n Введите другое число", replyMarkup: keyboard);
                        return;
                    }               
            }

        }
        public static int IDtoNUM_Z(long id)
        {
            int zm = -1;
            for (int j = 0; j < zakazList.Count; j++)
            {
                if (zakazList[j].id == id)
                {
                    zm = j;
                }
            }
            return zm;
        }
        public static int IDtoNUM_M(long id)
        {
            int zm = -1;
            for (int j = 0; j < masters.Count; j++)
            {
                if (masters[j].id == id)
                {
                    zm = j;
                }
            }
            return zm;
        }
        public static int[] UC(long id,Chat chat)
        {
            int CHAT = -1;
            int USER = -1;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Id == id)
                {
                    USER = i;
                }
            }
            if (USER == -1)
            {
                user user1 = new user();
                user1.Id = id;
                USER = users.Count();
                CHAT = user1.chats.Count();
                user1.ChatsAccess.Add(0);
                user1.ChatStep.Add("0");
                user1.chats.Add(chat);
                users.Add(user1);

                
            }
            else
            {

                {
                    for (int i = 0; i < users[USER].chats.Count; i++)
                    {
                        if (users[USER].chats[i].Id == chat.Id)
                        {
                            CHAT = i;
                        }
                    }
                    if (CHAT == -1)
                    {
                        CHAT = users[USER].chats.Count();
                        users[USER].chats.Add(chat);
                        users[USER].ChatsAccess.Add(0);
                        users[USER].ChatStep.Add("0");

                    }
                }
            }
            int[] UC = new[] { USER, CHAT };
            return UC;
        }
        public static async Task MASTERN()
        {

        }

        public static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            Console.WriteLine(callbackQuery.Data);
            if (callbackQuery.Data.StartsWith("Перейти на заказ"))
            {
                int[] uc=UC(callbackQuery.From.Id, callbackQuery.Message.Chat);
                int USER = uc[0];
                int CHAT = uc[1];
                Console.WriteLine(CHAT + " " + USER + " GGG");
                await saveusers();



                string[] subs = (callbackQuery.Data.ToString()).Split(' ');
               
                
                Message message = callbackQuery.Message;

                int ij = IDtoNUM_Z(Convert.ToInt32(subs[4]));
                users[USER].Zakaz = (Convert.ToInt32(subs[4]));
                users[USER].lastMessage = ij.ToString();
                //Console.WriteLine(USER + " " + CHAT);t
                //Console.WriteLine(USER + " " + subs[4]+" "+zm);
                //if (zm!=-1)
                //await botClient.SendTextMessageAsync(message.Chat, "Ваш заказ №" + zakazList[ij].id + "\n Время " + zakazList[ij].TimeStart + "\n Адресс " + zakazList[ij].Zakaz.adress + "\n телефон " + zakazList[ij].Zakaz.phone + "\n ФИО " + zakazList[ij].Zakaz.fio + "\n ТИП " + zakazList[ij].Zakaz.type + "\n Другое " + zakazList[ij].Zakaz.hyinya + "\n Мастер ");
                switch (users[USER].access)
                {
                    case 1:
                        {
                            keybord(1, callbackQuery.Message, botClient, USER);
                            users[USER].ChatStep[CHAT] = "5";
                            users[USER].menu = new List<int>();
                            users[USER].menu.Add(5);
                            return;
                        }
                    case 2:
                        {
                            keybord(4, callbackQuery.Message, botClient, USER);
                            users[USER].ChatStep[CHAT] = "5";
                            users[USER].menu = new List<int>();
                            users[USER].menu.Add(5);
                            return;
                        }
                }

                
                
                return;
            }
            if (callbackQuery.Data.StartsWith("Перейти на мастера"))
            {
                int[] uc = UC(callbackQuery.From.Id, callbackQuery.Message.Chat);
                int USER = uc[0];
                int CHAT = uc[1];

                await saveusers();



                string[] subs = (callbackQuery.Data.ToString()).Split(' ');


                Message message = callbackQuery.Message;

                Console.WriteLine(subs[4]);
                users[USER].Master = (Convert.ToInt64(subs[4]));

                //Console.WriteLine(USER + " " + CHAT);t
                //Console.WriteLine(USER + " " + subs[4]+" "+zm);
                //if (zm!=-1)
                //await botClient.SendTextMessageAsync(message.Chat, "Ваш заказ №" + zakazList[ij].id + "\n Время " + zakazList[ij].TimeStart + "\n Адресс " + zakazList[ij].Zakaz.adress + "\n телефон " + zakazList[ij].Zakaz.phone + "\n ФИО " + zakazList[ij].Zakaz.fio + "\n ТИП " + zakazList[ij].Zakaz.type + "\n Другое " + zakazList[ij].Zakaz.hyinya + "\n Мастер ");
                switch (users[USER].access)
                {
                    case 1:
                        {
                            keybord(2, callbackQuery.Message, botClient, USER);
                            users[USER].menu = new List<int>();
                            users[USER].menu.Add(7);
                            return;
                        }
                    case 2:
                        {
                            keybord(4, callbackQuery.Message, botClient, USER);
                            users[USER].ChatStep[CHAT] = "5";
                            users[USER].menu = new List<int>();
                            users[USER].menu.Add(7);
                            return;
                        }
                }



                return;
            }

            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"You choose with data: {callbackQuery.Data}"
                );
            return;
        }

        static void Main(string[] args)
        {

                Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            string fileName = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\zakazList.txt";
            string jsonString = System.IO.File.ReadAllText(fileName);
            zakazList = JsonSerializer.Deserialize<List<ZakazOthet>>(jsonString)!;

            string fileName1 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\users.txt";
            string jsonString1 = System.IO.File.ReadAllText(fileName1);
            users = JsonSerializer.Deserialize<List<user>>(jsonString1)!;

            string fileName2 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\masters.txt";
            string jsonString2 = System.IO.File.ReadAllText(fileName2);
            masters = JsonSerializer.Deserialize<List<master>>(jsonString2)!;
            
            //string fileName3 = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\masters.txt";
            //string jsonString3 = System.IO.File.ReadAllText(fileName3);
            //globals = JsonSerializer.Deserialize<globals>(jsonString3)!;

            //string fileName = @"C:\Users\10PC\Desktop\TelegaBot\dateSaves\zakazList.txt";
            //using FileStream openStream = System.IO.File.OpenRead(fileName);
            //List<ZakazOthet>? zakazList =
            //await JsonSerializer.DeserializeAsync<WeatherForecast>(openStream);
            //zakazList = Newtonsoft.Json.JsonConvert.DeserializeObject<ZakazOthet>(System.IO.File.ReadAllText(@"C:\Users\10PC\Desktop\TelegaBot\dateSaves\zakazList.txt"));

            var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;

                var receiverOptions = new ReceiverOptions

                {
                    AllowedUpdates = { }, // receive all update types
                };

                bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

                Console.ReadLine();
            Console.ReadLine();

        }
    }
}