using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace StyleManager
{
    class Program
    {
        private static Config config;

        static void Main(string[] args)
        {
            //Load configuration
            var json = File.ReadAllText("appsettings.json");
            config = JsonConvert.DeserializeObject<Config>(json);

            //Get list of selected styles
            List<List<string>> selectedNames = new List<List<string>>();
            var files = Directory.GetFiles(config.root + "\\" + config.selection);
            foreach (var file in files)
            {
                //trim off path and extension, value will be used for replacements
                string temp = file.Replace(config.root + "\\" + config.selection + "\\", "");
                temp = temp.Replace(".png", "");
                temp = temp.Replace(".jpg", "");
                //generate guid to be used for this value
                string guid = Guid.NewGuid().ToString();
                List<string> pair = new List<string>();
                pair.Add(temp);
                pair.Add(guid);
                selectedNames.Add(pair);
            }

            RenameFilesInDirectory(config.selection, selectedNames);
            RenameFilesInDirectory(config.thumbnails, selectedNames);
            RenameFilesInDirectory(config.models, selectedNames);
        }

        private static void RenameFilesInDirectory(string directory, List<List<string>> selections)
        {
            //get directory info
            var path = config.root + "\\" + directory;
            var files = Directory.GetFiles(path);
            //Update names of selected images
            foreach (var s in selections)
            {
                foreach(var f in files)
                {
                    if (f.Contains(s[0]))
                    {
                        var dest = f.Replace(s[0], s[1]);
                        Console.WriteLine("Source: " + f);
                        Console.WriteLine("Destination: " + dest);
                        File.Move(f, dest);
                    }
                }
            }
        }
    }
}
