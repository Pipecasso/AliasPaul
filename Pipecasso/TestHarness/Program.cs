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
            //podTransformer.SaveToXls("p:\\here.xlsx");
            //podTransformer.LoadFromXls("here.xlsx");

            Camera camera = new Camera(normal, cubeView, 5, 2, canvas.Width, canvas.Height);
            foreach (KeyValuePair<dynamic, Shapes3d> kvp in podshapes)
            {
                Shapes2d shapes2d = camera.ProjectMyShapes(kvp.Value);
                projectedshapes.Add(kvp.Key, shapes2d);
                
                
                foreach (Cone2d cone in shapes2d.Cones)
                {
                    Ellipse2dPointByPoint e = cone.start;
                    double angle = e.AngleToHorizontal;
                    double degangle = angle * 180 / Math.PI;
                }
            }

            /*Pen shaun1 = new Pen(Color.AliceBlue, 4);
            Pen shaun2 = new Pen(Color.OliveDrab, 3);
            Pen shaun3 = new Pen(Color.Lime, 5);
            Pen shaun4 = new Pen(Color.White, 3);

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                Dictionary<dynamic, Tuple<Pen, Brush>> pencilcase = new Dictionary<dynamic, Tuple<Pen, Brush>>();
                foreach (Pipeline pline in lp.pod.Pipelines)
                {
                    foreach (Component c in pline.Components)
                    {
                        if (c.Material.Group == "Valves")
                        {
                            pencilcase.Add(c, new Tuple<Pen, Brush>(shaun1, null));
                        }
                        else if (c.Material.Group == "Flanges")
                        {
                            pencilcase.Add(c, new Tuple<Pen, Brush>(shaun2, null));
                        }
                        else if (c.Material.Group == "Welds" || c.Material.Group == "Bolts")
                        {
                            pencilcase.Add(c, new Tuple<Pen, Brush>(shaun3, null));
                        }
                        else
                        {
                            pencilcase.Add(c, new Tuple<Pen, Brush>(shaun4,null));
                        }
                    }
                }


                
                
                
                
                PODCanvas podCanvas = new PODCanvas(canvas);
                PODArtist picasso = new PODArtist(lp.pod, podCanvas, projectedshapes);
                picasso.DrawIt(pencilcase);
                picasso.SaveIt("Pipecasso2021.bmp");
            }*/
        }
    }
}
