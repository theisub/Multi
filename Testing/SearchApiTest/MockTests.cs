using System;
using NUnit.Framework;
using ConcertSearchApi.Api;
using ConcertSearchApi.Models;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using ConcertNotifier;
using System.Linq;

namespace SearchApiTest
{
    [TestFixture]
    public class MockTests
    {
        
        [Test]
        public void GetAllUsers()    
        {
            var data = new List<tblUser>
            {
                new tblUser {user_id = 41, user_city = "Moscow"},
                new tblUser {user_id = 21, user_city = "Novosibirsk"},
                new tblUser {user_id = 11, user_city = "Saint-Petersburg"}

            }.AsQueryable();

            var MockSet = new Mock<DbSet<tblUser>>();
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<myContext>();
            mockContext.Setup(c => c.tblUsers).Returns(MockSet.Object);

            var service = new UserActions(mockContext.Object);
            var users = service.SelectAllUsers().ToList();
            var temp = users.Count();
            Assert.AreEqual(3, users.Count());
            Assert.AreEqual(11, users[0].user_id);
            Assert.AreEqual(21, users[1].user_id);
            Assert.AreEqual(41, users[2].user_id);
        }

        [Test]
        public void InsertUser()
        {

            int user_id = 99;
            string user_city = "Moscow";

            var MockSet = new Mock<DbSet<tblUser>>();

            var mockContext = new Mock<myContext>();
            mockContext.Setup(c => c.tblUsers).Returns(MockSet.Object);

            var service = new UserActions(mockContext.Object);
            service.InsertUser(user_id, user_city);

            MockSet.Verify(m => m.Add(It.IsAny<tblUser>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void FindUserTest()
        {

            int find_id = 5;
            var data = new List<tblUser>
            {
                new tblUser {user_id = 41, user_city = "Moscow"},
                new tblUser {user_id = 21, user_city = "Novosibirsk"},
                new tblUser {user_id = 11, user_city = "Saint-Petersburg"}

            }.AsQueryable();

            var MockSet = new Mock<DbSet<tblUser>>();
            var mockContext = new Mock<myContext>();
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(c => c.tblUsers).Returns(MockSet.Object);

            var service = new UserActions(mockContext.Object);
           // service.InsertUser(user_id, "Moscow");
            //var users = service.SelectAllUsers();
            MockSet.Setup(m => m.Find(It.IsAny<object[]>()))
            .Returns<object[]>(ids => data.FirstOrDefault(d => d.user_id== (int)ids[0]));
            Assert.AreEqual(service.FindUser(find_id),null);
            //MockSet.Verify(m => m.Add(It.IsAny<tblUser>()), Times.Exactly(2));
            //mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }

        [Test]
        public void TestDeletion()
        {
            var mockContext = new Mock<myContext>();
            var service = new GroupActions(mockContext.Object);

            var data = new List<tblGroup>
            {
                new tblGroup() {group_id=111, group_name="number1"},
                new tblGroup() {group_id=222, group_name="number2"},
                new tblGroup() {group_id=333, group_name="number3"},

            };

            int idToDelete = 111;

            mockContext
                .Setup(s => s.tblGroups.Find(idToDelete))
                .Returns(data.Single(s => s.group_id == idToDelete));

            var MockSet = new Mock<DbSet<tblGroup>>();
            MockSet.Setup(c => c.Remove(It.IsAny<tblGroup>())).Callback<tblGroup>((entity) => data.Remove(entity));

            MockSet.As<IQueryable<tblGroup>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            MockSet.As<IQueryable<tblGroup>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            MockSet.As<IQueryable<tblGroup>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            MockSet.As<IQueryable<tblGroup>>().Setup(m => m.GetEnumerator()).Returns(data.AsQueryable().GetEnumerator());

            mockContext.Setup(c => c.tblGroups).Returns(MockSet.Object);


            service.DeleteGroup(idToDelete);
            Assert.AreEqual(2, data.Count());
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpdateUserTest()
        {

            int change_id = 21;
            string new_city = "Kazan";
            var data = new List<tblUser>
            {
                new tblUser {user_id = 41, user_city = "Moscow"},
                new tblUser {user_id = 21, user_city = "Novosibirsk"},
                new tblUser {user_id = 11, user_city = "Saint-Petersburg"}

            }.AsQueryable();

            var MockSet = new Mock<DbSet<tblUser>>();
            var mockContext = new Mock<myContext>();
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<tblUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(c => c.tblUsers).Returns(MockSet.Object);

            var service = new UserActions(mockContext.Object);
            // service.InsertUser(user_id, "Moscow");
            service.UpdateUser(change_id, new_city);
            var users = service.SelectAllUsers();
            Assert.AreEqual(new_city, users[1].user_city);
            //MockSet.Verify(m => m.Add(It.IsAny<tblUser>()), Times.Exactly(2));
            //mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }


    }
}