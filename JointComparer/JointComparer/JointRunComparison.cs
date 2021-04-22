using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointComparer
{
    public class JointRunComparison : Dictionary<JointPair, JointComparison>
    {
        public int Excess { get; }
        public JointRunComparison(JointRun jr1, JointRun jr2)
        {
            Excess = jr1.Joints.Count - jr2.Joints.Count;

            JointRun smaller;
            JointRun larger;

            if (Excess <= 0)
            {
                smaller = jr1;
                larger = jr2;
            }
            else
            {
                larger = jr1;
                smaller = jr2;
            }

            foreach (KeyValuePair<int, Joint> kvp in smaller.Joints)
            {
                Joint j = kvp.Value;
                Joint k = larger.Joints[kvp.Key];
                JointPair jp = new JointPair(j, k);
                JointComparison jc = new JointComparison(j, k);
                if (jc.Identical == false)
                {
                    Add(jp, jc);
                }

            }
        }

        public bool SameSize
        {
            get { return Excess == 0; }
        }

        

    
    }
}
