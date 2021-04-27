using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JointComparer
{
    public class PODKeypoint
    {
        public PODKeypoint(PODComponent component, string name,bool keypointA)
        {
            this.component = component;
            this.name = name;
            this.KeypointA = keypointA;
        }

        public PODKeypoint(XmlNode xKey)
        {
            name = xKey.Attributes["Name"].Value;
            string jointName = xKey.Attributes["JointName"].Value;
            KeypointA = jointName == "KeyA";
            XmlNode xComponent = xKey.SelectSingleNode("Component");
            if (xComponent!=null)
            {
                component = new PODComponent(xComponent);
            }
        }

        public PODComponent component { get; }
        public string name { get; }

        public bool KeypointA { get; }

        public string JointName { get => KeypointA ? "KeyA" : "KeyB";}

        public static bool operator == (PODKeypoint keypoint1, PODKeypoint keypoint2)
        {
            bool ret;
            if (keypoint1 is null && keypoint2 is null)
            {
                ret = true;
            }
            else if (keypoint1 is null || keypoint2 is null)
            {
                ret = false;
            }
            else
            {
                ret = keypoint1.Equals(keypoint2);
            }
            return ret;
        }

        public static bool operator !=(PODKeypoint keypoint1, PODKeypoint keypoint2)
        {
            return !(keypoint1 == keypoint2);
        }

        public void Save(XmlDocument xDoc,XmlNode xParent)
        {
            XmlNode xNode = xDoc.CreateNode(XmlNodeType.Element, "Keypoint", xDoc.NamespaceURI);
            XmlAttribute xName = xDoc.CreateAttribute("Name");

            if (component == null)
            {
                xName.Value = "null";
            }
            else
            {
                xName.Value = name;
                component.Save(xDoc, xNode);
            }

            xNode.Attributes.Append(xName);

            XmlAttribute xJName = xDoc.CreateAttribute("JointName");
            xJName.Value =JointName;
            xNode.Attributes.Append(xJName);

            xParent.AppendChild(xNode);
        }

        public override bool Equals(object obj)
        {
            return obj is PODKeypoint keypoint &&
                   EqualityComparer<PODComponent>.Default.Equals(component, keypoint.component) &&
                   name == keypoint.name &&
                   KeypointA == keypoint.KeypointA;
        }

        public override int GetHashCode()
        {
            int hashCode = 19451855;
            hashCode = hashCode * -1521134295 + EqualityComparer<PODComponent>.Default.GetHashCode(component);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + KeypointA.GetHashCode();
            return hashCode;
        }
    }
}
