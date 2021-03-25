using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasPOD;

namespace KeypointRunner
{
    class KeypointRunItems : List<KeypointRunItem>
    {
        private KeypointRunItem _Anchor;
        public KeypointRunItems(KeypointRunItem anchor)
        {
            _Anchor = anchor;
        }

        public void Sort3D()
        {
            CompareToAnchor3d compareToAnchor = new CompareToAnchor3d(_Anchor);
            this.Sort(compareToAnchor);
        }

        public void Sort2D()
        {
            CompareToAnchor2d compareToAnchor = new CompareToAnchor2d(_Anchor);
            this.Sort(compareToAnchor);
        }

        public KeypointRunItem Anchor { get => _Anchor; }

    }
}
