using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using PodHandshake;
using JointComparer;
using System.IO;

namespace JointRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirpath = args[0];
            string manifest_file1 = args[1];
            string manifest_file2 = args[2];

            JointCompare joint_compare = new JointCompare();
            List<string> podpaths = GetFilesIncludingSubfolders(dirpath, "*.pod",false);
            joint_compare.Smoke(podpaths, manifest_file2, @"D:\PODPCFStore\NewConrep\CustomerProjects\Reference");
  
        }


        public static List<string> GetFilesIncludingSubfolders(string path, string searchPattern,bool current)
        {
            List<string> paths = new List<string>();
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
                    if ( (current && lastdir == "REFERENCE")  || (!current && lastdir == "CURRENT"))
                    {
                        continue;
                    }
                    directoriesQueue.Enqueue(directory);
                }

                paths.AddRange(Directory.GetFiles(currentPath, searchPattern).ToList());
            }

            return paths;
        }
    }
}
