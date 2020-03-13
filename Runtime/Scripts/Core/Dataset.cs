using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Recon {
    
    public class Dataset<T> {
        bool _changed = true;
        List<T> _list = new List<T> ();
        Dictionary<T, int> _map = new Dictionary<T, int>();

        public int Add(T vision) {
            _changed = true;

            var i = _list.Count;
            _list.Add (vision);
            _map [vision] = i;
            return i;
        }
        public int Remove(T vision) {
            var i = _list.IndexOf (vision);
            if (i >= 0) {
                _changed = true;
                _list.RemoveAt (i);
                _map.Remove (vision);
            }
            return i;
        }
        public bool Reset() {
            var curr = _changed;
            _changed = false;
            return curr;
        }
        public bool TryGetIndex(T item, out int index) {
            return _map.TryGetValue (item, out index);
        }
        public int Count {
            get { return _list.Count; }
        }
        public T this[int index] {
            get { return _list [index]; }
        }
        public IList<T> GetList() {
            return _list;
        }
    }
}
