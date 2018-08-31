﻿using Newtonsoft.Json.Linq;
using PostmanToBluePrism.Models;
using System.Collections.Generic;

namespace PostmanToBluePrism.Helpers
{
    class Parser
    {

        public List<Item> ToModel(string json)
        {
            List<Item> pages = new List<Item>();

            JObject j = JObject.Parse(json);

            var items = j["item"];

            foreach (var item in items)
            {
                Item i = new Item();
                i.name = (string)item["name"];

                var request = item["request"];
                Request r = new Request();
                r.method = (string)request["method"];

                var headers = request["header"];
                List<Header> headerList = new List<Header>();
                foreach (var header in headers)
                {
                    Header h = new Header();
                    h.key = (string)header["key"];
                    h.value = (string)header["value"];
                    headerList.Add(h);
                }
                r.header = headerList;

                var body = request["body"];
                Body b = new Body();
                b.mode = (string)body["mode"];
                b.raw = (string)body["raw"];
                r.body = b;

                var url = request["url"];
                Url u = new Url();
                u.raw = (string)url["raw"];
                u.protocol = (string)url["protocol"];
                r.url = u;

                i.request = r;
                pages.Add(i);
            }
            return pages;
        }
    }
}
