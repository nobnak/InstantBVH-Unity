using UnityEngine;
using System.Collections.Generic;
using Gist;

namespace Reconnitioning.Treap {

    public class CachedTreapController<Value> : ITreapController<Value> {
        public Treap<Value> Root { get { return _root; } }

        Treap<Value> _root;
        MemoryPool<Treap<Value>> _pool = new MemoryPool<Treap<Value>>();

        #region ITreapController implementation
        public bool TryGet (ulong key, out Treap<Value> n) {
            return Treap<Value>.TryGet (_root, key, out n);
        }
        public Treap<Value> Insert (ulong key) {
            return Treap<Value>.Insert (ref _root, key, _pool);
        }
        public ITreapController<Value> Clear() {
            Root.Clear ();
            return this;
        }
        #endregion
    }
}