using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GeoFilter
{
    public class PixelBag
    {
        private Dictionary<GeoPixel, ColourVector> _Pixels;
        private Queue<GeoPixel> _OpenBorders;
        private HashSet<GeoPixel> _ClosedBorders;
        private HashSet<GeoPixel> _BagNeighbours;
        private GeoPixel _origin;
       


        Func<VectorBox, GeoPixel, GeoPixel, double> Distance = (vbox, p, q) =>
        {
            ColourVector cvp = vbox[p];
            ColourVector cvq = vbox[q];
            DenseVector diff = cvp - cvq;
            double mag = diff.L2Norm();
            return mag;

        };


        public PixelBag(GeoPixel origin, VectorBox box, double theta, Func<VectorBox, GeoPixel, GeoPixel, double, bool> include, HashSet<GeoPixel> todo = null, bool xdump = false)
        {
            _origin = origin;
            Worksheet xlWorkSheet1 = null;
            Application xlApp = null;
            Workbook xlWorkBook = null;

            if (xdump)
            {
                 xlApp = new Microsoft.Office.Interop.Excel.Application();

               
                object misValue = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet1 = (Worksheet)xlWorkBook.ActiveSheet;
            }


            Dictionary<GeoPixel, GeoPixel> whoaddedwho = new Dictionary<GeoPixel, GeoPixel>();
            _OpenBorders = new Queue<GeoPixel>();
            _ClosedBorders = new HashSet<GeoPixel>();
            _BagNeighbours = new HashSet<GeoPixel>();
            _Pixels = new Dictionary<GeoPixel, ColourVector>();
            _Pixels.Add(origin, box[origin]);
           
            Dictionary<GeoPixel, int> ActiveChildren = new Dictionary<GeoPixel, int>();
            HashSet<GeoPixel> ActivePixels = new HashSet<GeoPixel>();

            _OpenBorders.Enqueue(origin);
            ActivePixels.Add(origin);
            ProcessPixel(true, whoaddedwho, ActiveChildren, ActivePixels, box, theta, include,todo, xdump, xlWorkSheet1);

            while (_OpenBorders.Count >0)
            {
                ProcessPixel(false, whoaddedwho, ActiveChildren, ActivePixels, box, theta, include,todo, xdump, xlWorkSheet1) ;
            }

            if (xdump)
            {
                xlApp.ActiveWorkbook.SaveAs("Edge.xlsx");
                xlWorkBook.Close();
                xlApp.Quit();

                Marshal.ReleaseComObject(xlApp);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlWorkSheet1);
            }

        }

        private void ProcessPixel( bool bFirst, Dictionary<GeoPixel, GeoPixel> whoaddedwho, Dictionary<GeoPixel, int> ActiveChildren, HashSet<GeoPixel> ActivePixels, VectorBox box, double theta, Func<VectorBox, GeoPixel, GeoPixel, double, bool> include,HashSet<GeoPixel> todo,bool xdump, Worksheet xlWorkSheet1)
        {
            GeoPixel p = _OpenBorders.Dequeue();
            int irx = p.x - _origin.x;
            int iry = p.y - _origin.y;
        
            bool bBorderPixel = false;

            List<GeoPixel> GoodNeighbours = GetNeighbours(p);

            int activecount = 0;
            GeoPixel Parent = null;
            if (!bFirst)
            {
                Parent = whoaddedwho[p];
                activecount = ActiveChildren[Parent];
                activecount--;
                if (activecount > 0) ActiveChildren[Parent] = activecount;
                whoaddedwho.Remove(p);
            }
          

            int iAdded = 0;
            foreach (GeoPixel pn in GoodNeighbours)
            {
                int irx2 = pn.x - _origin.x;
                int iry2 = pn.y - _origin.y;
                if (todo == null)
                {
                    if (ActivePixels.Contains(pn)) continue;
                }
                else
                {
                    if (ActivePixels.Contains(pn) || !todo.Contains(pn)) continue;
                }

                //0,0 gets through a second time??? why???????

                //double do it for diagnostic purpo
                if (xdump && Math.Abs(irx2) < 50 && Math.Abs(iry2) < 50)
                {
                    //xlWorkSheet1.Cells[iry2 + 51, irx2 + 51].Font.Color = dTest < theta ? Color.Black : Color.IndianRed;
                    double dTest = Distance(box, pn, _origin);
                    int x1 = iry2 + 51;
                    int y1 = irx2 + 51;
                    xlWorkSheet1.Cells[x1, y1].Font.Color = dTest < theta ? Color.Black : Color.IndianRed;
                    xlWorkSheet1.Cells[x1, y1] = dTest;


                }

                if (include(box, _origin, pn, theta))
                {
                    _Pixels.Add(pn, box[pn]);
                    _OpenBorders.Enqueue(pn);
                    whoaddedwho[pn] = p;
                    ActivePixels.Add(pn);
                    iAdded++;
                }
                else
                {
                    if (!_BagNeighbours.Contains(pn)) _BagNeighbours.Add(pn);
                    if (!bBorderPixel) bBorderPixel = true;
                }
            }
            if (iAdded > 0) ActiveChildren.Add(p, iAdded);

            if (bBorderPixel && !_ClosedBorders.Contains(p))
            {
                _ClosedBorders.Add(p);
            }

            if (activecount == 0 && Parent != null)
            {
                ActiveChildren.Remove(Parent);
                ActivePixels.Remove(Parent);
            }

        }

        public HashSet<GeoPixel> ClosedBorders
        {
            get
            {
                return _ClosedBorders;
            }       
        }

        public HashSet<GeoPixel> BagNeighbours
        {
            get
            {
                return _BagNeighbours;
            }
        }

        public Dictionary<GeoPixel, ColourVector> Pixels
        {
            get
            {
                return _Pixels;
            }
        }

        List<GeoPixel> GetNeighbours(GeoPixel p)
        {
            //st george
            List<GeoPixel> Neighbours = new List<GeoPixel>();
            GeoPixel q = new GeoPixel(p.x - 1, p.y);
            Neighbours.Add(q);
            q = new GeoPixel(p.x + 1, p.y);
            Neighbours.Add(q);
            q = new GeoPixel(p.x, p.y - 1);
            Neighbours.Add(q);
            q = new GeoPixel(p.x, p.y + 1);
            Neighbours.Add(q);


            //st Andrew  
            q = new GeoPixel(p.x + 1, p.y + 1);
            Neighbours.Add(q);
 
            q = new GeoPixel(p.x - 1, p.y - 1);
            Neighbours.Add(q);
      
            q = new GeoPixel(p.x + 1, p.y - 1);
            Neighbours.Add(q);
           
            q = new GeoPixel(p.x - 1, p.y + 1);
            Neighbours.Add(q);
       


            return Neighbours;


        }






    }
}
