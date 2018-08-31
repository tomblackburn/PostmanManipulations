using System;
using System.Collections.Generic;
using System.Text;

namespace PostmanToBluePrism.Models
{
    class Item
    {
        public string name { get; set; }
        public Request request { get; set; }
        public List<object> response { get; set; }
        public List<Event> @event { get; set; }
    }
}
