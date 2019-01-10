using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertSearchApi.Models
{
        public class Concert : Event
        {
            public string Genre { get; set; }
        }
}
