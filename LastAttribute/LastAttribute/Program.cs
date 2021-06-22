using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using Intergraph.PersonalISOGEN;
using AliasPOD;
using System.IO;

namespace LastAttribute
{
    class Program
    {
        static TimeSpan LastAttribute(LoadedPod loadedPod)
        {
    
            using (IsogenAssemblyLoaderCookie cookieMonster = new IsogenAssemblyLoaderCookie(loadedPod.isogenAssemblyLoader))
            {
                DateTime loopstart = DateTime.Now;
                foreach (Pipeline pipeline in loadedPod.pod.Pipelines)
                {
                    foreach (Component component in pipeline.Components)
                    {
                        long attCount = component.Attributes.Count;
                        AliasPOD.Attribute lastAtt = component.Attributes.Item(attCount - 1);
                       
                    }
                }
                DateTime loopend = DateTime.Now;
                return loopend - loopstart;
            }




        }
        
        static void Main(string[] args)
        {
            string manifest = args[0];
            string podpath = args[1];
            int loops = Convert.ToInt32(args[2]);
            int pulse = 0;
            if (loops > 1000)
            {
                pulse = loops / 1000;
            }
            else
            {
                pulse = 1;
            }

            DateTime dateimestart = DateTime.Now;

            LoadedPod loadedPOD = new LoadedPod(manifest, podpath);

            DateTime podloaded = DateTime.Now;
            TimeSpan podloadtime = podloaded - dateimestart;

            List < TimeSpan > times = new List<TimeSpan>();
            for (int i=0;i<loops;i++)
            {
                times.Add(LastAttribute(loadedPOD));
                if (i%pulse == 0)
                {
                    System.Console.WriteLine(i.ToString());
                }
            }

            DateTime finished = DateTime.Now;
            TimeSpan totalloops = finished - podloaded;
            
            using (StreamWriter sw=  new StreamWriter("LastAttribute.log"))
            {
                int tick = 1;
                sw.WriteLine($"POD Load Time {podloadtime.ToString()}");
                foreach (TimeSpan ts in times)
                {
                    sw.WriteLine($"Loop {tick} - { ts.ToString()} ");
                    tick++;
                }

                sw.WriteLine($"Total Loop Time {totalloops.ToString()}");
            }
        }
    }
}
