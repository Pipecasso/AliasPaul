using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AliasPOD;
using Intergraph.PersonalISOGEN;
using PodHandshake;
using AliasImportExport;

namespace AttributePopper
{
    class Program
    {
 
        static void Main(string[] args)
        {
            string strManifest = args[0];
            string pathorpod = args[1];
            string save = args.Length > 2 ? args[2] : string.Empty;

            bool saveit = save == "y";
            string mandir = Path.GetDirectoryName(strManifest);
            IsogenAssemblyLoader ial = new IsogenAssemblyLoader(strManifest, mandir);
            string corecomps = Path.Combine(mandir, "Core Components");
            ial.AddStringPath(corecomps);
            Random randy = new Random();

            FileAttributes attr = File.GetAttributes(pathorpod);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                var pods = Directory.EnumerateFiles(pathorpod, "*.pod", SearchOption.TopDirectoryOnly);
                foreach (string pod in pods)
                {
                    string poddir = Path.GetDirectoryName(pod);
                    string fname = Path.GetFileNameWithoutExtension(pod);
                    string p = $"{fname}.pcf";
                    string pcfpath = Path.Combine(poddir, p);

                    if (!File.Exists(pcfpath))
                    {
                        PopAPod(randy,pod, ial, mandir, pcfpath,saveit);
                    }
                }
            }
            else
            {
                string poddir = Path.GetDirectoryName(pathorpod);
                string fname = Path.GetFileNameWithoutExtension(pathorpod);
                string pcfpath = Path.Combine(poddir, $"{fname}.pcf");
                if (!File.Exists(pcfpath))
                {
                    PopAPod(randy,pathorpod, ial, mandir, pcfpath,saveit);
                }
            }
        }

        static void PopAPod(Random rand,string spod, IsogenAssemblyLoader ial, string bwfcfolder,string pcfpath,bool save)
        {
       
            Func<Random,int, string> RandomNumberFunction = (r,max) =>
            {
                int i = r.Next(0, max);
                return i.ToString();
            };

            Func<Random ,string> RandomDoctorFunction = (r) =>
            {
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

            Func<Random,string> RandomBeatleFunction = (r) =>
            {
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

            Func<Random,string> RandomUsState = (r) =>
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

                return states[r.Next(49)];
            };

            Func<Random, string> FounderMembers = (r) =>
             {
                 string go = string.Empty;
                 int i = r.Next(0, 11);
                 switch (i)
                 {
                     case 0: go = "Accrington Stangley"; break;
                     case 1: go = "Aston Villa"; break;
                     case 2: go = "Blackburn Rovers"; break;
                     case 3: go = "Bolton Wanderers"; break;
                     case 4: go = "Burnley"; break;
                     case 5: go = "Derby County"; break;
                     case 6: go = "Everton"; break;
                     case 7: go = "Notts County"; break;
                     case 8: go = "Preston North End"; break;
                     case 9: go = "Stoke City"; break;
                     case 10: go = "West Bromwich Albion"; break;
                     case 11: go = "Wolverhampton Wanderers"; break;
                 }
                 return go;
             };
            
            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                POD pod = new POD();
                pod.Handshake = HandshakeTools.GetPODHandshake(bwfcfolder);
                pod.Load(spod);

                Random r = new Random();

                Console.WriteLine(spod);
                for (int i = 0; i < pod.Pipelines.Count; i++)
                {
                    Pipeline pline = pod.Pipelines.Item(i);
                    SetAttributes(rand,string.Empty, pline.Attributes, RandomBeatleFunction, RandomNumberFunction, 250, 43);
                    for (int ii = 0; ii < pline.Components.Count; ii++)
                    {
                        Component component = pline.Components.Item(ii);
                        if (component.Material.Group == "Welds")
                        {
                            //Attribute 37 is UState
                            SetAttributes(rand,"WELD-", component.Attributes, RandomUsState, RandomNumberFunction, 100, 37);
                        } 
                        else
                        {
                            //attribute 71 is Doctor Who, attribute 17 is football
                            SetAttributes(rand,"COMPONENT-", component.Attributes, RandomDoctorFunction,FounderMembers, RandomNumberFunction, 1000, 71,17);
                        }

                        for (int inf = 0;inf < component.InformationElements.Count;inf ++)
                        {
                            InformationElement ie = component.InformationElements.Item(inf);
                            if (ie.Type == "Thickness-Measurement-Location")
                            {
                                SetAttributes(rand,"INFORMATION-", ie.Attributes, RandomBeatleFunction, RandomNumberFunction, 500, 23);
                            }
                        }           
                    }
                }


                if (save)
                {
                    string dir = Path.GetDirectoryName(spod);
                    string filename = Path.GetFileName(spod);
                    string newpodpath = Path.Combine(dir, "POP", filename);
                    string newdir = Path.GetDirectoryName(newpodpath);
                    
                    if (Directory.Exists(newdir) == false)
                    {
                        Directory.CreateDirectory(newdir);
                    }

                    if (File.Exists(newpodpath) == false)
                    {
                        pod.Save(newpodpath);
                    }
                  
                }

                AliasImportExport.ImportExport importExport = new ImportExport();
                importExport.InputObject = pod;
                importExport.InputType = (int)InputType.eInputTypePOD;
                importExport.OutputType = (int)OutputType.eOutputTypePCF;
                importExport.OutputName = pcfpath;
                int iExp = importExport.Execute();

            }
        }

        static void SetAttributes(Random randy,string attkey, AliasPOD.Attributes attributes, Func<Random,string> attributefunc,Func<Random,int,string> attfunction2,int max,int ioddoneout)
        {
            for (int i = 1; i <= 100; i++)
            {
                string AttributeName = $"{attkey}ATTRIBUTE{i}";
                AliasPOD.Attribute attribute = attributes.Item(AttributeName);
                if (attribute == null) break;
                if (attribute.IsValueValid()) continue;
                if (i==ioddoneout)
                {
                    attribute.Value = attributefunc(randy);
                }
                else if (i==ioddoneout)
                {
                    attribute.Value = attributefunc(randy);
                }
                else
                {
                    attribute.Value = attfunction2(randy,max);
                }
            }

        }

        static void SetAttributes(Random randy, string attkey, AliasPOD.Attributes attributes, Func<Random, string> attributefunc, Func<Random, string> attributefunc2, Func<Random, int, string> attributefunc3, int max, int ioddoneout1, int ioddoneout2)
        {
            for (int i = 1; i <= 100; i++)
            {
                string AttributeName = $"{attkey}ATTRIBUTE{i}";
                AliasPOD.Attribute attribute = attributes.Item(AttributeName);
                if (attribute == null) break;
                if (attribute.IsValueValid()) continue;
                if (i == ioddoneout1)
                {
                    attribute.Value = attributefunc(randy);
                }
                else if (i==ioddoneout2)
                {
                    attribute.Value = attributefunc2(randy);
                }
                else
                {
                    attribute.Value = attributefunc3(randy, max);
                }
            }

        }



    }
}
