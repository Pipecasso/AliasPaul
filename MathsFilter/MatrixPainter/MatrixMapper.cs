using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;

namespace MatrixPainter
{
    internal class MatrixMapper
    {
        internal enum Source { red, green, blue, all };
        internal Dictionary<Source,TransformMatrix> _mmaps;
    
        internal MatrixMapper()
        {
            _mmaps = new Dictionary<Source, TransformMatrix>();
        }

        internal bool IsValidMap()
        {
            return _mmaps.Any() &&
               ((_mmaps.ContainsKey(Source.all) && _mmaps.Count==1) || (_mmaps.ContainsKey(Source.all)==false));
        }

    }
}
