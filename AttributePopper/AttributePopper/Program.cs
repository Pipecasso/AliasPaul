using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AliasPOD;
using Intergraph.PersonalISOGEN;
using PodHandshake;

namespace AttributePopper
{
    class Program
    {

       

        static void Main(string[] args)
        {
            string strManifest = args[0];
            string pathorpod = args[1];

            string mandir = Path.GetDirectoryName(strManifest);
            IsogenAssemblyLoader ial = new IsogenAssemblyLoader(strManifest, mandir);
            string corecomps = Path.Combine(mandir, "Core Components");
            ial.AddStringPath(corecomps);


            FileAttributes attr = File.GetAttributes(pathorpod);

            if (attr.HasFlag(FileAttributes.Directory))
            {

            }
            else
            {
                PopAPod(pathorpod, ial, mandir);
            }
        }

        static void PopAPod(string spod, IsogenAssemblyLoader ial, string bwfcfolder)
        {
            Func<int, string> RandomNumberFunction = (max) =>
            {
                Random r = new Random();
                int i = r.Next(0, max);
                return i.ToString();
            };

            Func<string> RandomDoctorFunction = () =>
            {
                Random r = new Random();
                int i = r.Next(0, 12);
                string doc = string.Empty;
                switch (i)
                {
                    case 0: doc = "Hartnell"; break;
                    case 1: doc = "Troughton"; break;
                    case 2: doc = "Pertwee"; break;
                    case 3: doc = "T.Baker"; break;
                    case 4: doc = "Davison"; break;
                    case 5: doc = "C.Baker"; break;
                    case 6: doc = "McCoy"; break;
                    case 7: doc = "McGann"; break;
                    case 8: doc = "Ecclesston"; break;
                    case 9: doc = "Tennant"; break;
                    case 10: doc = "Smith"; break;
                    case 11: doc = "Capaldi"; break;
                    case 12: doc = "Whittaker"; break;
                }
                return doc;
            };

            Func<string> RandomBeatleFunction = () =>
            {
                Random r = new Random();
                int i = r.Next(0, 3);
                string beatle = string.Empty;
                switch (i)
                {
                    case 0: beatle = "John"; break;
                    case 1: beatle = "Paul"; break;
                    case 2: beatle = "George"; break;
                    case 3: beatle = "Ringo"; break;
                }
                return beatle;
            };

            Func<string> RandomUsState = () =>
            {
                List<string> states = new List<string>();
                states.Add("New York");
                states.Add("Maine");
                states.Add("Florida");
                states.Add("Massachusetts");
                states.Add("Alabama");
                states.Add("Georgia");
                states.Add("Tennessee");
                states.Add("Alaska");
                states.Add("Hawaii");
                states.Add("Washington");
                states.Add("Oregon");
                states.Add("Wyoming");
                states.Add("Iowa");
                states.Add("New Mexico");
                states.Add("Nevada");
                states.Add("Texas");
                states.Add("North Dakota");
                states.Add("South Dakota");
                states.Add("North Carolina");
                states.Add("South Carolina");
                states.Add("Louisiana");
                states.Add("Utah");
                states.Add("Maryland");
                states.Add("Virginia");
                states.Add("West Virginia");
                states.Add("Montana");
                states.Add("New Hampshire");
                states.Add("Vermont");
                states.Add("New Jersey");
                states.Add("Rhode Island");
                states.Add("Connecticut");
                states.Add("Idaho");
                states.Add("Colarado");
                states.Add("Arkansaw");
                states.Add("Pennsylvania");
                states.Add("Michigan");
                states.Add("Illinois");
                states.Add("Minnesota");
                states.Add("Wisconsin");
                states.Add("Kentucky");
                states.Add("Arizona");
                states.Add("Kansas");
                states.Add("Mississippi");
                states.Add("Ohio");
                states.Add("Oklahoma");
                states.Add("California");
                states.Add("Nebraska");
                states.Add("Delaware");
                states.Add("Indiana");
                states.Add("Missouri");


                Random r = new Random();
                return states[r.Next(49)];
            };
            
            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                POD pod = new POD();
                pod.Handshake = HandshakeTools.GetPODHandshake(bwfcfolder);
                pod.Load(spod);
                string poddir = Path.GetDirectoryName(spod);
                string fname = Path.GetFileNameWithoutExtension(spod);
                string fpop = $"{fname}pop.pod";
                string newpath = Path.Combine(poddir, fpop);

                for (int i = 0; i < pod.Pipelines.Count; i++)
                {
                    Pipeline pline = pod.Pipelines.Item(i);
                    SetAttributes(string.Empty, pline.Attributes, RandomBeatleFunction, RandomNumberFunction, 250, 43);
                    for (int ii = 0; ii < pline.Components.Count; ii++)
                    {
                        Component component = pline.Components.Item(ii);
                        if (component.Material.Group == "Welds")
                        {
                            //Attribute 37 is UState
                            SetAttributes("WELD-", component.Attributes, RandomUsState, RandomNumberFunction, 100, 37);
                        } 
                        else
                        {
                            //attribute 61 is Doctor Who
                            SetAttributes("COMPONENT-", component.Attributes, RandomDoctorFunction, RandomNumberFunction, 1000, 71);
                        }

                        for (int inf = 0;inf < component.InformationElements.Count;inf ++)
                        {
                            InformationElement ie = component.InformationElements.Item(inf);
                            if (ie.Type == "Thickness-Measurement-Location")
                            {
                                SetAttributes("INFORMATION-", ie.Attributes, RandomBeatleFunction, RandomNumberFunction, 500, 23);
                            }
                        }           
                    }
                }

                pod.Save(newpath);
            }
        }

        static void SetAttributes(string attkey, AliasPOD.Attributes attributes, Func<string> attributefunc,Func<int,string> attfunction2,int max,int ioddoneout)
        {
            for (int i = 1; i <= 100; i++)
            {
                string AttributeName = $"{attkey}ATTRIBUTE{i}";
                AliasPOD.Attribute attribute = attributes.Item(AttributeName);
                if (attribute.IsValueValid()) continue;
                if (i==ioddoneout)
                {
                    attribute.Value = attributefunc();
                }
                else
                {
                    attribute.Value = attfunction2(max);
                }
            }

        }

      

    }
}
