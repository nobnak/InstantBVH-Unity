using Gist.Pooling;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Recon.Treap {

    public class TreapEditorTest {
    	[Test]
        public void EditorTest() {
            Treap<int> root = null;
            var alloc = new MemoryPool<Treap<int>> (()=>new Treap<int>(),(v)=>v.Clear(), (v)=> { });

            var n = 4;
            var numbers = new int[n];
            var rands = new float[n];
            for (var i = 0; i < n; i++) {
                numbers [i] = i;
                rands [i] = Random.value;
            }
            System.Array.Sort (rands, numbers);

            for (var i = 0; i < n; i++)
                Treap<int>.Insert (ref root, (ulong)i, alloc);

            var heap = Heapup<int>(new List<int> (), root);
            for (var i = 0; i < heap.Count; i++)
                Assert.AreEqual (i, heap [i]);

            for (var i = 0; i < n; i++) {
                Treap<int> t;
                Assert.True(Treap<int>.TryGet(root, (ulong)i, out t));
                t.Values.AddLast (i);
            }

            for (var i = 0; i < n; i++) {
                Treap<int> t;
                Treap<int>.TryGet (root, (ulong)i, out t);
                Assert.AreEqual (1, t.Values.Count);
                Assert.AreEqual (i, t.Values.Last.Value);
            }
    	}

        static List<int> Heapup<Value>(List<int> l, Treap<Value> t) {
            if (t == null)
                return l;

            Heapup (l, t.ch [0]);
            l.Add ((int)t.key);
            Heapup (l, t.ch [1]);

            return l;
        }
    }
}