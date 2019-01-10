using ConcertNotifier;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SearchApiTest
{
    [TestFixture]
    public class FunctionalTests
    {
        private readonly tblGroup group1 = new tblGroup{group_id = 1,group_name = "group1"};
        private readonly tblGroup group2 = new tblGroup { group_id = 2, group_name = "group2" };
        private readonly tblGroup group3 = new tblGroup { group_id = 3, group_name = "group3" };
        private readonly tblConcert concert1 = new tblConcert { concert_date = DateTime.Now, group_id = 1, concert_id = 1, concert_link = "no url", concert_city = "loc1", concert_place = "place1", concert_title = "concert1" };
        private readonly tblConcert concert2 = new tblConcert { concert_date = DateTime.Now, group_id = 2, concert_id = 2, concert_link = "no url", concert_city = "loc2", concert_place = "place2", concert_title = "concert2" };


        

        [Test]
        public void UpdateCityOfUser()
        {
            UserActions userActions = new UserActions();
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            int user_id = 1;

            string old_city = "Moscow";
            string updated_city = "NewTown";
            string answer = userActions.InsertUser(user_id, old_city);

            answer = userActions.AddingCityToUser(user_id, updated_city);
            string expected_answer = answer = String.Format("Ваш город обновлен, теперь вы находитесь в городе {0}", updated_city);

            Assert.AreEqual(expected_answer, answer);
        }

        [Test]
        public void InsertUser()
        {
            UserActions userActions = new UserActions();
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            int user_id = 1;
            string user_city = "Moscow";
            string answer = userActions.InsertUser(user_id,user_city);
            string expected_answer = answer = string.Format("Поздравляем с регистрацией, ваш город {0}", user_city);

            Assert.AreEqual(expected_answer, answer);
        }

        [Test]
        public void WrongInsertUser()
        {
            UserActions userActions = new UserActions();
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            int user_id = 1;
            string user_city = "Moscow";
            string answer = userActions.InsertUser(user_id, user_city);

            Assert.Throws<Exception>(() => userActions.InsertUser(user_id, user_city));

        }




        [Test]
        public void RemoveExistingGroup()
        {
            GroupActions groupActions = new  GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);
            groupActions.InsertGroupObject(group3);


            groupActions.DeleteGroup(group3.group_id);
            DbSet<tblGroup> temp = groupActions.SelectAllGroups();
            var num = temp.FirstOrDefault(a => a.group_id == group3.group_id);

            Assert.AreEqual(null,num);
        }

        [Test]
        public void RemoveNonexistGroup()
        {
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            Assert.Throws<InvalidOperationException>(() => groupActions.DeleteGroup(group3.group_id));
        }

        [Test]
        public void ShowList()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "1) group1\n";

            userActions.InsertUser(user_id, user_city);
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            string answer =subscriptionActions.ShowSubList(user_id);
            Assert.AreEqual(expected_answer,answer);

        }


        [Test]
        public void ShowEmptyList()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "Добавьте группу с помощью команды \"/add <Группа>\".";
            userActions.InsertUser(user_id, user_city);
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            string answer = subscriptionActions.ShowSubList(user_id);

            Assert.AreEqual(expected_answer, answer);


        }

        [Test]
        public void ShowListOfUnauth()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "Пользователь с таким id не найден. Добавьте город с помощью \"/city";
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            string answer = subscriptionActions.ShowSubList(user_id);

            Assert.AreEqual(expected_answer, answer);


        }

        [Test]
        public void ClearList()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "Ваш список подписок очищен";

            userActions.InsertUser(user_id, user_city);
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            string answer = subscriptionActions.ShowSubList(user_id);

            answer = subscriptionActions.DeleteAllSubscriptions(user_id);

            Assert.AreEqual(expected_answer, answer);

        }

        [Test]
        public void RemoveFromList()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "1) group1\n";

            userActions.InsertUser(user_id, user_city);
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            subscriptionActions.InsertSubscription(user_id, group2.group_id);

            subscriptionActions.DeleteSubscription(user_id, group2.group_name);
            string answer = subscriptionActions.ShowSubList(user_id);


            Assert.AreEqual(expected_answer, answer);
        }

        [Test]
        public void RemoveFromMiddleList()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "1) group1\n" + "2) group3\n";

            userActions.InsertUser(user_id, user_city);
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);
            groupActions.InsertGroupObject(group3);
            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            subscriptionActions.InsertSubscription(user_id, group2.group_id);
            subscriptionActions.InsertSubscription(user_id, group3.group_id);

            subscriptionActions.DeleteSubscription(user_id, group2.group_name);
            string answer = subscriptionActions.ShowSubList(user_id);


            Assert.AreEqual(expected_answer, answer);
        }

        [Test]
        public void RemoveNonExistFromList()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "У вас нет подписки с таким именем";

            userActions.InsertUser(user_id, user_city);
            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);
            groupActions.InsertGroupObject(group3);
            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            subscriptionActions.InsertSubscription(user_id, group2.group_id);
            subscriptionActions.InsertSubscription(user_id, group3.group_id);

            string answer = subscriptionActions.DeleteSubscription(user_id, "random name");


            Assert.AreEqual(expected_answer, answer);
        }





        [Test]
        public void ShowEmptyConcerts()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            ConcertActions concertActions = new ConcertActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "Не найдено ни одного концерта в вашем городе у исполнителей из вашего списка подписок ";
            userActions.InsertUser(user_id, user_city);

            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);

            string answer = concertActions.ShowConcList(user_id);

            Assert.AreEqual(expected_answer, answer);
        }

        [Test]
        public void ShowConcert()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            ConcertActions concertActions = new ConcertActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = String.Format("Инфо о концерте группы {0} Название: {1} Место: {2} Время: " +
                                                   "{3} Ссылка на источник: {4} \n", concert1.group_id, concert1.concert_title, concert1.concert_city, concert1.concert_date, concert1.concert_link);
            userActions.InsertUser(user_id, user_city);

            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);
            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            concertActions.InsertConc(concert1, group1.group_id);


            string answer = concertActions.ShowConcList(user_id);

            Assert.AreEqual(expected_answer, answer);
        }


        [Test]
        public void ShowConcertInTown()
        {
            SubscriptionActions subscriptionActions = new SubscriptionActions();
            GroupActions groupActions = new GroupActions();
            UserActions userActions = new UserActions();
            ConcertActions concertActions = new ConcertActions();
            groupActions.DeleteAll();

            int user_id = 1;
            string user_city = "Moscow";
            string expected_answer = "Не найдено ни одного концерта в вашем городе у исполнителей из вашего списка подписок";
            userActions.InsertUser(user_id, user_city);

            groupActions.InsertGroupObject(group1);
            groupActions.InsertGroupObject(group2);
            subscriptionActions.InsertSubscription(user_id, group1.group_id);
            concertActions.InsertConc(concert1, group1.group_id);


            string answer = concertActions.ShowConcList(user_id,user_city);

            Assert.AreEqual(expected_answer, answer);
        }

    }








}
