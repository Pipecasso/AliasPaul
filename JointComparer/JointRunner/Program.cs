using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using PodHandshake;
using JointComparer;

namespace JointRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string podpath = args[0];
            string manifest_file1 = args[1];
       
            JointCompare joint_compare = new JointCompare();
            //joint_compare.Smoke(podpath, manifest_file1,"arse.xml");
            joint_compare.Light("arse.xml");
  
        }
    }
}
