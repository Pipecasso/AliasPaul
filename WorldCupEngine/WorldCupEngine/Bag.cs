using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupEngine
{
    public class Bag<T>
    {
        private HashSet<T> _items;
        private Random _randy;

        public Bag()
        {
            _items = new HashSet<T>();
            _randy = new Random();
        }

        public void Fill(IEnumerable<T> things)
        {
            foreach (T t in things)
            {
                Add(t);
            }
        }

        public void Add(T t)
        {
            _items.Add(t);
        }

        public T Take()
        {
            T togo = default(T);
            int r = _randy.Next(1, _items.Count);
            int tick = 1;
            foreach (T t in _items)
            {
                if (tick == r)
                {
                    togo = t;
                    break;
                }
                else
                {
                    tick++;
                }
            }
            _items.Remove(togo);
            return togo;
        }
    }
}
