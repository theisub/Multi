using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertSearchApi.Models
{
        public abstract class Event
        {
            public int ID { get; set; }
            public string Place { get; set; }
            public string Location { get; set; }
            public DateTime Date { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Link { get; set; }
        }
}
