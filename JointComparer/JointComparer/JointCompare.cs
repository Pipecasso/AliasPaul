using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using PodHandshake;
using System.Xml;

namespace JointComparer
{
    public class JointCompare
    {

        private List<JointRun> _JointRuns;
        public JointCompare()
        {
            _JointRuns = new List<JointRun>();
    
        }

     

        public void Smoke(string pod, string manifest,string xmlpath)
        {
            string manifestRootFolder = System.IO.Path.GetDirectoryName(manifest);


            IsogenAssemblyLoader ial = new IsogenAssemblyLoader(manifest, manifestRootFolder, manifestRootFolder, true);

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                AliasPOD.POD p = new AliasPOD.POD();
                p.Handshake = HandshakeTools.GetPODHandshake(manifestRootFolder);
                p.Load(pod);
                Smoke(p);
            }

            XmlDocument xDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

            //create the root element
            XmlElement root = xDoc.DocumentElement;
            xDoc.InsertBefore(xmlDeclaration, root);

            XmlNode xNode = xDoc.CreateNode(XmlNodeType.Element, "JointRuns", xDoc.NamespaceURI);
            XmlAttribute xAtt = xDoc.CreateAttribute("Count");
            xAtt.Value = _JointRuns.Count.ToString();
            xNode.Attributes.Append(xAtt);
            xDoc.AppendChild(xNode);

            foreach (JointRun jr in _JointRuns)
            {
                jr.Save(xDoc, xNode);
            }

            xDoc.Save(xmlpath);
         
        }

        public void Light(string xmlpath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlpath);

            XmlNodeList xJoints = xDoc.SelectNodes(@"/JointRuns/JointRun");
            foreach (XmlNode xJoint in xJoints)
            {
                JointRun jr = new JointRun(xJoint);
                _JointRuns.Add(jr);
            }


        }


        public void Smoke(AliasPOD.POD pod)
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
                    _JointRuns.Add(jr);
                }
            }
        }
    }
}
