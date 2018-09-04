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
            var envPath = @"C:\Core\Thoughtonomy\Products\Postman\Transunion.postman_environment.json";

            // Read .Json file content from a file location into a variable
            var json = File.ReadAllText(path);
            var envJson = File.ReadAllText(envPath);

            // Parse that JSON into our Classes/Models
            // TODO: Validate Models?
            Parser p = new Parser();

            List<Item> pages = p.ToModel(json);
            Models.Environment environment = p.EnvToModel(envJson);

            // Create StringBuilder object to hold the object Xml code
            StringBuilder sb = new StringBuilder();

            // Prepare BP Object and Page Boilerplate XML
            var objectBoilerplate = "<process name=\"{objectName}\" version=\"1.0\" bpversion=\"5.0.23.0\" narrative=\"{objectDescription}\" type=\"object\" runmode=\"Exclusive\" preferredid=\"{objectGuid}\"><view><camerax>0</camerax><cameray>0</cameray><zoom version=\"2\">1.25</zoom></view><preconditions /><endpoint narrative=\"\" /><subsheet subsheetid=\"{cleanupSubsheetGuid}\" type=\"CleanUp\" published=\"True\"><name>Clean Up</name><view><camerax>0</camerax><cameray>0</cameray><zoom version=\"2\">1.25</zoom></view></subsheet><stage stageid=\"{initialiseStartGuid}\" name=\"Start\" type=\"Start\"><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>-105</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><onsuccess>{initialiseEndGuid}</onsuccess></stage><stage stageid=\"{initialiseEndGuid}\" name=\"End\" type=\"End\"><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>90</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage><stage stageid=\"{initialiseProcessInfoGuid}\" name=\"Stage1\" type=\"ProcessInfo\"><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>-195</displayx><displayy>-105</displayy><displaywidth>150</displaywidth><displayheight>90</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><references><reference>System.Data.dll</reference><reference>System.Xml.dll</reference><reference>System.Drawing.dll</reference></references><imports><import>System</import><import>System.Drawing</import></imports><language>visualbasic</language><globalcode /><code /></stage><stage stageid=\"{cleanupSubsheetInfoGuid}\" name=\"Clean Up\" type=\"SubSheetInfo\"><subsheetid>{cleanupSubsheetGuid}</subsheetid><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>-195</displayx><displayy>-105</displayy><displaywidth>150</displaywidth><displayheight>90</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage><stage stageid=\"{cleanupStartGuid}\" name=\"Start\" type=\"Start\"><subsheetid>{cleanupSubsheetGuid}</subsheetid><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>-105</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><onsuccess>{cleanupEndGuid}</onsuccess></stage><stage stageid=\"{cleanupEndGuid}\" name=\"End\" type=\"End\"><subsheetid>{cleanupSubsheetGuid}</subsheetid><loginhibit /><narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative><displayx>15</displayx><displayy>90</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /></stage>";
            var pageBoilerplate = "<subsheet subsheetid=\"{subsheetGuid}\" type=\"Normal\" published=\"True\"> <name>{actionName}</name> <view> <camerax>0</camerax> <cameray>0</cameray> <zoom version=\"2\">1.25</zoom> </view> </subsheet> <stage stageid=\"{subsheetInfoGuid}\" name=\"{actionName}\" type=\"SubSheetInfo\"> <subsheetid>{subsheetGuid}</subsheetid> <narrative>{actionDescription}</narrative> <displayx>-120</displayx> <displayy>-135</displayy> <displaywidth>150</displaywidth> <displayheight>90</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\"/> </stage> <stage stageid=\"{startGuid}\" name=\"Start\" type=\"Start\"> <subsheetid>{subsheetGuid}</subsheetid> <narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative> <displayx>15</displayx> <displayy>-165</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\"/> <onsuccess>{actionGuid}</onsuccess> <inputs> <input type=\"text\" name=\"Address URL\" stage=\"Address URL\" /> {bodyInputParameter} <input type=\"text\" name=\"Verb\" stage=\"Verb\" /> {headersInputParameter} </inputs> </stage> <stage stageid=\"{actionGuid}\" name=\"Make HTTP Request\" type=\"Action\"> <subsheetid>{subsheetGuid}</subsheetid> <loginhibit/> <narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative> <displayx>15</displayx> <displayy>-45</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\"/> <inputs> <input type=\"text\" name=\"Address URL\" narrative=\"The URL to send the HTTP data to\" expr=\"[Address URL]\"/> <input type=\"text\" name=\"Body\" narrative=\"The body text of the HTTP request\" expr=\"{bodyDataItemReference}\"/> <input type=\"flag\" name=\"Use Proxy\" narrative=\"Set true if you need to use a proxy\" expr=\"\"/> <input type=\"text\" name=\"Content Type\" narrative=\"The content type of the HTTP request.\" expr=\"\"/> <input type=\"text\" name=\"Method\" narrative=\"The method of the HTTP request.\" expr=\"[Verb]\"/> <input type=\"text\" name=\"Proxy URL\" narrative=\"OPTIONAL: The proxy url\" expr=\"\"/> <input type=\"text\" name=\"Proxy Username\" narrative=\"OPTIONAL: The proxy username\" expr=\"\"/> <input type=\"password\" name=\"Proxy Password\" narrative=\"OPTIONAL: The proxy password\" expr=\"\"/> <input type=\"collection\" name=\"Headers\" narrative=\"OPTIONAL: Headers\" expr=\"[Headers]\"/> <input type=\"text\" name=\"Accept\" narrative=\"OPTIONAL: Accept\" expr=\"\"/> <input type=\"text\" name=\"Username\" narrative=\"OPTIONAL: Basic Authentication username\" expr=\"\"/> <input type=\"password\" name=\"Password\" narrative=\"OPTIONAL: Basic Authenction password\" expr=\"\"/> <input type=\"text\" name=\"Certificate ID\" narrative=\"OPTIONAL: The id of the client authentication certificate\" expr=\"\"/> <input type=\"flag\" name=\"Force Pre Authorization\" narrative=\"OPTIONAL: Force the request to send the authorization headers\" expr=\"\"/> </inputs> <outputs> <output type=\"text\" name=\"Result\" narrative=\"The result from the Post query.\" stage=\"Raw Response\"/> </outputs> <resource object=\"Utility - HTTP\" action=\"HTTP Request\"/> <onsuccess>{endGuid}</onsuccess> </stage> <stage stageid=\"{responseDataGuid}\" name=\"Raw Response\" type=\"Data\"> <subsheetid>{subsheetGuid}</subsheetid> <narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative> <displayx>90</displayx> <displayy>-45</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\"/> <datatype>text</datatype> <initialvalue/> <private/> <alwaysinit/> </stage> <stage stageid=\"{headersCollectionGuid}\" name=\"Headers\" type=\"Collection\"> <subsheetid>{subsheetGuid}</subsheetid> <loginhibit/> <narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative> <displayx>225</displayx> <displayy>-165</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\"/> <datatype>collection</datatype> <private/> <alwaysinit/> {headersCollectionData} </stage> <stage stageid=\"{endGuid}\" name=\"End2\" type=\"End\"> <subsheetid>{subsheetGuid}</subsheetid> <loginhibit/> <narrative>Created by Thoughtonomy Connect Wireframing Tool</narrative> <displayx>15</displayx> <displayy>75</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\"/> <outputs> <output type=\"text\" name=\"Raw Response\" stage=\"Raw Response\" /> </outputs> </stage> <stage stageid=\"{addressUrlDataGuid}\" name=\"Address URL\" type=\"Data\"> <subsheetid>{subsheetGuid}</subsheetid> <loginhibit /> <narrative> </narrative> <displayx>105</displayx> <displayy>-165</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /> <datatype>text</datatype> <initialvalue xml:space=\"preserve\">{addressUrlDataValue}</initialvalue> <private /> <alwaysinit /> </stage> {bodyDataItem} <stage stageid=\"{verbDataGuid}\" name=\"Verb\" type=\"Data\"> <subsheetid>{subsheetGuid}</subsheetid> <narrative> </narrative> <displayx>165</displayx> <displayy>-165</displayy> <displaywidth>60</displaywidth> <displayheight>30</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /> <datatype>text</datatype> <initialvalue xml:space=\"preserve\">{verbDataValue}</initialvalue> <private /> <alwaysinit /> </stage> <stage stageid=\"{inputblockGuid}\" name=\"Input Parameters\" type=\"Block\"> <subsheetid>{subsheetGuid}</subsheetid> <loginhibit /> <narrative> </narrative> <displayx>60</displayx> <displayy>-195</displayy> <displaywidth>270</displaywidth> <displayheight>60</displayheight> <font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"800080\" /> </stage>";

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
            objectBoilerplate = objectBoilerplate.Replace("{objectName}", Path.GetFileName(path).Replace(".postman_collection.json", ""));

            // Write object boilerplate to the StringBuilder
            sb.Append(objectBoilerplate);

            // Generate the placeholders for each page
            List<string> pageGuidPlaceholders = new List<string>
            {
                "{subsheetGuid}",
                "{subsheetInfoGuid}",
                "{startGuid}",
                "{endGuid}",
                "{actionGuid}",
                "{responseDataGuid}",
                "{headersCollectionGuid}",
                "{addressUrlDataGuid}",
                "{verbDataGuid}",
                "{inputblockGuid}",
            };

            // Loop through each page/item model that the parser has generated
            foreach (var page in pages)
            {
                // Create a Subsheet or Page XML
                var pageXml = pageBoilerplate;

                // If a body is supplied
                if (!String.IsNullOrEmpty(page.request.body.mode))
                {
                    pageXml = pageXml.Replace("{bodyInputParameter}", "<input type=\"text\" name=\"Body\" stage=\"Body\" />");
                    pageXml = pageXml.Replace("{bodyDataItemReference}", "[Body]");
                    pageXml = pageXml.Replace("{bodyDataItem}", "<stage stageid =\"{bodyDataGuid}\" name=\"Body\" type=\"Data\"><subsheetid>{subsheetGuid}</subsheetid><loginhibit /><narrative></narrative><displayx>285</displayx><displayy>-165</displayy><displaywidth>60</displaywidth><displayheight>30</displayheight><font family=\"Segoe UI\" size=\"10\" style=\"Regular\" color=\"000000\" /><datatype>text</datatype><initialvalue xml:space=\"preserve\">{bodyDataValue}</initialvalue><private /><alwaysinit /></stage>");
                    pageGuidPlaceholders.Add("{bodyDataGuid}");
                    pageXml = pageXml.Replace("{bodyDataValue}", page.request.body.raw);
                }
                else
                {
                    pageXml = pageXml.Replace("{bodyDataItemReference}", "");
                    pageXml = pageXml.Replace("{bodyInputParameter}","");
                    pageXml = pageXml.Replace("{bodyDataItem}", "");
                }

                // Replace the standard placeholders with new Guids
                foreach (var placeholder in pageGuidPlaceholders) pageXml = pageXml.Replace(placeholder, Guid.NewGuid().ToString());

                // Read necessary data from models and generate the action workflow structure(Utility -HTTP and JSON to Collection)
                pageXml = pageXml.Replace("{actionName}", page.name);
                pageXml = pageXml.Replace("{actionDescription}", page.request.description);
                pageXml = pageXml.Replace("{verbDataValue}", page.request.method);
                pageXml = pageXml.Replace("{addressUrlDataValue}", page.request.url.raw.Replace("&","&amp;"));

                // If the request has headers, create and embed the header collection in Page Xml
                if (page.request.header.Count > 0)
                {
                    StringBuilder headerInputs = new StringBuilder();
                    StringBuilder headerSb = new StringBuilder();
                    headerSb.Append("<collectioninfo><singlerow />");

                    foreach (var header in page.request.header)
                    {
                        headerSb.Append($"<field name=\"{ header.key }\" type=\"text\" namespace=\"\" />");
                    }

                    headerSb.Append("</collectioninfo><initialvalue><singlerow /><row>");
                    foreach (var header in page.request.header)
                    {
                        // Check if header value contains reference to environment variable
                        if (header.value.Contains("{{"))
                        {
                            foreach (var value in environment.values)
                            {
                                if (value.key == header.value.Replace("{{","").Replace("}}",""))
                                {
                                    header.value = value.value;
                                    break;
                                }
                            }
                        }

                        headerInputs.Append($"<input type=\"text\" name=\"Header - { header.key }\" stage=\"{ header.key }\" />");
                        headerSb.Append($"<field name=\"{ header.key }\" type=\"text\" value=\"{ header.value }\" />");
                    }
                    headerSb.Append("</row></initialvalue>");

                    pageXml = pageXml.Replace("{headersCollectionData}", headerSb.ToString());
                    pageXml = pageXml.Replace("{headersInputParameter}", headerInputs.ToString());
                }
                
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
