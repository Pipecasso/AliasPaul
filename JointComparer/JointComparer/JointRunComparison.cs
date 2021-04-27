using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointComparer
{
    public class JointRunComparison : Dictionary<JointPair, JointComparison>
    {
        
        public bool balanced { get; }


        public JointRunComparison(JointRun jr1, JointRun jr2,int indexshift)
        {
         
            if (jr1.Joints.Count == jr2.Joints.Count)
            {
                balanced = true; 
                foreach (KeyValuePair<int, Joint> kvpjoint in jr1.Joints)
                {
                    int currentindex = kvpjoint.Key - indexshift;
                    if (jr2.Joints.ContainsKey(currentindex))
                    {
                        Joint j1 = kvpjoint.Value;
                        Joint j2 = jr2.Joints[currentindex];
                        Add(new JointPair(j1, j2), new JointComparison(j1, j2));
                    }
                    else
                    {
                        balanced = false;
                        break;
                    }
                }
            }
            else
            {
                balanced = false;
            }
        }
    }
}
