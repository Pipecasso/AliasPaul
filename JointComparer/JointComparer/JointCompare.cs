using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using PodHandshake;
using System.Xml;
using System.IO;

namespace JointComparer
{
    public class JointCompare
    {

        //these need seperating into classes _xmlpaths only relevant for saving  _Diffs only relevant for comparison
        //the whole class needs splitting in two actually.

        private Dictionary<string,string> _XmlPaths;
        private List<ConrepPair> _Diffs;
       


        public void Smoke(Configuration configfile)
        {
            _XmlPaths = new Dictionary<string, string>();
            string manifestRootFolder = System.IO.Path.GetDirectoryName(configfile.manifest);
            IsogenAssemblyLoader ial = new IsogenAssemblyLoader(configfile.manifest, manifestRootFolder, manifestRootFolder, true);
            foreach (string pod in configfile.pods)
            {
                Smoke(pod, manifestRootFolder, ial,configfile.output,configfile.reference);
            }

        }

     

        private void Smoke(string pod, string manifestroot,IsogenAssemblyLoader ial,string xmlfolder,bool reference)
        {
            List<JointRun> JointRuns = new List<JointRun>();
            string[] subdirs = pod.Split(Path.DirectorySeparatorChar);

            string outfolder = xmlfolder == string.Empty ? Path.GetDirectoryName(pod) : xmlfolder;
            string podname = Path.GetFileNameWithoutExtension(pod);
            string lastfolder = subdirs[subdirs.Length - 2];
            string xmlpath = $@"{outfolder}\{ (reference ? "reference" : "current") }\{lastfolder}{podname}.xml";

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                AliasPOD.POD p = new AliasPOD.POD();
                p.Handshake = HandshakeTools.GetPODHandshake(manifestroot);
                p.Load(pod);
                Smoke(p,JointRuns);
            }

            XmlDocument xDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

            //create the root element
            XmlElement root = xDoc.DocumentElement;
            xDoc.InsertBefore(xmlDeclaration, root);

            XmlNode xNode = xDoc.CreateNode(XmlNodeType.Element, "JointRuns", xDoc.NamespaceURI);
            XmlAttribute xAtt = xDoc.CreateAttribute("Count");
            xAtt.Value = JointRuns.Count.ToString();
            xNode.Attributes.Append(xAtt);
            xDoc.AppendChild(xNode);

            foreach (JointRun jr in JointRuns)
            {
                jr.Save(xDoc, xNode);
            }


            _XmlPaths.Add(xmlpath,pod);
            xDoc.Save(xmlpath);

        }

        private static void PopulateJointRun(string xmlpath,List<JointRun> JointRuns)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlpath);

            XmlNodeList xJoints = xDoc.SelectNodes(@"/JointRuns/JointRun");
            foreach (XmlNode xJoint in xJoints)
            {
                JointRun jr = new JointRun(xJoint);
                JointRuns.Add(jr);
            }
        }

        public void Light(string path)
        {
            _Diffs = new List<ConrepPair>();
       
            string current = Path.Combine(path, "current");
            string reference = Path.Combine(path, "reference");

            HashSet<string> CurrentXML = GetXMLJoints(current);
            HashSet<string> ReferenceXML = GetXMLJoints(reference);
            HashSet<string> Intersection = new HashSet<string>();
         
            int differences = 0;

            foreach (string s in CurrentXML)
            {
                if (ReferenceXML.Contains(s))
                {
                    Intersection.Add(s);
                }
            }
            CurrentXML.RemoveWhere(s => (Intersection.Contains(s)));
            ReferenceXML.RemoveWhere(s => (Intersection.Contains(s)));



            XmlDocument xDoc = new XmlDocument();
            
            foreach(string s in Intersection)
            {
                List<JointRun> ReferenceJointRuns = new List<JointRun>();
                List<JointRun> CurrentJointRuns = new List<JointRun>();
                string curxml = Path.Combine(current, s);
                string refxml = Path.Combine(reference, s);
                PopulateJointRun(refxml, ReferenceJointRuns);
                PopulateJointRun(curxml, CurrentJointRuns);

                int Tick = 0;
                bool equal = true;

                if (ReferenceJointRuns.Count == CurrentJointRuns.Count)
                {
                    foreach (JointRun jref in ReferenceJointRuns)
                    {
                        JointRun jrcur = CurrentJointRuns[Tick];
                        if (jref != jrcur)
                        {
                            equal = false;
                            break;
                        }
                        Tick++;
                    }

                    if (!equal)
                    {
                        differences++;
                        ConrepPair crp = new ConrepPair(s, ReferenceJointRuns, CurrentJointRuns);
                        _Diffs.Add(crp);
                    }
                }
                else
                {
                    differences++;
                }
            }
         
        }

        private HashSet<string> GetXMLJoints(string path)
        {
            string[] xfiles = Directory.GetFiles(path, "*.xml");
            HashSet<string> outfiles = new HashSet<string>();
            foreach (string s in xfiles)
            {
                outfiles.Add(Path.GetFileName(s));
            }
           
            return outfiles;
        }
      

        static public void Smoke(AliasPOD.POD pod,List<JointRun> jointRuns)
        {
            for (int i = 0; i < pod.Pipelines.Count; i++)
            {
                AliasPOD.Pipeline Pipeline = pod.Pipelines.Item(i);
                AliasPOD.PipelineItems items = Pipeline.GetItems(AliasPOD.eItemType.eITComponent | AliasPOD.eItemType.eITConnection, AliasPOD.eSortCriteria.eSCRun);

                for (int j = 0; j < items.Count; j++)
                {
                    AliasPOD.PipelineItemGroup pig = items.Item(j);
                    string name = $"JointRun_{j+1}";
                    JointRun jr = new JointRun(pig,name);
                    jointRuns.Add(jr);
                }
            }
        }

        void ComparisonReport()
        {
        }
    }
}
