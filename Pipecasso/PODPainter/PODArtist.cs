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
        private IsogenAssemblyLoader _isogenAssemblyLoader;
        private POD _pod;
        
        public PODArtist(LoadedPod loadedPod,PODCanvas podCanvas, Dictionary<dynamic, Shapes2d> paintthis) : base(paintthis)
        {
            _isogenAssemblyLoader = loadedPod.isogenAssemblyLoader;
            _pod = loadedPod.pod;
        }
    
    
    }
}
