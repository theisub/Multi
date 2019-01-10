using System;
using NUnit.Framework;
using ConcertSearchApi.Api;
using ConcertSearchApi.Models;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using ConcertNotifier;
using System.Linq;
using System.Diagnostics;

namespace SearchApiTest
{
    
        [TestFixture]
        public class IntegrationTests
        {
            [Test]
            
            public void AddCityNewUser()
            {
                CityActions cityActions = new CityActions();
                UserActions userActions = new UserActions();

                int user = 777;
                string city = CityActions.GetCity("RandomTown");

                if (userActions.ContainsUser(user))
                    userActions.UpdateUser(user, city);
                else
                    userActions.InsertUser(user, city);


            }

            [Test]
            public void RemoveExistingSubscription()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                groupActions.DeleteAll();

                int group_id = 111;
                string group_name = "MyGroup";
                int user_id = 777;
                string user_city = "Rand";
                userActions.InsertUser(user_id, user_city);
                var dbuser = userActions.FindUser(user_id);
                //IQueryable<tblSubscription> result = null;
                string result = null;

                groupActions.InsertGroup(group_id, group_name); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id); //Добавление подписки, чтобы проверить её удаление

                if (dbuser != null)
                {
                    result = subscriptionActions.DeleteSubscription(user_id, group_name);


                }

                Assert.AreNotEqual(result, null);
            }

            [Test]
            public void RemoveEmptySubscription()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                groupActions.DeleteAll();
                int group_id = 111;
                int wrong_id = 1111;
                string group_name = "NoGroup";
                int user_id = 777;
                string user_city = "Rand";

                userActions.InsertUser(user_id, user_city);
                var dbuser = userActions.FindUser(user_id);


                IQueryable<tblSubscription> result = null;

                groupActions.InsertGroup(group_id, group_name); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id); //Добавление подписки, чтобы проверить её удаление

                if (dbuser != null)
                {
                    result = subscriptionActions.DeleteSubscription(user_id, wrong_id);
                }

                Assert.AreEqual(result, null);
            }

            [Test]
            public void ShowArtistList()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                ConcertActions concertActions = new ConcertActions();
                groupActions.DeleteAll();

                int found_id = 0;

                int user = 999;
                string city = "RandomTown";
                int group_id = 222;
                string group_name = "RandGroup";
                subscriptionActions.DeleteAllSubscriptions(user);

                userActions.InsertUser(user, city);
                groupActions.InsertGroup(group_id, group_name);
                var conc = new Concert();

                concertActions.InsertConcerts(conc, group_id);

                subscriptionActions.InsertSubscription(user, group_id);
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
                                found_id = find.group_id;
                            }
                        }
                    }

                }
                Assert.AreEqual(found_id, group_id);


            }

            [Test]
            public void CheckClearSubscriptionsWithAdditionalUser()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                groupActions.DeleteAll();

                int group_id = 111;
                string group_name = "MyGroup";
                int user_id = 777;
                int additional_user_id = 787;
                string user_city = "Rand";
                int increment = 0;

                userActions.InsertUser(user_id, user_city);
                userActions.InsertUser(additional_user_id, user_city);

                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;
                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;
                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;

                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(additional_user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;
                int AfterAdd = subscriptionActions.SelectAllSubscriptions().Count();

                Assert.AreEqual(AfterAdd, 4);

                subscriptionActions.DeleteAllSubscriptions(user_id);

                int AfterDelete = subscriptionActions.SelectAllSubscriptions().Count();

                Assert.AreEqual(AfterDelete, 1);

            }

            [Test]
            public void CheckClearSubscriptions()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                groupActions.DeleteAll();

                int group_id = 111;
                string group_name = "MyGroup";
                int user_id = 777;
                string user_city = "Rand";
                int increment = 0;

                userActions.InsertUser(user_id, user_city);


                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;
                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;
                groupActions.InsertGroup(group_id + increment, group_name + increment); // Добавление группы
                subscriptionActions.InsertSubscription(user_id, group_id + increment); //Добавление подписки, чтобы проверить её удаление
                increment++;


                int AfterAdd = subscriptionActions.SelectAllSubscriptions().Count();

                Assert.AreEqual(AfterAdd, 3);

                subscriptionActions.DeleteAllSubscriptions(user_id);

                int AfterDelete = subscriptionActions.SelectAllSubscriptions().Count();

                Assert.AreEqual(AfterDelete, 0);

            }

            [Test]
            public void AddConcertOfNewGroup()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                ConcertActions concertActions = new ConcertActions();
                groupActions.DeleteAll();
                Concert EmptyConcert = new Concert { Date = DateTime.Now, Genre = "any", ID = 111, Link = "no url", Location = "testloc", Place = "testplac", Title = "title" };

                int subscription_counter = 0;
                tblGroup found_group = null;

                string group_name = "RandGroup";
                var group_id = 888;
                int user_id = 777;
                string user_city = "RandTown";
                userActions.InsertUser(user_id, user_city);

                //groupActions.InsertGroup(group_id,group_name);

                if (!groupActions.ContainsGroup(group_name))
                {
                    var groups = groupActions.SelectAllGroups();
                    groupActions.InsertGroup(group_id, group_name);

                    concertActions.InsertConcert(EmptyConcert, group_id);
                    subscriptionActions.InsertSubscription(user_id, group_id);
                }
               
                subscription_counter = subscriptionActions.SelectSubscriptions(user_id).Count();
                found_group = groupActions.FindGroupById(group_id);

                Assert.AreEqual(1, subscription_counter);
                Assert.AreEqual(group_id, found_group.group_id);
            }


            [Test]
            public void AddConcertOfExistingGroup()
            {
                UserActions userActions = new UserActions();
                SubscriptionActions subscriptionActions = new SubscriptionActions();
                GroupActions groupActions = new GroupActions();
                ConcertActions concertActions = new ConcertActions();
                groupActions.DeleteAll();
                Concert EmptyConcert = new Concert { Date = DateTime.Now, Genre = "any", ID = 111, Link = "no url", Location = "testloc", Place = "testplac", Title = "title" };

                int subscription_counter = 0;
                tblGroup found_group = null;

                string group_name = "RandGroup";
                var group_id = 888;
                int user_id = 777;
                string user_city = "RandTown";
                userActions.InsertUser(user_id, user_city);

                groupActions.InsertGroup(group_id, group_name);

                var dbuser = userActions.FindUser(user_id);
                if (dbuser != null)
                {

                    if (!groupActions.ContainsGroup(group_name))
                    {
                        var groups = groupActions.SelectAllGroups();
                        groupActions.InsertGroup(group_id, group_name);

                        concertActions.InsertConcert(EmptyConcert, group_id);
                        subscriptionActions.InsertSubscription(user_id, group_id);

                    }
                    else
                    {
                        var CurrentGroup = groupActions.FindGroupByName(group_name);
                        if (!subscriptionActions.ContainsSubscription(group_name, user_id))
                        {
                            subscriptionActions.InsertSubscription(user_id, CurrentGroup.group_id);
                        }
                    }
                }

                subscription_counter = subscriptionActions.SelectSubscriptions(user_id).Count();
                found_group = groupActions.FindGroupById(group_id);
                Assert.AreEqual(1, subscription_counter);
                Assert.AreEqual(group_id, found_group.group_id);
            }
        }

}
