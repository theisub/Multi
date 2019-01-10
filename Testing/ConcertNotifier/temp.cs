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

namespace ConcertNotifier
{
    class Program
    {
        private static Telegram.Bot.TelegramBotClient bot = new Telegram.Bot.TelegramBotClient("649402348:AAEV6TUtYGCe46CFQKfA8axidFi2_7Bj9mo");
        private static DbSet<tblUser> SelectAllUsers()
        {

            myContext concertDB = new myContext();
            using (var context = new myContext())
            {
                foreach (var user in context.tblUsers)
                    Console.WriteLine(user.user_city + " and " + user.user_id);
            }

                return concertDB.tblUsers;
        }
        private static DbSet<tblGroup> SelectAllGroups()
        {
            myContext concertDB = new myContext();
            return concertDB.tblGroups;
        }

        private static DbSet<tblNotification> SelectAllNotifications()
        {
            myContext concertDB = new myContext();
            return concertDB.tblNotifications;
        }
        private static DbSet<tblSubscription> SelectAllSubscriptions()
        {
            myContext concertDB = new myContext();
            return concertDB.tblSubscriptions;
        }
        private static IList<tblSubscription> SelectSubscriptions(int user)
        {
            myContext concertDB = new myContext();
            return concertDB.tblSubscriptions.Where(b => b.user_id == user).ToList();
        }
        
        private static void InsertUser(int id, string city)
        {
            myContext concertDB = new myContext();
            var user = new tblUser();
            user.user_id = id;
            user.user_city = city;
            concertDB.tblUsers.Add(user);
            concertDB.SaveChanges();            
        }
        private static void InsertGroup(int id, string name)
        {
            myContext concertDB = new myContext();
            var group = new tblGroup();

            group.group_id = id;
            group.group_name = name;
            concertDB.tblGroups.Add(group);
            concertDB.SaveChanges();
        }

        private static void InsertNotification(int concertid, int id, DateTime date)
        {
            myContext concertDB = new myContext();
            var notification = new tblNotification();
            var counter =SelectAllNotifications();
            notification.notification_id = counter.Count() +1;
            notification.concert_id = concertid;
            notification.user_id = id;
            notification.notification_date = date;
            
            concertDB.tblNotifications.Add(notification);
            concertDB.SaveChanges();
        }
        private static void DeleteAllSubscriptions(int user)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblSubscriptions.Where(b => b.user_id == user);

            if (result != null)
            {
                foreach (var item in result)
                {
                    concertDB.tblSubscriptions.Remove(item);
                }
                concertDB.SaveChanges();
            }
        }

        private static void DeleteSubscription(int user, string name)
        {
            myContext concertDB = new myContext();
            var group = FindGroupByName(name);

            var Result = concertDB.tblSubscriptions.Where(b => (b.user_id == user) && (b.group_id == group.group_id));

            if (Result != null)
            {
                foreach (var item in Result)
                    concertDB.tblSubscriptions.Remove(item);
                concertDB.SaveChanges();
            }


        }

        private static void DeleteNotification(int user, string name)
        {
            myContext concertDB = new myContext();
            //var group = find
            var group = FindGroupByName(name);

            var concert = FindConcertById(group.group_id);

            var result = concertDB.tblNotifications.Where(b => b.concert_id == concert.concert_id);

            if (result != null)
            {
                foreach (var item in result)
                    concertDB.tblNotifications.Remove(item);
                concertDB.SaveChanges();
            }


        }

        private static void DeleteAllGroups(int user)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups;

            if (result != null)
            {
                foreach (var item in result)
                {
                    concertDB.tblGroups.Remove(item);
                }
                concertDB.SaveChanges();
            }
        }
        private static void InsertSubscription(int user, int group)
        {
            myContext concertDB = new myContext();
            var sub = new tblSubscription();
            var subs = SelectAllSubscriptions();
            sub.group_id = group;
            sub.user_id = user;
            sub.subscription_id = subs.Count() + 1;
            concertDB.tblSubscriptions.Add(sub);
            concertDB.SaveChanges();
        }
        private static void InsertConcert(Concert concert, int group)
        {
            myContext concertDB = new myContext();
            var conc = new tblConcert();

            conc.concert_id = concert.ID;
            conc.group_id = group;
            conc.concert_city = concert.Location;
            conc.concert_date = concert.Date;
            conc.concert_link = concert.Link;
            conc.concert_place = concert.Place;
            conc.concert_title = concert.Title;

            concertDB.tblConcerts.Add(conc);
            concertDB.SaveChanges();
        }
        private static bool ContainsUser(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblUsers.Where(b => b.user_id == id);

            if (result.Count() > 0)
                return true;
            else
                return false;
        }
        private static bool ContainsGroup(string name)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups.Where(b => b.group_name == name);

            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        private static bool ContainsSubscription(string name,int user)
        {
            myContext concertDB = new myContext();
            var group = FindGroupByName(name);
            var result = concertDB.tblSubscriptions.Where(b => (b.group_id == group.group_id) &&  (b.user_id == user));

            if (result.Count() > 0)
                return true;
            else
                return false;
        }
        private static tblUser FindUser(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblUsers.Where(b => b.user_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        private static tblConcert FindGroupIdByConcert(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblConcerts.Where(b => b.concert_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;


        }
        private static tblGroup FindGroupByName(string name)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups.Where(b => b.group_name == name);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        private static tblConcert FindConcertById(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblConcerts.Where(b => b.group_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        private static tblGroup FindGroupById(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups.Where(b => b.group_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }
        private static void UpdateUser(int id, string city)
        {
            
            myContext concertDB = new myContext();
            tblUser user = concertDB.tblUsers.Where(b => b.user_id == id).First();
            user.user_city = city;
            concertDB.SaveChanges();            
        }
        private static void UpdateGroup(int id, string name)
        {

            myContext concertDB = new myContext();
            tblGroup group = concertDB.tblGroups.Where(b => b.group_id == id).First();
            group.group_name = name;
            concertDB.SaveChanges();
        }
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
        private static string GetCity (string mess_city)
        {
            var city = "";
            switch (mess_city)
            {
                case "Москва":
                    city = "Москва";
                    break;
                case "Санкт-Петербург":
                    city = "Saint-Petersburg";
                    break;
                case "Казань":
                    city = "Kazan";
                    break;
                case "Новосибирск":
                    city = "Novosibirsk";
                    break;
                case "Екатеринбург":
                    city = "Ekaterinburg";
                    break;
                case "Moscow":
                    city = "Москва";
                    break;
                case "Saint-Petersburg":
                    city = "Санкт-Петербург";
                    break;
                case "Kazan":
                    city = "Казань";
                    break;
                case "Novosibirsk":
                    city = "Новосибирск";
                    break;
                case "Ekaterinburg":
                    city = "Екатеринбург";
                    break;
            }

            return city;
        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
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
                            if (ContainsUser(user))
                            {
                                var select = SelectSubscriptions(user);
                                if (select != null)
                                {
                                    List<Concert> concerts = new List<Concert>();
                                    int i = 1;
                                    foreach (var item in select)
                                    {

                                        var id = item.group_id;
                                        var find = FindGroupById(id);
                                        
                                        if (find != null)
                                        {
                                            var name = find.group_name;
                                            var kudago = new KudaGoSearchApi();

                                            var dbuser = FindUser(user);
                                            var concert = kudago.GetCityConcertsByGroup(name, dbuser.user_city);
                                            var test = concert.FirstOrDefault();

                                            concerts.Add(concert.FirstOrDefault());
                                            //concert = concert.OrderBy(x => x.Date).ToList();
                                           
                                        }
                                    }

                                    if (concerts.Count() > 0)
                                    {
                                        concerts = concerts.OrderBy(x => x.Date).ToList();
                                        foreach (var conc in concerts)
                                            SendInfoToUser(user, FindGroupById(FindGroupIdByConcert(conc.ID).group_id).group_name, conc);
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
                                string city = GetCity(mess_city);
                                if (city != "")
                                {

                                    answer = String.Format("Ваш город {0}. Добавьте группу с помощью команды \"/add <Группа>\".", 
                                        mess_city);
                                    Console.WriteLine(msg.Chat.Id.ToString());
                                    var user = Convert.ToInt32(msg.Chat.Id);
                                    if (ContainsUser(user))
                                    {
                                        UpdateUser(user, city);
                                    }
                                    else
                                    {
                                        InsertUser(user, city);
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
                                var group = string.Join(" ", message.Skip(1));
                                var kudago = new KudaGoSearchApi();
                                var user = Convert.ToInt32(msg.Chat.Id);
                                var dbuser = FindUser(user);
                                if (dbuser != null)
                                {
                                    var mess_city = dbuser.user_city;
                                    var city = GetCity(mess_city);
                                    var concerts = kudago.GetCityConcertsByGroup(group, city);
                                    if (concerts != null)
                                    {
                                        answer = String.Format("Вы добавили группу {0}. Вывод списка групп с помощью команды \"/list\".", 
                                            group);
                                        Console.WriteLine(msg.Chat.Id.ToString());

                                        if (!ContainsGroup(group))
                                        {
                                            var groups = SelectAllGroups();
                                            var group_id = groups.Count() + 1;                                        
                                            InsertGroup(group_id, group);
                                            InsertSubscription(user, group_id);
                                            foreach (var conc in concerts)
                                            {
                                                InsertConcert(conc, group_id);
                                                InsertNotification(conc.ID, user, conc.Date.AddDays(-7));

                                            }
                                        }
                                        
                                        else
                                        {
                                            var CurrentGroup = FindGroupByName(group);
                                            if (!ContainsSubscription(group, user))
                                            {
                                                InsertSubscription(user, CurrentGroup.group_id);
                                                foreach (var conc in concerts)
                                                {
                                                    InsertNotification(conc.ID, user, conc.Date.AddDays(-7));
                                                }
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


                    case "/remove":
                        {
                            if (message.Count() > 1)
                            {
                                var group = string.Join(" ", message.Skip(1));
                                var user = Convert.ToInt32(msg.Chat.Id);
                                var dbuser = FindUser(user);
                                if (dbuser != null)
                                {
                                    if (ContainsSubscription(group, user))
                                    {
                                        DeleteSubscription(user, group);
                                        DeleteNotification(user, group);
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
                                answer = "Введите названеи группы, которую вы хотите удалить из подписок.";
                        }
                        break;
                    case "/list":
                        {
                            var user = Convert.ToInt32(msg.Chat.Id);

                            if (ContainsUser(user))
                            {
                                var select = SelectSubscriptions(user);
                                if (select != null)
                                {
                                    int i = 1;
                                    foreach (var item in select)
                                    {
                                        var id = item.group_id;
                                        var find = FindGroupById(id);
                                        if (find != null)
                                        {
                                            var name = find.group_name;
                                            answer += String.Format("{0}) {1}\n", i++, name);
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
                            if (ContainsUser(user))
                            {
                                DeleteAllSubscriptions(user);
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
           // bot = new Telegram.Bot.TelegramBotClient(ConfigurationManager.AppSettings["TeleramKey"],wp);
           
            bot.OnMessage += BotOnMessageReceived;
            var me = bot.GetMeAsync().Result;
            Console.Title = me.Username;

            bot.StartReceiving(new UpdateType[] { UpdateType.MessageUpdate });
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }

        private static void ShowConcerts(Object state)
        {
            var select = SelectAllSubscriptions();
            if (select != null)
            {
                foreach (var item in select)
                {
                    var group = item.group_id;
                    var groupName = FindGroupById(group).group_name;
                    var user = item.user_id;
                    var mess_city = FindUser(user).user_city;
                    string city = GetCity(mess_city);
                    
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
            var str = FindGroupByName("Jamiroquai");
            var str1 = FindGroupById(1);
            CreateBot();
            Thread.Sleep(10000);
            Timer t = new Timer(ShowConcerts, null, 0, 60000);
            
            Console.ReadKey();
        }
    }
}
