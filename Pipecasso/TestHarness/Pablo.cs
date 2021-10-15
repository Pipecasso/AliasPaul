using AliasGeometry;
using AliasPOD;
using Intergraph.PersonalISOGEN;
using ManagedPodLoader;
using PODPainter;
using Projector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    public class Pablo
    {
        private string _dir;
        private string _podname;
        private Dictionary<dynamic, Shapes3d> _podshapes;

        private Dictionary<dynamic, Tuple<Pen, Brush>> _pencilcase;
   
        private LoadedPod _loadedPod;
        private CubeView _cubeView;

        public Pablo(string manfiest, string podpath, Dictionary<dynamic, Shapes3d> PodShapes, CubeView cubeView, int width, int height)
        {
            _dir = Path.GetDirectoryName(podpath);
            _podname = Path.GetFileNameWithoutExtension(podpath);

            _loadedPod = new LoadedPod(manfiest, podpath);

            _podshapes = PodShapes;
            Width = width;
            Height = height;

            _pencilcase = new Dictionary<dynamic, Tuple<Pen, Brush>>();

            Pen shaun1 = new Pen(Color.AliceBlue, 4);
            Pen shaun2 = new Pen(Color.OliveDrab, 3);
            Pen shaun3 = new Pen(Color.Lime, 5);
            Pen shaun4 = new Pen(Color.White, 3);

            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(_loadedPod.isogenAssemblyLoader))
            {
                foreach (Pipeline pline in _loadedPod.pod.Pipelines)
                {
                    foreach (Component c in pline.Components)
                    {
                        if (c.Material.Group == "Valves")
                        {
                            _pencilcase.Add(c, new Tuple<Pen, Brush>(shaun1, null));
                        }
                        else if (c.Material.Group == "Flanges")
                        {
                            _pencilcase.Add(c, new Tuple<Pen, Brush>(shaun2, null));
                        }
                        else if (c.Material.Group == "Welds" || c.Material.Group == "Bolts")
                        {
                            _pencilcase.Add(c, new Tuple<Pen, Brush>(shaun3, null));
                        }
                        else
                        {
                            _pencilcase.Add(c, new Tuple<Pen, Brush>(shaun4, null));
                        }
                    }
                }
            }
            _cubeView = cubeView;

        }

        public string Project(Vector3d normal,string tag)
        {
            Dictionary<dynamic, Shapes2d> ProjectedShapes = new Dictionary<dynamic, Shapes2d>();
            Camera camera = new Camera(normal, _cubeView, 5, 2, Width, Height);
            foreach (KeyValuePair<dynamic, Shapes3d> kvp in _podshapes)
            {
                Shapes2d shapes2d = camera.ProjectMyShapes(kvp.Value);
                ProjectedShapes.Add(kvp.Key, shapes2d);
                foreach (Cone2d cone in shapes2d.Cones)
                {
                    Ellipse2dPointByPoint e = cone.start;
                    double angle = e.AngleToHorizontal;
                    double degangle = angle * 180 / Math.PI;
                }
            }

            string xlname = string.Concat(_podname, tag,".xlsx");
            string xlpath = Path.Combine(_dir, xlname);
            string bitname = string.Concat(_podname, tag, ".bmp");
            string bitpath = Path.Combine(_dir, bitname);
            XLSProjectionWriter xLSProjectionWriter = new XLSProjectionWriter(_podshapes, ProjectedShapes, xlpath);
            xLSProjectionWriter.Go();

            Bitmap canvas = new Bitmap(Width, Height);
            PODCanvas podCanvas = new PODCanvas(canvas);
            PODArtist picasso = new PODArtist(_loadedPod.pod, podCanvas, ProjectedShapes);
            picasso.DrawIt(_pencilcase);
            picasso.SaveIt(bitpath);
            return bitpath;
        }


        public int Width { get; set; }
        public int Height { get; set; }
    
    
    }
}
