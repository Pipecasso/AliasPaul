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

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            string manifest = args[0];
            string podpath = args[1];

            Bitmap canvas = new Bitmap(1000, 400);


            PODTransformer podTransformer = null;
            LoadedPod lp = new LoadedPod(manifest, podpath);
            IsogenAssemblyLoader ial = lp.isogenAssemblyLoader;

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                podTransformer = new PODTransformer(lp.pod);
            }
            Dictionary<dynamic, Shapes3d> podshapes = podTransformer.Shapes3;
            Dictionary<dynamic, Shapes2d> projectedshapes = new Dictionary<dynamic, Shapes2d>();
            CubeView cubeView = podTransformer.GetCube();
            Point3d center = cubeView.Center;
            Point3d frl = cubeView.FrontTopLeft;
            Vector3d normal = new Vector3d(frl, center);
            normal.Normalise();
            Camera camera = new Camera(normal, cubeView, 5, 2, canvas.Width, canvas.Height);
            foreach (KeyValuePair<dynamic, Shapes3d> kvp in podshapes)
            {
                Shapes2d shapes2d = camera.ProjectMyShapes(kvp.Value);
                projectedshapes.Add(kvp.Key, shapes2d);
            }

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                PODCanvas podCanvas = new PODCanvas(canvas);
                PODArtist picasso = new PODArtist(lp.pod, podCanvas, projectedshapes);
            }
        }
    }
}
