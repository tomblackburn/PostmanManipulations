using Newtonsoft.Json.Linq;
using PostmanToBluePrism.Models;
using PostmanToBluePrism.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PostmanToBluePrism
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Core\Thoughtonomy\Products\Postman\Transunion.postman_collection.json";

            // Read .Json file content from a file location into a variable
            var json = File.ReadAllText(path);

            // Parse that JSON into our Classes/Models
            // TODO: Validate Models?
            Parser p = new Parser();
            List<Item> pages = p.ToModel(json);

            // Create StringBuilder object to hold the object Xml code
            StringBuilder sb = new StringBuilder();

            // Prepare BP Object and Page Boilerplate XML
            var objectBoilerplate = "<process name=\"{objectName}\" version=\"1.0\" bpversion=\"5.0.23.0\" narrative=\"{objectDescription}\" type=\"object\" runmode=\"Exclusive\" preferredid=\"{objectGuid}\"><view><camerax>0</camerax><cameray>0</cameray><zoom version=\"2\">1.25</zoom></view><preconditions /><endpoint narrative=\"\" /><subsheet subsheetid=\"{cleanupSubsheetGuid}\" type=\"CleanUp\" published=\"True\"><name>Clean Up</name><view><camerax>0</camerax><cameray>0</cameray><zoom version=\"2\">1.25</zoom></view></subsheet><stage stageid=\"{initialiseStartGuid}\" name=\"Start\" type=\"Start\"><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>-105</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><onsuccess>{initialiseEndGuid}</onsuccess></stage><stage stageid=\"{initialiseEndGuid}\" name=\"End\" type=\"End\"><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>90</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage><stage stageid=\"{initialiseProcessInfoGuid}\" name=\"Stage1\" type=\"ProcessInfo\"><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>-195</displayx><displayy>-105</displayy><displaywidth>150</displaywidth><displayheight>90</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><references><reference>System.Data.dll</reference><reference>System.Xml.dll</reference><reference>System.Drawing.dll</reference></references><imports><import>System</import><import>System.Drawing</import></imports><language>visualbasic</language><globalcode /><code /></stage><stage stageid=\"{cleanupSubsheetInfoGuid}\" name=\"Clean Up\" type=\"SubSheetInfo\"><subsheetid>{cleanupSubsheetGuid}</subsheetid><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>-195</displayx><displayy>-105</displayy><displaywidth>150</displaywidth><displayheight>90</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage><stage stageid=\"{cleanupStartGuid}\" name=\"Start\" type=\"Start\"><subsheetid>{cleanupSubsheetGuid}</subsheetid><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>-105</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><onsuccess>{cleanupEndGuid}</onsuccess></stage><stage stageid=\"{cleanupEndGuid}\" name=\"End\" type=\"End\"><subsheetid>{cleanupSubsheetGuid}</subsheetid><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>90</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage>";
            var pageBoilerplate = "<subsheet subsheetid=\"{subsheetGuid}\" type=\"Normal\" published=\"True\"><name>{actionName}</name><view><camerax>0</camerax><cameray>0</cameray><zoom version=\"2\">1.25</zoom></view></subsheet><stage stageid=\"{subsheetInfoGuid}\" name=\"{actionName}\" type=\"SubSheetInfo\"><subsheetid>{subsheetGuid}</subsheetid><narrative>{actionDescription}</narrative><displayx>-120</displayx><displayy>-135</displayy><displaywidth>150</displaywidth><displayheight>90</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage><stage stageid=\"{startGuid}\" name=\"Start\" type=\"Start\"><subsheetid>{subsheetGuid}</subsheetid><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>-165</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><onsuccess>{endGuid}</onsuccess></stage><stage stageid=\"{endGuid}\" name=\"End2\" type=\"End\"><subsheetid>{subsheetGuid}</subsheetid><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>75</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage>";

            // Cleanse BP Object Boilerplate with randomly generated GUIDs
            List<string> boilerplateGuidPlaceholders = new List<string>
            {
                "{objectGuid}",
                "{cleanupSubsheetGuid}",
                "{initialiseStartGuid}",
                "{initialiseEndGuid}",
                "{initialiseProcessInfoGuid}",
                "{cleanupSubsheetInfoGuid}",
                "{cleanupStartGuid}",
                "{cleanupEndGuid}"
            };
            foreach (var placeholder in boilerplateGuidPlaceholders) objectBoilerplate = objectBoilerplate.Replace(placeholder, Guid.NewGuid().ToString());
            objectBoilerplate = objectBoilerplate.Replace("{objectName}", Path.GetFileName(path).Replace(".postman_collection.json",""));

            // Write object boilerplate to the StringBuilder
            sb.Append(objectBoilerplate);

            // Generate the placeholders for each page
            List<string> pageGuidPlaceholders = new List<string>
            {
                "{subsheetGuid}",
                "{subsheetInfoGuid}",
                "{startGuid}",
                "{endGuid}"
            };

            // Loop through each page/item model that the parser has generated
            foreach (var page in pages)
            {
                // Create a Subsheet or Page XML
                var pageXml = pageBoilerplate;

                // Replace the standard placeholders with new Guids
                foreach (var placeholder in pageGuidPlaceholders) pageXml = pageXml.Replace(placeholder, Guid.NewGuid().ToString());

                // Read necessary data from models and generate the action workflow structure(Utility -HTTP and JSON to Collection)
                pageXml = pageXml.Replace("{actionName}", page.name);
                pageXml = pageXml.Replace("{actionDescription}", page.request.description);

                // Write Xml in a directory for the user to import

                // Could import?
                sb.Append(pageXml);
            }
            sb.Append("</process>");

            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }
    }
}
