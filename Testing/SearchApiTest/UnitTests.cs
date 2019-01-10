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
    public class  UnitTests 
    {
        private KudaGoSearchApi kudago;

        public UnitTests()
        {
            kudago = new KudaGoSearchApi();
        }

        [Test]
        public void GetAllConcertsByGroupTestNull()
        {
            string groupName = null;
            Assert.Null(kudago.GetAllConcertsByGroup(groupName));
        }

        [Test]
        public void GetAllConcertsByGroupTestEmpty()
        {
            var groupName = String.Empty;
            Assert.Null(kudago.GetAllConcertsByGroup(groupName));
        }
        [Test]
        public void ContainTest()
        {

            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroup(104,"group1");
            groupActions.InsertGroup(105, "group2");


            Assert.IsTrue(groupActions.ContainsGroup("group1"));


        }

        [Test]
        public void ContainTest_False()
        {
            
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroup(104, "group1");
            groupActions.InsertGroup(105, "group2");


            Assert.IsFalse(groupActions.ContainsGroup("nogroup"));                                          

        }

        [Test]
        public void FindGroupByIdTest()
        {
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroup(999, "group1");

            bool IsFound=false;
            if (groupActions.FindGroupById(999) != null)
            {
                IsFound = true;                                                                              
            }
            Assert.IsTrue(IsFound);
        }

        [Test]
        public void FindGroupByNameTest()
        {
            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroup(999, "group1");

            bool IsFound = false;
            if (groupActions.FindGroupByName("group1") != null)
            {
                IsFound = true;
            }
            Assert.IsTrue(IsFound);
        }


        [Test]
        public void ContainGroupTest()
        {

            GroupActions groupActions = new GroupActions();
            groupActions.DeleteAll();
            groupActions.InsertGroup(888, "grouptofind");

            bool IsFound = false;
            if (groupActions.ContainsGroup("grouptofind") != null)
            {
                IsFound = true;                                                                             
            }
            Assert.IsTrue(IsFound);
        }






    }
}