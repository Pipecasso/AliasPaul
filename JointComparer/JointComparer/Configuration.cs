using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;


namespace JointComparer
{
    public class Configuration
    {

        private List<string> _pods;
        public string manifest { get; }
        public bool reference { get; }

        public string output { get; }

        public string input { get; }

        public List<string> pods { get => _pods; }


        public Configuration(string manifest, bool reference, string output, string input)
        {
            this.manifest = manifest;
            this.reference = reference;
            this.output = output;
            this.input = input;
            _pods = new List<string>();

            GetFilesIncludingSubfolders(input, "*.pod");
        }

        public Configuration(XmlDocument xDoc)
        {
            manifest = xDoc.SelectSingleNode(@"JointCompare_Config/Manifest").InnerText;
            string sref = xDoc.SelectSingleNode(@"JointCompare_Config/reference").InnerText;
            reference = sref == "true";
            output = xDoc.SelectSingleNode(@"JointCompare_Config/Output").InnerText;
            input = xDoc.SelectSingleNode(@"JointCompare_Config/Input").InnerText;
            _pods = new List<string>();

            GetFilesIncludingSubfolders(input, "*.pod");
        }

        private void GetFilesIncludingSubfolders(string path, string searchPattern)
        {
            Queue<string> directoriesQueue = new Queue<string>();
            directoriesQueue.Enqueue(path);

            while (directoriesQueue.Count > 0)
            {
                var currentPath = directoriesQueue.Dequeue();
                var directories = Directory.GetDirectories(currentPath);

                foreach (var directory in directories)
                {
                    string[] subdirs = directory.Split(Path.DirectorySeparatorChar);
                    string lastdir = subdirs[subdirs.Length - 1];
                    if ((reference == false && lastdir == "REFERENCE") || (reference && lastdir == "CURRENT"))
                    {
                        continue;
                    }
                    directoriesQueue.Enqueue(directory);
                }

                _pods.AddRange(Directory.GetFiles(currentPath, searchPattern).ToList());
            }
        }
    }
}
