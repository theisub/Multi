using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ConcertNotifier
{
    public class SubscriptionActions
    {
        public  DbSet<tblSubscription> SelectAllSubscriptions()
        {
            myContext concertDB = new myContext();
            return concertDB.tblSubscriptions;
        }

        public  IList<tblSubscription> SelectSubscriptions(int user)
        {
            myContext concertDB = new myContext();
            return concertDB.tblSubscriptions.Where(b => b.user_id == user).ToList();
        }
        public string DeleteAllSubscriptions(int user)
        {
            string answer = "Ваш список подписок очищен";

            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return answer;

        }
        public string DeleteSubscription(int user, string name)
        {
            GroupActions groupActions = new GroupActions();
            myContext concertDB = new myContext();
            var group = groupActions.FindGroupByName(name);

            string answer = null;
            var result = concertDB.tblSubscriptions.Where(b => (b.user_id == user) && (b.group_id == group.group_id));

            try
            {
                if (result.ToList().Count()>0)
                    foreach (var item in result)
                        concertDB.tblSubscriptions.Remove(item);
                concertDB.SaveChanges();
                answer = "Исполнитель успешно удален";
            }
            catch (Exception e)
            {
                answer = "У вас нет подписки с таким именем";
                Console.WriteLine(e);
                return answer;
            }




            return answer;
        }
            

         


        

        public string ShowSubList(int user)
        {
            UserActions userActions = new UserActions();
            GroupActions groupActions = new GroupActions();

            string answer = null;


            if (userActions.ContainsUser(user))
            {
                var select = SelectSubscriptions(user);
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
                        else
                        {
                            answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                        }
                    }

                    if (select.Count() ==0)
                    {
                        answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                    }

                }
               
            }
            else
            {
                answer = "Пользователь с таким id не найден. Добавьте город с помощью \"/city";
            }

            return answer;

        }

        public IQueryable<tblSubscription> DeleteSubscription(int user, int group_id)
        {
            GroupActions groupActions = new GroupActions();
            myContext concertDB = new myContext();
            var group = groupActions.FindGroupById(group_id);
            if (group == null)
                return null;
            return null;
            //регресс?
           /* var Result = concertDB.tblSubscriptions.Where(b => (b.user_id == user) && (b.group_id == group.group_id));

            if (Result != null)
            {
                foreach (var item in Result)
                    concertDB.tblSubscriptions.Remove(item);
                concertDB.SaveChanges();
            }

            return Result;*/


        }
        public  void InsertSubscription(int user, int group)
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

        public  bool ContainsSubscription(string name, int user)
        {
            myContext concertDB = new myContext();
            GroupActions groupActions = new GroupActions();
            var group = groupActions.FindGroupByName(name);
            var result = concertDB.tblSubscriptions.Where(b => (b.group_id == group.group_id) && (b.user_id == user));

            if (result.Count() > 0)
                return true;
            else
                return false;
        }
        
    }

}