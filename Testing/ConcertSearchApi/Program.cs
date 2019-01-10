using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertSearchApi.Api;
using ConcertSearchApi.Models;

namespace ConcertSearchApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var kudago = new KudaGoSearchApi();
            var group = "Die Antwoord";
            var concerts = kudago.GetCityConcertsByGroup(group, "Москва");
            Console.WriteLine(group);
            Console.WriteLine(concerts.Count);
            foreach (var concert in concerts)
            {
                Console.WriteLine(concert.ID.ToString() + " " + concert.Title + " " + concert.Place + " "
                    + concert.Link + " " + concert.Location + " " + concert.Date.ToString());

            }


            Console.ReadKey();
        }
    }
}
