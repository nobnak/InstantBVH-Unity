using UnityEngine;
using System.Collections.Generic;

namespace Reconnitioning {
        
    public class Treap<Value> {
        public int key;
        public float pri;

        public readonly Treap<Value>[] ch = new Treap<Value>[2];
        public readonly LinkedList<Value> Values = new LinkedList<Value>();

        ITreapAlloc _alloc;

        public Treap(int key, ITreapAlloc alloc) {
            this.key = key;
            this.pri = alloc.Priority ();
            this._alloc = alloc;
        }

        public static bool TryGet(Treap<Value> t, int key, out Treap<Value> n) {
            n = t;
            if (t == null)
                return false;
            if (t.key == key)
                return true;

            var dir = (key < t.key ? 0 : 1);
            return TryGet(t.ch [dir], key, out n);
        }
        public static Treap<Value> Insert(ref Treap<Value> t, int key, ITreapAlloc alloc) {
            if (t == null)
                return t = alloc.Create (key);
            var dir = (key < t.key ? 0 : 1);
            var n = Insert (ref t.ch [dir], key, alloc);
            if (t.pri < t.ch [dir].pri)
                Rotate (ref t, 1 - dir);
            return n;
        }
        public static Treap<Value> Rotate(ref Treap<Value> t, int dir) {
            var s = t.ch [1 - dir];
            t.ch [1 - dir] = s.ch [dir];
            s.ch [dir] = t;
            return t = s;
        }

        #region Classes
        public interface ITreapAlloc {
            float Priority();
            Treap<Value> Create(int key);
            void Destroy (Treap<Value> t);
        }
        public class SimpleTreapAlloc : ITreapAlloc {
            public SimpleTreapAlloc() {}

            #region ITreapAlloc implementation
            public float Priority () { return Random.value; }
            public Treap<Value> Create (int key) {
                return new Treap<Value> (key, this);
            }
            public void Destroy (Treap<Value> t) {}
            #endregion
        }
        #endregion
    }
}
