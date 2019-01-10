using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ConcertNotifier
{
    public class UserActions
    {
        private myContext concertDB = new myContext();
        public UserActions()
        {

        }
        public UserActions(myContext context)
        {
            concertDB = context;

        }

        /// <summary>
        /// Добавь в регресс
        /// </summary>
        /// <returns></returns>
        public List<tblUser> SelectAllUsers()
        {

            //try
            //{
                var query = from b in concertDB.tblUsers
                            orderby b.user_id
                            select b;
                
                return query.ToList();

            /*}
            catch (Exception e)
            {

                Console.Write(e);
                return null;
            }*/
           
                
            
        }

        public string InsertUser(int id, string city)
        {
            //myContext concertDB = new myContext();
            string answer = null;
            try
            {
                var user = new tblUser();
                user.user_id = id;
                user.user_city = city;
                concertDB.tblUsers.Add(user);
                concertDB.SaveChanges();
                answer = string.Format("Поздравляем с регистрацией, ваш город {0}", city);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw (new Exception());
            }


            return answer;

        }

        //Добавь в регресс
        public string AddingCityToUser(int id, string city)
        {
            UserActions userActions = new UserActions();
            string answer = null;
            //try
            //{
                if (userActions.ContainsUser(id))
                {

                    userActions.UpdateUser(id, city);
                    answer = String.Format("Ваш город обновлен, теперь вы находитесь в городе {0}", city);

                }
                else
                {
                    userActions.InsertUser(id, city);
                    answer = String.Format("Вы зарегестрированы в городе {0}", city);

                }

            /*}
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }*/
            

            return answer;
        }

        public  bool ContainsUser(int id)
        {
            //myContext concertDB = new myContext();
            var result = concertDB.tblUsers.Where(b => b.user_id == id);

            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        //регресс
        public  tblUser FindUser(int id)
        {
            myContext concertDB = new myContext();
            var result = concertDB.tblUsers.Where(b => b.user_id == id);

            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        public  void UpdateUser(int id, string city)
        {
                var user = new tblUser();
                //myContext concertDB = new myContext();
            
                user = concertDB.tblUsers.Where(b => b.user_id == id).First();
                user.user_city = city;
                concertDB.SaveChanges();
            
        }


        
    }
    
}