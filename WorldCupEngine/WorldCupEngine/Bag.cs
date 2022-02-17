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

        public int Count
        {
            get { return _items.Count; }
        }

        public bool Empty
        {
            get { return !_items.Any<T>(); }
        }

        public void Add(T t)
        {
            _items.Add(t);
        }

        public IEnumerable<T> Contents
        {
            get
            {
                return _items;
            }
        }

        public T Take()
        {
            T togo = default(T);
            if (_items.Count == 1)
            {
                togo = _items.First<T>();
            }
            else
            {
                int r = _randy.Next(1, _items.Count+1);
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
            }
            _items.Remove(togo);
            return togo;
        }
    }
}
