using System;
using System.Collections.Generic;
using System.Text;

namespace PostmanToBluePrism.Models
{
    class Request
    {
        public string method { get; set; }
        public List<Header> header { get; set; }
        public Body body { get; set; }
        public Url url { get; set; }
        public string description { get; set; }
    }
}
