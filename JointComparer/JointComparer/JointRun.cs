using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JointComparer
{
    public class JointRun
    {
        private Dictionary<int, Joint> _Joints;
        public string Name { get; }

        public JointRun(AliasPOD.PipelineItemGroup pig,string name)
        {
            _Joints = new Dictionary<int, Joint>();
            for (int i=0;i<pig.Count;i++)
            {
                AliasPOD.IPipelineItem pi = pig.Item(i);
                if (pi.Type == AliasPOD.eItemType.eITConnection)
                {
                    AliasPOD.Connection con = (AliasPOD.Connection)pi.Object;
                    _Joints.Add(con.Index, new Joint(con));
                }
            }
            Name = name;
        }

        public JointRun(XmlNode xJointRun)
        {
            _Joints = new Dictionary<int, Joint>();
            Name = xJointRun.Attributes["Name"].Value;
            XmlNodeList xJoints = xJointRun.SelectNodes("Joint");
            foreach (XmlNode xJoint in xJoints)
            {
                Joint joint = new Joint(xJoint);
                _Joints.Add(joint.Index, joint);
            }
        }

        public Dictionary<int, Joint> Joints { get => _Joints; }

        public override bool Equals(object obj)
        {
            return obj is JointRun run && ((JointRun)obj == this);
        }

        public override int GetHashCode()
        {
            return 1628168371 + EqualityComparer<Dictionary<int, Joint>>.Default.GetHashCode(_Joints);
        }

        static public bool operator == (JointRun jr1, JointRun jr2)
        {
            bool ret;
            if (jr1 is null && jr2 is null)
            {
                ret = true;
            }
            else if (jr1 is null || jr2 is null)
            {
                ret = false;
            }
            else
            {
                ret = true;
                if (jr1.Joints.Count == jr2.Joints.Count)
                {
                    foreach (KeyValuePair<int,Joint> kvp in jr1.Joints)
                    {
                        if (jr2.Joints.ContainsKey(kvp.Key))
                        {
                           
                            Joint j1 = kvp.Value;
                            Joint j2 = jr2.Joints[kvp.Key];
                            if (j1!=j2)
                            {
                                ret = false;
                                break;
                            }
                        
                        
                        }
                        else
                        {
                            ret = false;
                            break;
                        }
                    }
                }
                else
                {
                    ret = false;
                }
            }
            return ret;
        }

        static public bool operator != (JointRun jr1, JointRun jr2)
        {
            return !(jr1 == jr2);

        }

        public void Save(XmlDocument xDoc,XmlNode xParent)
        {
            XmlNode xNode = xDoc.CreateNode(XmlNodeType.Element, "JointRun", xDoc.NamespaceURI);

            XmlAttribute xName = xDoc.CreateAttribute("Name");
            xNode.Attributes.Append(xName);
            xName.Value = Name;

            XmlAttribute xCount = xDoc.CreateAttribute("Count");
            xNode.Attributes.Append(xCount);
            xCount.Value = _Joints.Count.ToString();

            xParent.AppendChild(xNode);

            foreach(KeyValuePair<int,Joint> kvp in _Joints)
            {
                kvp.Value.Save(xDoc, xNode);

            }
        }
    }
}
