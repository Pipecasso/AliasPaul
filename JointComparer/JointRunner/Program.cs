using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using PodHandshake;
using JointComparer;
using System.IO;
using System.Xml;

namespace JointRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(args[0]);

            JointCompare joint_compare = new JointCompare();
            Configuration joint_config = new Configuration(xDoc);
            joint_compare.Smoke(joint_config);
  
        }


        
    }
}
