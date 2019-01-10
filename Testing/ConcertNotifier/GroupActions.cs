using System;
using System.Data.Entity;
using System.Linq;

namespace ConcertNotifier
{
    public class GroupActions
    {
        private myContext concertDB = new myContext();
        public  DbSet<tblGroup> SelectAllGroups()
        {
            //myContext concertDB = new myContext();
            return concertDB.tblGroups;
        }
        public GroupActions()
        {

        }
        public GroupActions(myContext context)
        {
            concertDB = context;

        }
        public void DeleteAll()
        {
            var subs = concertDB.tblSubscriptions;

            if (subs != null)
            {
                foreach (var item in subs)
                {
                    concertDB.tblSubscriptions.Remove(item);
                }
                concertDB.SaveChanges();
            }

            var concs = concertDB.tblConcerts;
            foreach (var item in concs)
            {
                concertDB.tblConcerts.Remove(item);
            }
            concertDB.SaveChanges();

            var groups = concertDB.tblGroups;
            foreach (var item in groups)
            {
                concertDB.tblGroups.Remove(item);
            }
            concertDB.SaveChanges();

            var users = concertDB.tblUsers;
            foreach (var item in users)
            {
                concertDB.tblUsers.Remove(item);
            }
            concertDB.SaveChanges();




        }

        //регресс
        public void DeleteGroup(int id)
        {
            try
            {
                var result = concertDB.tblGroups.Where(b => b.group_id == id).First();
                concertDB.tblGroups.Remove(result);
                concertDB.SaveChanges();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw e;
            }
        }
        public void InsertGroup(int id, string name)
        {
            //myContext concertDB = new myContext();
            var group = new tblGroup();

            group.group_id = id;
            group.group_name = name;
            concertDB.tblGroups.Add(group);
            concertDB.SaveChanges();
        }

        public void InsertGroupObject(tblGroup Group)
        {

            concertDB.tblGroups.Add(Group);
            concertDB.SaveChanges();
        }

       

       


        public  bool ContainsGroup(string name)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups.Where(b => b.group_name == name);

            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        public tblGroup FindGroupByName(string name)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups.Where(b => b.group_name == name);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        public  tblGroup FindGroupById(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblGroups.Where(b => b.group_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        /* public  tblConcert FindGroupIdByConcert(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblConcerts.Where(b => b.concert_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;


        }*/
        /*
        public  void UpdateGroup(int id, string name)
        {

            myContext concertDB = new myContext();
            tblGroup group = concertDB.tblGroups.Where(b => b.group_id == id).First();
            group.group_name = name;
            concertDB.SaveChanges();
        }*/


        
    }
    
}