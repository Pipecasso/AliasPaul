using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GeoFilter
{
    public class BagOfBags
    {
        private Dictionary<GeoPixel, PixelBag> _PixelBags;
        private VectorBox _box;
        private HashSet<GeoPixel> _todo;
        private List<PixelBag> _BagList;

        public BagOfBags(GeoPixel seed, VectorBox box, double theta, Func<VectorBox, GeoPixel, GeoPixel, double,bool> funky)
        {
            _box = box;
            _todo = new HashSet<GeoPixel>();
            _PixelBags = new Dictionary<GeoPixel, PixelBag>();
            _BagList = new List<PixelBag>();

            //populate to do
            for (int ii = 0;ii<box.Width;ii++)
            {
                for (int jj=0;jj<box.Height;jj++)
                {
                    GeoPixel gp = new GeoPixel(ii, jj);
                    _todo.Add(gp);
                }

            }

            Random randy = new Random();
            PixelBag pb = Bagit(seed,theta, funky,_todo) ;
            _BagList.Add(pb);
            while (_todo.Count > 0)
            {
                Dictionary<GeoPixel,ColourVector> gp = pb.Pixels;
                foreach (KeyValuePair<GeoPixel, ColourVector> kvp in gp)
                {
                    _todo.Remove(kvp.Key);
                    _PixelBags.Add(kvp.Key, pb);
                }

                if (_todo.Count > 0)
                {
                    int iR = randy.Next(0, _todo.Count - 1);
                    int iTick = 0;
                    foreach (GeoPixel geop in _todo)
                    {
                        seed = geop;
                        if (_todo.Count > 50)
                        {
                            if (iR == iTick) break;
                            iTick++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    pb = Bagit(seed, theta, funky, _todo);
                    _BagList.Add(pb);
                }
              
            }
        }

        public Bitmap Process()
        {
            VectorBox vb = _box.Clone();
            foreach (PixelBag pb in _BagList)
            {
                foreach (GeoPixel gp in pb.ClosedBorders)
                {
                    vb[gp] = new ColourVector(Color.Black);
                }
            }
            BitmapBox bitbox = new BitmapBox(vb);
            return bitbox.bitmap;
        }

        private PixelBag Bagit(GeoPixel p,double theta, Func<VectorBox, GeoPixel, GeoPixel, double, bool> funky, HashSet<GeoPixel> todo)
        {
            PixelBag pb = new PixelBag(p, _box, theta, funky,todo);
            return pb;
        }
    }
}
