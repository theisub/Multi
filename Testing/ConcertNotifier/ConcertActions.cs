using ConcertSearchApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace ConcertNotifier
{
    public class ConcertActions
    {
        public DbSet<tblConcert> SelectAllConcerts()
        {
            myContext concertDB = new myContext();
            return concertDB.tblConcerts;
        }
        public  void InsertConcert(Concert concert, int group)
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
        public void InsertConc(tblConcert concert, int group)
        {
            myContext concertDB = new myContext();
            var conc = new tblConcert();

            conc.concert_id = concert.concert_id;
            conc.group_id = group;
            conc.concert_city = concert.concert_city;
            conc.concert_date = concert.concert_date;
            conc.concert_link = concert.concert_link;
            conc.concert_place = concert.concert_place;
            conc.concert_title = concert.concert_title;

            concertDB.tblConcerts.Add(conc);
            concertDB.SaveChanges();
        }

        public void InsertConcerts(Concert concert, int group)
        {
            myContext concertDB = new myContext();
            var conc = new tblConcert();

            conc.concert_id = group + 1;
            conc.group_id = group;
            conc.concert_city = "Test location";
            conc.concert_date = System.DateTime.Now;
            conc.concert_link = "no url";
            conc.concert_place = "place location";
            conc.concert_title = "Test ";

            concertDB.tblConcerts.Add(conc);
            concertDB.SaveChanges();
        }
        /*
        public  tblConcert FindGroupIdByConcert(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblConcerts.Where(b => b.concert_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;


        }*/

        public string ShowConcList(int user)
        {
            UserActions userActions = new UserActions();
            SubscriptionActions subscriptionActions =new  SubscriptionActions();
         
            GroupActions groupActions = new GroupActions();

            string answer = null;
            if (userActions.ContainsUser(user))
            {
                var select = subscriptionActions.SelectSubscriptions(user);
                if (select != null)
                {
                    DbSet<tblConcert> concerts = SelectAllConcerts();
                    
                    int i = 0;
                    foreach (var item in select)
                    {

                        var id = item.group_id;
                        var find = groupActions.FindGroupById(id);

                        if (find != null)
                        {

                        }
                       

                        for (i = 0; i < concerts.Count(); i++)
                        {
                            foreach (var conc in concerts)
                                answer += String.Format("Инфо о концерте группы {0} Название: {1} Место: {2} Время: " +
                                                        "{3} Ссылка на источник: {4} \n", conc.group_id, conc.concert_title, conc.concert_city, conc.concert_date, conc.concert_link);

                        }
                       
                    }

                    if (select.Count() == 0)
                    {
                        answer = "Не найдено ни одного концерта в вашем городе у исполнителей из вашего списка подписок ";
                    }

                   

                }
               
            }

            return answer;

        }


        public string ShowConcList(int user, string user_city)
        {
            UserActions userActions = new UserActions();
            SubscriptionActions subscriptionActions = new SubscriptionActions();

            GroupActions groupActions = new GroupActions();
            bool atleastone = false;
            string answer = null;

            if (userActions.ContainsUser(user))
            {
                var select = subscriptionActions.SelectSubscriptions(user);
                if (select != null)
                {
                    DbSet<tblConcert> concerts = SelectAllConcerts();

                    int i = 0;
                    foreach (var item in select)
                    {

                        var id = item.group_id;
                        var find = groupActions.FindGroupById(id);

                        if (find != null)
                        {

                        }
                        else
                            answer = "Не найдено ни одного концерта в вашем городе у исполнителей из вашего списка подписок ";

                        for (i = 0; i < concerts.Count(); i++)
                        {
                            foreach (var conc in concerts)
                            {
                                if (conc.concert_city == user_city)
                                {
                                    answer += String.Format(
                                        "Инфо о концерте группы {0} Название: {1} Место: {2} Время: " +
                                        "{3} Ссылка на источник: {4} \n", conc.group_id, conc.concert_title,
                                        conc.concert_city, conc.concert_date, conc.concert_link);
                                    atleastone = true;
                                }
                                
                            }

                        }

                    }

                }
                else
                {
                    answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
                }
            }

            if (atleastone == false)
                answer = "Не найдено ни одного концерта в вашем городе у исполнителей из вашего списка подписок";
            return answer;

        }

        /*
        public  tblConcert FindConcertById(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblConcerts.Where(b => b.group_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }*/

        
    }
    
}