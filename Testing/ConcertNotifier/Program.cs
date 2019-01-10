using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.Entity;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using ConcertSearchApi.Api;
using ConcertSearchApi.Models;
using System.Threading;
using System.Configuration;

namespace ConcertNotifier
{
    class Program
    {
        private static Telegram.Bot.TelegramBotClient bot = new Telegram.Bot.TelegramBotClient("649402348:AAEV6TUtYGCe46CFQKfA8axidFi2_7Bj9mo");
        
       
        
    
       
        
       
        private static async void SendInfoToUser(int user, string group, Concert concert)
        {
            try
            {
                var message = String.Format("\nИнфо о концерте группы {0}\nНазвание: {1}\nМесто: {2}\nВремя: " +
                "{3}\nСсылка на источник: {4}\n", group, concert.Title, concert.Place, concert.Date, concert.Link);
                await bot.SendTextMessageAsync(user, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            UserActions userActions = new UserActions();
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            ConcertActions concertActions = new ConcertActions();
            try
            {
                Telegram.Bot.Types.Message msg = messageEventArgs.Message;
                if (msg == null || msg.Type != MessageType.TextMessage) return;

                string answer = "";
                var message = msg.Text.Trim().Split(' ');

                switch (message[0])
                {
                    case "/start":
                        {
                            answer = "Укажите Ваш город с помощью команды \"/city <Город>\".";
                        }

                        break;
                    case "/show":
                        {
                            var user = Convert.ToInt32(msg.Chat.Id);

                            //var concerts = kudago.GetCityConcertsByGroup(group, "Москва");
                            if (userActions.ContainsUser(user))
                            {
                                var select = subscriptionActions.SelectSubscriptions(user);
                                if (select != null)
                                {
                                    List<Concert> concerts = new List<Concert>();
                                    int i = 1;
                                    foreach (var item in select)
                                    {

                                        var id = item.group_id;
                                        var find = groupActions.FindGroupById(id);

                                        if (find != null)
                                        {
                                            var name = find.group_name;
                                            var kudago = new KudaGoSearchApi();

                                            var dbuser = userActions.FindUser(user);
                                            var concert = kudago.GetCityConcertsByGroup(name, "Москва");
                                            var test = concert.FirstOrDefault();

                                            concerts.Add(concert.FirstOrDefault());
                                            //concert = concert.OrderBy(x => x.Date).ToList();

                                        }
                                    }

                                    if (concerts.Count() > 0)
                                    {
                                        concerts = concerts.OrderBy(x => x.Date).ToList();
                                       // foreach (var conc in concerts)
                                       //     SendInfoToUser(user, groupActions.FindGroupById(groupActions.FindGroupIdByConcert(conc.ID).group_id).group_name, conc);
                                    }
                                    else
                                        answer = "Не найдено ни одного концерта в вашем городе у исполнителей из вашего списка подписок ";

                                }
                                else
                                {
                                    answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                                }
                            }


                        }
                        break;
                    case "/city":
                        {
                            if (message.Count() > 1)
                            {
                                var mess_city = message[1];
                                string city = CityActions.GetCity(mess_city);
                                if (city != "")
                                {

                                    answer = String.Format("Ваш город {0}. Добавьте группу с помощью команды \"/add <Группа>\".", 
                                        mess_city);
                                    Console.WriteLine(msg.Chat.Id.ToString());
                                    var user = Convert.ToInt32(msg.Chat.Id);
                                    if (userActions.ContainsUser(user))
                                    {
                                        userActions.UpdateUser(user, city);
                                    }
                                    else
                                    {
                                        userActions.InsertUser(user, city);
                                    }
                                }
                                else
                                {
                                    answer = "Вашего города нет в списке.";
                                }
                            }
                            else
                            {
                                answer = "Использование: \"/city <Город>\".";
                            }
                        }
                        break;
                    case "/add":
                        {
                            if (message.Count() > 1)
                            {
                                var group = message[1];
                                var kudago = new KudaGoSearchApi();
                                var user = Convert.ToInt32(msg.Chat.Id);
                                var dbuser = userActions.FindUser(user);
                                if (dbuser != null)
                                {
                                    var mess_city = dbuser.user_city;
                                    var city = CityActions.GetCity(mess_city);
                                    var concerts = kudago.GetCityConcertsByGroup(group, city);
                                    if (concerts != null)
                                    {
                                        answer = String.Format("Вы добавили группу {0}. Вывод списка групп с помощью команды \"/list\".", 
                                            group);
                                        Console.WriteLine(msg.Chat.Id.ToString());

                                        if (!groupActions.ContainsGroup(group))
                                        {
                                            var groups = groupActions.SelectAllGroups();
                                            var group_id = groups.Count() + 1;
                                            groupActions.InsertGroup(group_id, group);
                                            foreach (var conc in concerts)
                                            {
                                               concertActions.InsertConcert(conc, group_id);
                                                /*   InsertNotification(conc.ID, user, conc.Date.AddDays(-7));*/

                                            }
                                            subscriptionActions.InsertSubscription(user, group_id);

                                        }
                                        else
                                        {
                                            var CurrentGroup = groupActions.FindGroupByName(group);
                                            if (!subscriptionActions.ContainsSubscription(group, user))
                                            {
                                                subscriptionActions.InsertSubscription(user, CurrentGroup.group_id);
                                                /*foreach (var conc in concerts)
                                                {
                                                    InsertNotification(conc.ID, user, conc.Date.AddDays(-7));
                                                }*/
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        answer = String.Format("К сожалению, запрашиваемая Вами группа не проводит концертов в городе {0}.",
                                        city);

                                    }
                                }
                                else
                                {
                                    answer = "Укажите Ваш город с помощью команды \"/city <Город>\".";
                                }
                            }
                            else
                            {
                                answer = "Использование: \"/add <Группа>\".";
                            }
                        }
                        break;

                    /*
                case "/remove":
                    answer = "";
                    break;
                     */
                    case "/remove":
                        {
                            if (message.Count() > 1)
                            {
                                var group = string.Join(" ", message.Skip(1));
                                var user = Convert.ToInt32(msg.Chat.Id);
                                var dbuser = userActions.FindUser(user);
                                if (dbuser != null)
                                {
                                    if (subscriptionActions.ContainsSubscription(group, user))
                                    {
                                        subscriptionActions.DeleteSubscription(user, group);
                                        /* DeleteNotification(user, group);*/
                                        answer = String.Format("Вы удалили группу {0} из подписок. Вывод списка групп с помощью команды \"/list\".",
                                            group);
                                    }
                                    else
                                    {

                                        answer = String.Format("Этой группы нет у вас в подписках.",
                                            group);

                                    }




                                }
                            }
                            else
                                answer = "Введите название группы, которую вы хотите удалить из подписок.";
                        }
                        break;
                    case "/list":
                        {
                            var user = Convert.ToInt32(msg.Chat.Id);
                            if (userActions.ContainsUser(user))
                            {
                                var select = subscriptionActions.SelectSubscriptions(user);
                                if (select != null)
                                {
                                    int i = 1;
                                    foreach (var item in select)
                                    {
                                        var id = item.group_id;
                                        var find = groupActions.FindGroupById(id);
                                        if (find != null)
                                        {
                                            var name = find.group_name;
                                            answer += string.Format("{0}) {1}\n", i++, name);
                                        }
                                    }
                                }
                                else
                                {
                                    answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                                }
                            }
                            else
                            {
                                answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                            }
                        }
                        break;
                    case "/clear":
                        {
                            var user = Convert.ToInt32(msg.Chat.Id);
                            if (userActions.ContainsUser(user))
                            {
                                subscriptionActions.DeleteAllSubscriptions(user);
                                answer = "Все Ваши группы удалены.";
                            }
                            else
                            {
                                answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                            }
                        }
                        break;
                    case "/help":
                        answer = "";
                        break;
                    /*default:
                        answer = "default";
                        break;*/
                }
                if (answer != "")
                    await bot.SendTextMessageAsync(msg.Chat.Id, answer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private static void CreateBot()
        {
            //WebProxy wp = new WebProxy(ConfigurationManager.AppSettings["ProxyUri"], true);
           // bot = new Telegram.Bot.TelegramBotClient(ConfigurationManager.AppSettings["TelegramKey"],wp);
           
            bot.OnMessage += BotOnMessageReceived;
            var me = bot.GetMeAsync().Result;
            Console.Title = me.Username;

            bot.StartReceiving(new UpdateType[] { UpdateType.MessageUpdate });
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }

        private static void ShowConcerts(Object state)
        {
            UserActions userActions = new UserActions();
            GroupActions groupActions = new GroupActions();
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            var select = subscriptionActions.SelectAllSubscriptions();
            if (select != null)
            {
                foreach (var item in select)
                {
                    var group = item.group_id;
                    var groupName = groupActions.FindGroupById(group).group_name;
                    var user = item.user_id;
                    var mess_city = userActions.FindUser(user).user_city;
                    string city = CityActions.GetCity(mess_city);
                    
                    var kudago = new KudaGoSearchApi();
                    var concerts = kudago.GetCityConcertsByGroup(groupName, city);
                    if (concerts != null)
                    {
                        foreach (var conc in concerts)
                        {
                            SendInfoToUser(user, groupName, conc);
                        }
                    }
                    
                }
            }
        }
        static void Main(string[] args)
        {
            //InsertUser(1, "City1");
            /*foreach (var user in UserActions.SelectAllUsers())
            {
                Console.WriteLine(user.user_id + " " + user.user_city);
            }*/

            UserActions userActions = new UserActions();
            userActions.InsertUser(22, "City1");
            userActions.InsertUser(22, "City1");

            //CreateBot();
            Thread.Sleep(10000);
            Timer t = new Timer(ShowConcerts, null, 0, 60000);
            
            Console.ReadKey();
        }
    }
}
