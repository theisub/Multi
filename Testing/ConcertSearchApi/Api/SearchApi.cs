using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertSearchApi.Models;

namespace ConcertSearchApi.Api
{
    public abstract class SearchApi : ConcertSearch
    {
        protected abstract string initUrl { get; }
        protected abstract string apiVersion { get; }
        protected abstract dynamic GetJsonByLink(string link);
        public abstract List<Concert> GetAllConcertsByGroup(string group);
        public abstract List<Concert> GetCityConcertsByGroup(string group, string city);
        public abstract List<Concert> GetAllConcertsByGenre(string genre);
        public abstract List<Concert> GetCityConcertsByGenre(string genre, string city);
    }
}
