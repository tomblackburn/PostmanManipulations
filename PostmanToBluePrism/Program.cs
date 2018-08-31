using PostmanToBluePrism.Models;
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

            List<Item> pages = new List<Item>();

            // Parse that JSON into our Classes/Models

            // Validate Models?

            // Prepare BP Object Boilerplate XML

            // Cleabnse BP Object Boilerplate with randomly generated GUIDs

            // Foreach item that has been pased from JSON, Create a Subsheet or Page XML

            // Read necessary data from models and generate the action workflow structure (Utility - HTTP and JSON to Collection)

            // Write Xml in a directory for the user to import

            // Could import?

        }
    }
}
