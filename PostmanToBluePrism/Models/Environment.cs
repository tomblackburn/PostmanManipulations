using System;
using System.Collections.Generic;

namespace PostmanToBluePrism.Models
{
    public class Environment
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Value> values { get; set; }
        public long timestamp { get; set; }
        public string _postman_variable_scope { get; set; }
        public DateTime _postman_exported_at { get; set; }
        public string _postman_exported_using { get; set; }
    }
}
