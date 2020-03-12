using nobnak.Gist;
using nobnak.Gist.Extensions.AABB;
using nobnak.Gist.Extensions.Range;
using nobnak.Gist.Pooling;
using nobnak.Gist.Primitive;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recon.Core {

    public class Node<V> {

        public readonly Node<V>[] ch = new Node<V>[2];
        public readonly LinkedList<V> Values = new LinkedList<V>();

		public FastBounds bb;
        public int offset, length;

        public Node<V> Reset(int offset, int length) {
            this.offset = offset;
            this.length = length;
			return this;
		}
        public Node<V> SetChildren(Node<V> l, Node<V> r) {
            ch [0] = l; 
            ch [1] = r;
            return this;
        }
        public Node<V> Clear() {
            System.Array.Clear (ch, 0, ch.Length);
            Values.Clear ();
            return this;
        }
		public bool IsLeaf() {
			return ch [0] == null && ch [1] == null;
		}
        public FastBounds Recalculate(IReadOnlyList<IVolume<V>> volumes) {
            if (IsLeaf ()) {
                bb = volumes.Range(offset, length).Select(v => v.Bounds).Encapsulate ();
                foreach (var v in volumes.Range (offset, length).Select(v => v.Value))
                    Values.AddLast (v);
            } else
                bb.Encapsulate (ch.Where (c => c != null)
					.Select (c => c.Recalculate (volumes))
					.Encapsulate ());
            return bb;
        }

        #region Static
        public static IMemoryPool<Node<V>> Clear(Node<V> t, IMemoryPool<Node<V>> alloc) {
            if (t == null)
                return alloc;

            for (var i = 0; i < 2; i++)
                Clear (t.ch [i], alloc);
            return alloc.Free (t.Clear());
        }
        #endregion

        #region Gizmo
        public static void DrawBounds(Node<V> t, int depth, int length) {
            if (t == null || depth >= length)
                return;

            Gizmos.DrawWireCube (t.bb.Center, t.bb.Size);

            foreach (var s in t.ch.Where (s => s != null))
                DrawBounds (s, depth + 1, length);
        }
        #endregion
    }
}
