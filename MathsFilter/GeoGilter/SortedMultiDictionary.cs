using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoFilter
{
    public class SortedMultiDictionary<K,T> :IEnumerable<K>
    {
        private SortedDictionary<K,List<T>> _dictionary;

        public SortedMultiDictionary()
        {
            _dictionary = new SortedDictionary<K,List<T>>();
            
        }

        public void Add(K k,T t)
        {
            if (_dictionary.ContainsKey(k))
            {
                _dictionary[k].Add(t);
            }  
            else
            {
                List<T> list = new List<T>() { t };
                _dictionary.Add(k, list);
            }
        }

        public IEnumerator<K> GetEnumerator()
        {
            foreach (KeyValuePair<K,List<T>> kvp in _dictionary)
            {
                foreach(T t in kvp.Value)
                {
                    yield return kvp.Key;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
