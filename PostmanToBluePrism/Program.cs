using Newtonsoft.Json.Linq;
using PostmanToBluePrism.Models;
using PostmanToBluePrism.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace PostmanToBluePrism
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read .Json file content from a file location into a variable
            var json = File.ReadAllText(@"C:\Core\Thoughtonomy\Products\Postman\Transunion.postman_collection.json");

            // Parse that JSON into our Classes/Models

            // TODO: Validate Models?
            Parser p = new Parser();
            List<Item> pages = p.ToModel(json);

            // TODO: Prepare BP Object Boilerplate XML
            // TODO: Cleanse BP Object Boilerplate with randomly generated GUIDs

            foreach (var page in pages)
            {
                // Foreach item that has been pased from JSON, Create a Subsheet or Page XML

                // Read necessary data from models and generate the action workflow structure(Utility -HTTP and JSON to Collection)

                // Write Xml in a directory for the user to import

                // Could import?
            }
        }
    }
}
