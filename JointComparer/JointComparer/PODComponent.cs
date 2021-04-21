using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JointComparer
{
    public class PODComponent
    {
        public PODComponent(int sequenceNumber, string externalUCI, int externalUCIIndex,string flyText)
        {
            SequenceNumber = sequenceNumber;
            ExternalUCI = externalUCI;
            ExternalUCIIndex = externalUCIIndex;
            FlyText = flyText;
        }

        public PODComponent(XmlNode xNode)
        {
            string seqNum = xNode.Attributes["SequenceNumber"].Value;
            SequenceNumber = Convert.ToInt32(seqNum);
            ExternalUCI = xNode.Attributes["ExternalUCI"].Value;
            string xUCIIndex = xNode.Attributes["ExternalUCIIndex"].Value;
            ExternalUCIIndex = Convert.ToInt32(xUCIIndex);
            FlyText = xNode.Attributes["Description"].Value;
        }

        public int SequenceNumber { get; }
        public string ExternalUCI { get; }
        public int ExternalUCIIndex { get; }

        public string FlyText { get; }

        public override bool Equals(object obj)
        {
            return obj is PODComponent component &&
                   SequenceNumber == component.SequenceNumber &&
                   ExternalUCI == component.ExternalUCI &&
                   ExternalUCIIndex == component.ExternalUCIIndex &&
                   FlyText == component.FlyText;
        }

        public override int GetHashCode()
        {
            int hashCode = 1689777945;
            hashCode = hashCode * -1521134295 + SequenceNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExternalUCI);
            hashCode = hashCode * -1521134295 + ExternalUCIIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FlyText);
            return hashCode;
        }

        public static bool operator ==(PODComponent component1, PODComponent component2)
        {
            bool ret;
            if (component1 is null && component2 is null)
            {
                ret = true;
            }
            else if (component1 is null || component2 is null)
            {
                ret = false;
            }
            else
            {
                ret = component1.Equals(component2);
            }
            return ret;
        }

        public static bool operator !=(PODComponent component1,PODComponent component2)
        {
            return !(component1 == component2);
        }


        public void Save(XmlDocument xdoc,XmlNode parent)
        {
            XmlNode xNode = xdoc.CreateNode(XmlNodeType.Element, "Component",xdoc.NamespaceURI);
            
            XmlAttribute xSeq = xdoc.CreateAttribute("SequenceNumber");
            xSeq.Value = SequenceNumber.ToString();
            xNode.Attributes.Append(xSeq);

            XmlAttribute xXUCI = xdoc.CreateAttribute("ExternalUCI");
            xXUCI.Value = ExternalUCI;
            xNode.Attributes.Append(xXUCI);

            XmlAttribute xXIndex = xdoc.CreateAttribute("ExternalUCIIndex");
            xXIndex.Value = ExternalUCIIndex.ToString();
            xNode.Attributes.Append(xXIndex);

            XmlAttribute xFly = xdoc.CreateAttribute("Description");
            xFly.Value = FlyText;
            xNode.Attributes.Append(xFly);

            parent.AppendChild(xNode);
        }

    }
}
