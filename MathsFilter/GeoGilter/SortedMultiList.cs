using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeoFilter
{
    public class SortedMultiList<K> : ICollection<K>
    {
        private SortedDictionary<K, int> _dictionary;
       
        public SortedMultiList()
        {
            _dictionary = new SortedDictionary<K, int>();
        }

        public int Count => _dictionary.Values.Sum();

        public bool IsReadOnly => false;

        public void Add(K item)
        {
            if (_dictionary.ContainsKey(item))
            {
                _dictionary[item]++;
            }
            else
            {
                _dictionary.Add(item, 1);
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(K item)
        {
            return _dictionary.ContainsKey(item);
        }

        public void CopyTo(K[] array, int arrayIndex)
        {
            foreach (K k in this)
            {
                array[arrayIndex++] = k;
            }
        }

        public IEnumerator<K> GetEnumerator()
        {
            foreach (KeyValuePair<K, int> pair in _dictionary)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    yield return pair.Key;
                }
            }
        }

        public bool Remove(K item)
        {
            bool removed;
            if (_dictionary.ContainsKey(item))
            {
                removed = true;
                _dictionary[item]--;
                if (_dictionary[item] == 0) _dictionary.Remove(item);
            }
            else
            {
                removed = false;
            }
            return removed;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public K First
        {
            get => _dictionary.FirstOrDefault().Key;
        }

        public K Last
        {
            get => _dictionary.LastOrDefault().Key;
        }
    }

}
