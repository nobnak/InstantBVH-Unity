using UnityEngine;
using System.Collections.Generic;
using Gist;

namespace Reconnitioning.Treap {
    
    public class Treap<Value> {
        public ulong key;
        public float pri;

        public readonly Treap<Value>[] ch = new Treap<Value>[2];
        public readonly LinkedList<Value> Values = new LinkedList<Value>();

        public Treap<Value> Reset(ulong key, float pri) {
            this.key = key;
            this.pri = pri;
            return this;
        }
        public Treap<Value> Clear() {
            System.Array.Clear (ch, 0, ch.Length);
            Values.Clear ();
            return this;
        }

        #region Static
        public static float Priority() {
            return Random.value;
        }
        public static bool TryGet(Treap<Value> t, ulong key, out Treap<Value> n) {
            n = t;
            if (t == null)
                return false;
            if (t.key == key)
                return true;

            var dir = (key < t.key ? 0 : 1);
            return TryGet(t.ch [dir], key, out n);
        }
        public static Treap<Value> Insert(ref Treap<Value> t, ulong key, IMemoryPool<Treap<Value>> alloc) {
            if (t == null)
                return t = alloc.New ().Reset (key, Priority ());
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
        public static IMemoryPool<Treap<Value>> Clear(Treap<Value> t, IMemoryPool<Treap<Value>> alloc) {
            if (t == null)
                return alloc;

            for (var i = 0; i < 2; i++)
                Clear (t.ch [i], alloc);
            return alloc.Free (t.Clear());
        }
        #endregion
    }
}
