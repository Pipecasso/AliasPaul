using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointComparer
{
    class JointRunDifferenceTotals
    {
        public JointRunDifferenceTotals(int keypointA, int keypointB, int type, int connectorCount,int typeImprovement)
        {
            KeypointA = keypointA;
            KeypointB = keypointB;
            Type = type;
            ConnectorCount = connectorCount;
            TypeImprovement = typeImprovement;
        }

        public int KeypointA { get; set; }
        public int KeypointB { get; set; }

        public int Type { get; set; }

        public int ConnectorCount { get; set; }

        public int TypeImprovement { get; set; }

        public void KeypointAInc()
        {
            KeypointA++;
        }

        public void KeypointBInc()
        {
            KeypointB++;
        }

        public void TypeInc()
        {
            Type++;
        }

        public void ConnectorCountInc()
        {
            ConnectorCount++;
        }

        public void TypeImprovementInc()
        {
            TypeImprovement++;
        }

        static public JointRunDifferenceTotals operator + (JointRunDifferenceTotals jrdt1,JointRunDifferenceTotals jrdt2)
        {
            return new JointRunDifferenceTotals(jrdt1.KeypointA + jrdt2.KeypointA, jrdt1.KeypointB + jrdt2.KeypointB, jrdt1.Type + jrdt2.Type, jrdt1.ConnectorCount + jrdt2.ConnectorCount,jrdt1.TypeImprovement + jrdt2.TypeImprovement);

        }
    
    }
}
