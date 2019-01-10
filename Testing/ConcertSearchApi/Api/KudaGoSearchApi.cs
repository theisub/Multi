using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConcertSearchApi.Models;
using Newtonsoft.Json;

namespace ConcertSearchApi.Api
{
    public class KudaGoSearchApi : SearchApi
    {
        protected override string initUrl { get { return @"https://kudago.com/public-api"; } }
        protected override string apiVersion { get { return @"v1.4"; } }
        public List<string> GetGenres()
        {

            List<string> facts = new List<string>();
            return facts;
        }
        protected override dynamic GetJsonByLink(string link)
        {
            System.Net.WebRequest reqGET = WebRequest.Create(link);
            WebResponse resp = reqGET.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            return JsonConvert.DeserializeObject(sr.ReadToEnd());
        }

        public override List<Concert> GetAllConcertsByGroup(string group)
        {
            List<Concert> concerts = null;
            try
            {
                dynamic stuff = GetJsonByLink(String.Format("{0}/{1}/search/?q={2}&ctype=event", initUrl, apiVersion, group));
                if (stuff != null)
                {
                    concerts = new List<Concert>();

                    foreach (var even in stuff.results)
                    {
                        var id = Convert.ToInt32(even.id);
                        var concert = GetJsonByLink(String.Format("{0}/{1}/events/{2}/" +
                        "?categories=concert&fields=id,title,site_url,place,location,dates,categories&expand=place,location",
                        initUrl, apiVersion, id));

                        string title = concert.title;
                        title = title.ToLower();
                        if (concert != null && (title.Equals(group.ToLower()) || title.Equals("концерт " + group.ToLower())))
                        {

                            var item = new Concert();
                            item.ID = id;
                            item.Title = concert.title;
                            item.Link = concert.site_url;
                            item.Place = concert.place.title;
                            item.Location = concert.location.name;
                            //concert.tags, actual since
                            item.Date =
                                (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(Convert.ToDouble(concert.dates[0].start));

                            concerts.Add(item);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return concerts;
        }
        public override List<Concert> GetCityConcertsByGroup(string group, string city)
        {
            var allConcerts = GetAllConcertsByGroup(group);

            if (allConcerts != null)
            {
                return allConcerts.Where(concert => concert.Location.Equals(city)).ToList();
            }

            return null;
        }
        public override List<Concert> GetAllConcertsByGenre(string genre)
        {
            return null;
        }
        public override List<Concert> GetCityConcertsByGenre(string genre, string city)
        {
            return null;
        }
    }
}
