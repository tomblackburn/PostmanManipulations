using System;
using System.Collections.Generic;
using System.Text;

namespace PostmanToBluePrism.Models
{
    class Url
    {
        public string raw { get; set; }
        public string protocol { get; set; }
        public List<string> host { get; set; }
        public List<string> path { get; set; }
        public List<Query> query { get; set; }
    }
}
