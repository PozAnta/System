using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyWPF
{
    class XMLItems
    {
        public string path;
        public string id_test;
        public string setupId;
		// new branch test

        public XMLItems(string path, string id_test, string setupId)
        {
            this.path = path;
            this.id_test = id_test;
            this.setupId = setupId;
        }

        public void UpdateXML(string[] results)
        {
            var xdoc = XDocument.Load(path);
            var node = xdoc.Descendants("Test");

            var atr = node.Attributes().ToList();
            var field_of_test = xdoc.Descendants("Field" + id_test);

            //var elem = node.ElementAt(0).Value;

            var elem1 = field_of_test.ElementAt(0).Value; // Element result for test id #1

            if (results.Count() == field_of_test.Count())  // Check if lenght of results compare with lenght of xml Field
            {
                for (int i = 0; i < results.Count(); i++)
                {
                    field_of_test.ElementAt(i).Value = results[i];
                }
            }
            else
            {
                Console.WriteLine("The number of steps not equal to number results!");
            }

            xdoc.Save(path);

        }

        public void InsertXML(string[] step_arr, string[] expect_arr, string[] test_result)
        {
            XDocument xdoc = XDocument.Load(path);              // Load from path xml exist
            XElement root = new XElement("Test");               // Create root catalog "Tests"
            XAttribute atr = new XAttribute("TestId", id_test); // Write atributes to catalog "Test"
            root.Add(atr);                                      // Add atribute to catalog "Test"

            int i = 0;
            foreach (string key in step_arr)
            {
                root.Add(new XElement("Field" + id_test, new XAttribute("Name", key), new XAttribute("Expect", expect_arr[i]), test_result[i])); //Create new catalog "Field" into catalog "Test"
                i++;                                                                                                                    // Add atributes: "Name", "Expect" and value result to "Field"
            }

            xdoc.Element("Method").Add(root); // Add main catalog to Main catalog "Method"
            xdoc.Save(path); // Save xml file

        }

        public void CreateXML(string[] step_arr, string[] expect_arr, string[] results)
        {
            XElement root = new XElement("Method"); // Create root catalog "Tests"
            XAttribute root_atr = new XAttribute("SetupId", setupId); // Write atributes to catalog "Test"
            root.Add(root_atr);

            XElement test = new XElement("Test"); // Create root catalog "Tests"
            XAttribute atr = new XAttribute("TestId", id_test); // Write atributes to catalog "Test"
            test.Add(atr); // Add atribute to catalog "Test"
            
            int i = 0;
            foreach (string key in step_arr)
            {
                test.Add(new XElement("Field" + id_test, new XAttribute("Name", key), new XAttribute("Expect", expect_arr[i]), results[i])); //Create new catalog "Field" into catalog "Test"
                i++;                                                                                                                         // Add atributes: "Name", "Expect" and value result to "Field"
            }

            root.Add(test);
            root.Save(path);
        }

        public bool FindTest()
        {
            var xdoc = XDocument.Load(path);
            var node = xdoc.Descendants("Test");
            var atr = node.Attributes().ToList();
            bool find_test = false;

            foreach (string key in atr)
            {
                if (key == id_test)
                {
                    find_test = true;
                    break;
                }
            }
            return (find_test);
        }
    }
}
