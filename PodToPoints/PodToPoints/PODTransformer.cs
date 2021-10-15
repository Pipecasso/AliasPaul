using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using Intergraph.PersonalISOGEN;
using AliasPOD;
using AliasPOD3D;
using Projector;
using AliasGeometry;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace PodToPoints
{
    public class PODTransformer
    {
        private ModelNode _modelNode;
        private Dictionary<string, Component> _uidcompmap;
        private Dictionary<dynamic, Shapes3d> _Shapes;
        private Dictionary<Tuple<Pipeline, int>, string> _spoolIdsBySequence;


        public PODTransformer(POD pod)
        {
            _uidcompmap = new Dictionary<string, Component>();
            _spoolIdsBySequence = new Dictionary<Tuple<Pipeline, int>, string>();
            Traverse(pod);
        }



        public PODTransformer(string szManifest, string szPOD)
        {
            _uidcompmap = new Dictionary<string, Component>();
            _spoolIdsBySequence = new Dictionary<Tuple<Pipeline, int>, string>();
            _Shapes = new Dictionary<dynamic, Shapes3d>();
            LoadedPod loadedPod = new LoadedPod(szManifest, szPOD);


            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(loadedPod.isogenAssemblyLoader))
            {
                Traverse(loadedPod.pod);
            }
        }

        public void Traverse(POD pod)
        {
            SymbolicModelBuilder symbolicModelBuilder = new SymbolicModelBuilder();
            _modelNode = symbolicModelBuilder.BuildModel(pod);
            _Shapes = new Dictionary<dynamic, Shapes3d>();

            TraverseModel(_modelNode);

            foreach (Pipeline pipeline in pod.Pipelines)
            {
                foreach (Component component in pipeline.Components)
                {
                    _uidcompmap.Add(component.UID, component);
                }

                foreach (InformationElement info in pipeline.InformationElements)
                {
                    if (info.Type == "Spool-Summary")
                    {
                        if (!info.Attributes.IsItem("SEQUENCE-NUMBER") || !info.Attributes.IsItem("IDENTIFIER")) continue;
                        int sequenceNumber = info.Attributes.Item("SEQUENCE-NUMBER").Value;
                        string identifier = info.Attributes.Item("IDENTIFIER").Value;
                        _spoolIdsBySequence.Add(System.Tuple.Create(pipeline, sequenceNumber), identifier);
                    }
                }
            }
        }

        public void LoadFromXls(string filepath)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filepath,false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                IEnumerable<Worksheet> worksheets = workbookPart.WorksheetParts.Select(x => x.Worksheet);
                IEnumerable<SheetData> sheetDataAll = worksheets.SelectMany(x => x.Elements<SheetData>());
                SheetData sheetData = sheetDataAll.Where(x => x.InnerXml != null && x.InnerXml != string.Empty).First();
                SharedStringTablePart stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            }
        }

        public void SaveToXls(string filepath)
        {
            Dictionary<Line3d, string> Lines = new Dictionary<Line3d, string>();
            foreach (KeyValuePair<dynamic, Shapes3d> kvp in _Shapes)
            {
                Shapes3d shapes = kvp.Value;
                string uci = kvp.Key.ExternalUCI;
                foreach (Line3d line in shapes.Lines)
                {
                    if (!Lines.ContainsKey(line)) { Lines.Add(line, uci); }
                }
            }
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                Sheet line_sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Lines"
                };
                sheets.Append(line_sheet);

            }
         
         
            

        }

      


        public Dictionary<dynamic,Shapes3d> Shapes3 { get => _Shapes; }

        public Dictionary<Tuple<Pipeline,int>,string> SpoolIdsBySequence { get => _spoolIdsBySequence; }

        private void TraverseModel(ModelNode node)
        {
            AliasPOD3D.Primitives primatives = node.Geometry;
          

            try
            {

            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {

            }
            
            foreach (ModelNode child in node.Children)
            {
                TraverseModel(child);
            }

            if (primatives.Count > 0)
            {
                double dx, dy, dz;
                Shapes3d Shapes = new Shapes3d();
               
                foreach (IPrimitive iprim in primatives)
                {
                    AliasPOD3D.PrimitiveType pt = iprim.Type;
                    switch (pt)
                    {
                        case AliasPOD3D.PrimitiveType.ptLine:

                            AliasPOD3D.Line iline = (AliasPOD3D.Line)iprim;
                            iline.GetStart(out dx, out dy, out dz);
                            Point3d p1 = new Point3d(dx, dy, dz);
                            iline.GetEnd(out dx, out dy, out dz);
                            Point3d p2 = new Point3d(dx, dy, dz);
                         
                            Line3d whatsmyline = new Line3d(p1, p2);
                            Shapes.Lines.Add(whatsmyline);
                            break;

                        case AliasPOD3D.PrimitiveType.ptCone:
                            Cone coneyisland = (Cone)iprim;
                            string uid = node.Object.uid;
                            Cone3d cone3 = MakeCorrectedCone(coneyisland ,uid);
                            Shapes.Cones.Add(cone3);
                            break;

                        default:
                            int ooooh = 0;
                            break;

                    }
                }

                _Shapes.Add(node.Object, Shapes);
            }
        }

        Cone3d MakeCorrectedCone(Cone c, string uid)
        {
            double dx, dy, dz;
            c.GetStart(out dx, out dy, out dz);
            Point3d ptStart = new Point3d(dx, dy, dz);
      //      System.Diagnostics.Debug.WriteLine("Circle Start {0} {1} {2}", dx, dy, dz);
            c.GetEnd(out dx, out dy, out dz);
            Point3d ptEnd = new Point3d(dx, dy, dz);
        //    System.Diagnostics.Debug.WriteLine("Circle End {0} {1} {2}", dx, dy, dz);
            Point3d ptMid = Point3d.MidPoint(ptStart, ptEnd);
        //    System.Diagnostics.Debug.WriteLine("Circle Mid {0} {1} {2}", ptMid.X, ptMid.Y, ptMid.Z);
            double conelength = Point3d.Distance(ptStart, ptEnd);
            Vector3d vConeLine = null;
            Vector3d vExistingConeLine = new Vector3d(ptStart, ptEnd);
            vExistingConeLine.Normalise();

            bool FoundConeDir = false;
            if (_uidcompmap.ContainsKey(uid))
            {
                Component cpent = _uidcompmap[uid];
                if (cpent.Material.Configuration == AliasPOD.eConfigurationType.eCTInline)
                {
                    ComponentLeg cl = cpent.Legs.Item(0);
                    if (cl.IsDirectionValid() == true)
                    {
                        cl.GetDirection(out dx, out dy, out dz);
                        vConeLine = new Vector3d(dx, dy, dz);
                        FoundConeDir = true;
                    }
                }
            }
            if (!FoundConeDir)
            {
                vConeLine = vExistingConeLine;
            }

            double check = Vector3d.Dot(vConeLine, vExistingConeLine);

          //  System.Diagnostics.Debug.WriteLine("New Cone Line {0},{1},{2}", vConeLine.X, vConeLine.Y, vConeLine.Z);
            Cone3d conetogo = new Cone3d(ptMid, conelength, vConeLine, c.StartDiameter, c.EndDiameter);
           // System.Diagnostics.Debug.WriteLine("Circle Start New {0} {1} {2}", conetogo.circleStart.Center.X, conetogo.circleStart.Center.Y, conetogo.circleStart.Center.Z);
            //System.Diagnostics.Debug.WriteLine("Circle Start End {0} {1} {2}", conetogo.circleEnd.Center.X, conetogo.circleEnd.Center.Y, conetogo.circleEnd.Center.Z);
            return conetogo;
        }

        public CubeView GetCube()
        {
            CubeView cv = new CubeView(GetPoints());
            return cv;
        }

        public List<Point3d> GetPoints()
        {
            List<Point3d> cubePoints = new List<Point3d>();
            foreach (KeyValuePair<dynamic, Shapes3d> kvshapes in _Shapes)
            {
                Shapes3d shapes = kvshapes.Value;
                cubePoints.AddRange(shapes.Points());
            }
            return cubePoints;

        }


    }
}
