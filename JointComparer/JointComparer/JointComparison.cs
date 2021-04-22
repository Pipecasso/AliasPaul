using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointComparer
{
    public class JointComparison
    {
        public bool KeypointA { get; }
        public bool KeypointB { get; }
        public bool Type { get; }
        public bool ConnectorCount{get;}
    
        //to do connector order/items

        public JointComparison(Joint j,Joint k)
        {
            KeypointA = j.KeypointA == k.KeypointA;
            KeypointB = j.KeypointB == k.KeypointB;
            
            
            Type = j.Type == k.Type;
            ConnectorCount = j.Connectors.Count == k.Connectors.Count;
        }

        public bool Identical
        { 
            get
            {
                return (KeypointA && KeypointB && Type && ConnectorCount);
            }
        
        }

    }
}
