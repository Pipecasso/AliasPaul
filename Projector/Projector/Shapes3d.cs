using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;

namespace Projector
{
    public class Shapes3d
    {
        private List<Line3d> _Lines;
        private List<Cone3d> _Cones;

        public Shapes3d()
        {
            _Lines = new List<Line3d>();
            _Cones = new List<Cone3d>();
        }

        public List<Line3d> Lines { get => _Lines; }
        public List<Cone3d> Cones { get => _Cones; }
    
    }
}
