using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFilter
{ 
    public class ChunkyIntList : SortedMultiList<int>
    {
        private int _lower;
        private int _upper;
        private uint _under;
        private uint _over;

        private Dictionary<Tuple<int, int>, int> _chunks;
        private Dictionary<int, Tuple<int, int>> _chunkcache;

        public ChunkyIntList(int lower,int upper,int chunktotal)
        {
            _lower = lower;
            _upper = upper;
            _under = 0;
            _over = 0;
            _chunks = new Dictionary<Tuple<int, int>, int>();
            _chunkcache = new Dictionary<int, Tuple<int, int>>();
            int rem;
            int chunksize = Math.DivRem(upper - lower, chunktotal, out rem);
            int chunklow = lower;
            for (int i=0;i<chunktotal;i++)
            {
                int chunkup = chunklow + chunksize;
                if (rem>0)
                {
                    chunkup++;
                    rem--;
                }
                Tuple<int, int> chunk = new Tuple<int, int>(chunklow, chunkup);
                chunklow = chunkup + 1;
                _chunks.Add(chunk, 0);
              /*  for (int j=chunk.Item1;j<=chunk.Item2;j++)
                {
                    _chunkcache.Add(j, chunk);
                }*/

            }


        }

        public uint Under { get => _under; }
        public uint Over { get => _over; }

        public new void Add(int value)
        {
            base.Add(value);
            if (value < _lower)
            {
                _under++;
            }
            else if (value > _upper)
            {
                _over++;
            }
            else
            {
                Tuple<int, int> chunk = GetChunk(value);
                _chunks[chunk]++;
            }
        }

        public new void Remove(int value)
        {
            base.Remove(value);
            if (value < _lower)
            {
                _under--;
            }
            else if (value > _upper)
            {
                _over--;
            }
            else
            {
                Tuple<int, int> chunk = GetChunk(value);
                _chunks[chunk]--;
            }
        }

        private Tuple<int, int> GetChunk(int val)
        {
            Tuple<int, int> chunk;
            if (_chunkcache.ContainsKey(val))
            {
                chunk = _chunkcache[val];
            }
            else
            {
                chunk = _chunks.Keys.Where(x => x.Item1 < val && x.Item2 > val).Single();
                _chunkcache[val] = chunk;
            }
            return chunk;
        }
    }
}
