using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using Painter;
using Projector;
using System.Drawing;
using Intergraph.PersonalISOGEN;
using AliasPOD;

namespace PODPainter
{
    public class PODArtist : Artist
    {
        private POD _pod;
        
        public PODArtist(AliasPOD.POD pod,PODCanvas podCanvas, Dictionary<dynamic, Shapes2d> paintthis) : base(paintthis)
        {
  
            _pod = pod;
        }
    
    
    }
}
