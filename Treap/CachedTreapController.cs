using UnityEngine;
using System.Collections.Generic;

namespace Reconnitioning.Treap {

    public class CachedTreapController<Value> : ITreapAlloc<Value>, ITreapController<Value> {
        public Treap<Value> Root { get { return _root; } }

        Treap<Value> _root;
        Stack<Treap<Value>> _cache = new Stack<Treap<Value>>();

        #region ITreapController implementation

        public bool TryGet (int key, out Treap<Value> n) {
            return Treap<Value>.TryGet (_root, key, out n);
        }
        public Treap<Value> Insert (int key) {
            return Treap<Value>.Insert (ref _root, key, this);
        }

        #endregion

        #region ITreapAlloc implementation
        public float Priority () {
            return Random.value;
        }
        public Treap<Value> Create (int key) {
            if (_cache.Count > 0)
                return _cache.Pop ();
            return new Treap<Value> (key, Priority ());
        }
        public void Destroy (Treap<Value> t) {
            if (t == null)
                return;
            
            t.Clear ();
            _cache.Push (t);

            for (var i = 0; i < 2; i++)
                Destroy (t.ch [i]);
        }
        #endregion
    }
}