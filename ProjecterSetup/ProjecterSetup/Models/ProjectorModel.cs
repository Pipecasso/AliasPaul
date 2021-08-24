using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;
using Projector;

namespace ProjecterSetup.Models
{
    public class ProjectorModel
    {       
        public CubeView Cube { get; set; }
        public Vector3d Normal { get; set; }
        double Div { get; set; }
        double Mult { get; set; }

    
    
    }
}
