using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;



namespace JointComparer
{
    public class Joint
    {
        public PODKeypoint KeypointA { get; }
        public PODKeypoint KeypointB { get; }


        int iType { get; }

        public AliasPOD.eConnection Type { get => (AliasPOD.eConnection)iType; }



        public List<PODComponent> Connectors { get; }

        public int Index { get; }


        public Joint(AliasPOD.Connection pod_joint)
        {
            Connectors = new List<PODComponent>();

            AliasPOD.Component compKeyA = null;
            AliasPOD.Component compKeyB = null;

            if (pod_joint.KeypointA == null)
            {
                KeypointA = new PODKeypoint(null, string.Empty, true);
            }
            else
            {
                compKeyA = pod_joint.KeypointA.Component;
                PODComponent podcompKeyA = new PODComponent(compKeyA.SequenceNumber, compKeyA.ExternalUCI, compKeyA.ExternalUCIIndex,compKeyA.FlyText);
                KeypointA = new PODKeypoint(podcompKeyA, pod_joint.KeypointA.Name, true);
            }

            if (pod_joint.KeypointB == null)
            {
                KeypointB = new PODKeypoint(null, string.Empty, false);
            }
            else
            {
                compKeyB = pod_joint.KeypointB.Component;
                PODComponent podcompKeyB = new PODComponent(compKeyB.SequenceNumber, compKeyB.ExternalUCI, compKeyB.ExternalUCIIndex,compKeyB.FlyText);
                KeypointB = new PODKeypoint(podcompKeyB, pod_joint.KeypointB.Name,false);
            }

            iType = (int)pod_joint.Type;
            AliasPOD.Components components = pod_joint.Components;
            foreach (AliasPOD.Component c in components)
            {
                PODComponent podComponent = new PODComponent(c.SequenceNumber, c.ExternalUCI, c.ExternalUCIIndex, c.FlyText); 
                Connectors.Add(podComponent);
            }

            Index = pod_joint.Index;
        }

        public Joint(XmlNode xJoint)
        {
            string Type = xJoint.Attributes["Type"].Value;
            iType = Convert.ToInt32(Type);
            Connectors = new List<PODComponent>();

            string sindex = xJoint.Attributes["Index"].Value.ToString();
            Index = Convert.ToInt32(sindex);

            XmlNode xKeyA = xJoint.SelectSingleNode("Keypoint[@JointName='KeyA']");
            KeypointA = new PODKeypoint(xKeyA);

            XmlNode xKeyB = xJoint.SelectSingleNode("Keypoint[@JointName='KeyB']");
            KeypointB = new PODKeypoint(xKeyB);

            XmlNodeList xConnectors = xJoint.SelectNodes("Connectors/Component");
            foreach (XmlNode xConnect in xConnectors)
            {
                PODComponent connector = new PODComponent(xConnect);
                Connectors.Add(connector);
            
            }
        }

        static public bool operator ==(Joint j, Joint k)
        {
            bool ret;
            if (j is null && k is null)
            {
                ret = true;
            }
            else if (j is null || k is null)
            {
                ret = false;
            }
            else
            {
                ret = j.Type == k.Type && j.Connectors.Count == k.Connectors.Count && KeypointEquivalence(j.KeypointA, k.KeypointA) && KeypointEquivalence(j.KeypointB, k.KeypointB);
                if (ret)
                {
                    //maybe check for equivalane of connectors
                    


                }
            }

            return ret;
        }


        static bool KeypointEquivalence(PODKeypoint jk,PODKeypoint kk)
        {
            bool ret;
            if (jk == kk)
            {
                ret = true;
            }
            else if ( (jk == null || kk == null) || (jk.name != kk.name))
            {
                ret = false;
            }
            else 
            {
                PODComponent jcomp = jk.component;
                PODComponent kcomp = kk.component;
                ret = ComponentEquivalence(jcomp, kcomp);
            }
            return ret;
        }

        static bool ComponentEquivalence(PODComponent jcomp,PODComponent kcomp)
        {
            bool ret;
            if (jcomp.ExternalUCI == kcomp.ExternalUCI && jcomp.ExternalUCIIndex == kcomp.ExternalUCIIndex)
            {
                ret = true;
            }
            else
            {
                ret = jcomp.SequenceNumber == kcomp.SequenceNumber;
            }
            return ret;
        }

        static public bool operator != (Joint j, Joint k)
        {
            return !(j == k);
        }

        public override bool Equals(Object o)
        {
            bool ret = false;
            if (o is Joint)
            {
                Joint j = (Joint)o;
                ret = this == j;
            }
            return ret;
        }

        public override int GetHashCode()
        {
            int hashCode = -1222045930;
            hashCode = hashCode * -1521134295 + EqualityComparer<PODKeypoint>.Default.GetHashCode(KeypointA);
            hashCode = hashCode * -1521134295 + EqualityComparer<PODKeypoint>.Default.GetHashCode(KeypointB);
            hashCode = hashCode * -1521134295 + iType.GetHashCode();
            foreach (PODComponent component in Connectors)
            {
                hashCode = hashCode * -1521134295 + component.GetHashCode();
            }
            return hashCode;
        }

        public void Save(XmlDocument xDoc,XmlNode xParent)
        {
            XmlNode xNode = xDoc.CreateNode(XmlNodeType.Element, "Joint", xDoc.NamespaceURI);
            XmlAttribute xAtt = xDoc.CreateAttribute("Type");
            xNode.Attributes.Append(xAtt);
            xAtt.Value = iType.ToString();
            xParent.AppendChild(xNode);

            XmlAttribute xIndex = xDoc.CreateAttribute("Index");
            xIndex.Value = Index.ToString();
            xNode.Attributes.Append(xIndex);


            KeypointA.Save(xDoc, xNode);
            KeypointB.Save(xDoc, xNode);
           
            XmlNode xNodeConnectors = xDoc.CreateNode(XmlNodeType.Element, "Connectors", xDoc.NamespaceURI);
            XmlAttribute xCount = xDoc.CreateAttribute("Count");
            xCount.Value = Connectors.Count.ToString();
            xNodeConnectors.Attributes.Append(xCount);

            xNode.AppendChild(xNodeConnectors);
            foreach (PODComponent podComponent in Connectors)
            {
                podComponent.Save(xDoc, xNodeConnectors);
            } 
        }

      
    }
}
