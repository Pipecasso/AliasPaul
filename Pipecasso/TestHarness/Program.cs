using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using PodToPoints;
using Projector;
using AliasGeometry;
using System.IO;
using PODPainter;
using System.Drawing;
using Intergraph.PersonalISOGEN;
using Painter;
using AliasPOD;

namespace TestHarness
{
    class Program
    {

        Action<Canvas, Shapes2d, Tuple<Pen, Brush>> PaintThis1 = (canvas, shapes, penbrush) => 
        { 
            //to do we need a collection of shapes that in in system drawing form...
        
        };
        
        

        static void Main(string[] args)
        {
            string manifest = args[0];
            string podpath = args[1];

      
            PODTransformer podTransformer = null;
            LoadedPod lp = new LoadedPod(manifest, podpath);
            IsogenAssemblyLoader ial = lp.isogenAssemblyLoader;

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                podTransformer = new PODTransformer(lp.pod);
            }
            Dictionary<dynamic, Shapes3d> podshapes = podTransformer.Shapes3;
            CubeView cubeView = podTransformer.GetCube();
        

            Dictionary<string, Vector3d> Normals = new Dictionary<string, Vector3d>();
            Normals.Add("down",new Vector3d(0, 0, -1));
            Normals.Add("up", new Vector3d(0, 0,  1));
            Normals.Add("east",new Vector3d(1, 0, 0));
            Normals.Add("west",new Vector3d(-1, 0, 0));
            Normals.Add("south",new Vector3d(0, -1, 0));
            Normals.Add("north",new Vector3d(0, 1, 0));

            Vector3d vDir = new Vector3d(cubeView.FrontTopLeft, cubeView.Center);
            vDir.Normalise();
            Normals.Add("ftl", vDir);
            vDir *= -1;
            Normals.Add("bbr", vDir);

            vDir = new Vector3d(cubeView.FrontTopRight, cubeView.Center);
            vDir.Normalise();
            Normals.Add("ftr", vDir);
            vDir *= -1;
            Normals.Add("bbl", vDir);

            vDir = new Vector3d(cubeView.FrontBottomLeft, cubeView.Center);
            vDir.Normalise();
            Normals.Add("fbl", vDir);
            vDir *= -1;
            Normals.Add("btr", vDir);

            vDir = new Vector3d(cubeView.FrontBottomRight, cubeView.Center);
            vDir.Normalise();
            Normals.Add("fbr", vDir);
            vDir *= -1;
            Normals.Add("btl", vDir);

            Pablo picasso = new Pablo(manifest, podpath, podshapes, cubeView, 1000, 400);

            foreach (KeyValuePair<string,Vector3d> kvp in Normals)
            {
                picasso.Project(kvp.Value, kvp.Key);
            }   
        }
    }
}
