using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointComparer
{
    public class JointPair : Tuple<Joint,Joint>
    {
        public JointPair(Joint j1, Joint j2) : base(j1, j2) { }

    
    }
}
