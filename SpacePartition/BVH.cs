using nobnak.Gist;
using nobnak.Gist.Extensions.AABB;
using nobnak.Gist.Extensions.Range;
using nobnak.Gist.Pooling;
using nobnak.Gist.Primitive;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recon.SpacePartition {

    public class BVH<Value> where Value : class {

        public readonly BVH<Value>[] ch = new BVH<Value>[2];
        public readonly LinkedList<Value> Values = new LinkedList<Value>();

		public FastBounds bb;
        public int offset, length;

        public BVH<Value> Reset(int offset, int length) {
            this.offset = offset;
            this.length = length;
			return this;
		}
        public BVH<Value> SetChildren(BVH<Value> l, BVH<Value> r) {
            ch [0] = l; 
            ch [1] = r;
            return this;
        }
        public BVH<Value> Clear() {
            System.Array.Clear (ch, 0, ch.Length);
            Values.Clear ();
            return this;
        }
		public bool IsLeaf() {
			return ch [0] == null && ch [1] == null;
		}
        public FastBounds Build(IReadOnlyList<FastBounds> bounds, IReadOnlyList<Value> values) {
            if (IsLeaf ()) {
                bb = bounds.Range (offset, length).Encapsulate ();
                foreach (var v in values.Range (offset, length))
                    Values.AddLast (v);
            } else
                bb.Encapsulate (ch.Where (t => t != null).Select (t => t.Build (bounds, values)).Encapsulate ());
            return bb;
        }

        #region Static
        public static IMemoryPool<BVH<Value>> Clear(BVH<Value> t, IMemoryPool<BVH<Value>> alloc) {
            if (t == null)
                return alloc;

            for (var i = 0; i < 2; i++)
                Clear (t.ch [i], alloc);
            return alloc.Free (t.Clear());
        }
        #endregion

        #region Gizmo
        public static void DrawBounds(BVH<Value> t, int depth, int length) {
            if (t == null || depth >= length)
                return;

            Gizmos.DrawWireCube (t.bb.Center, t.bb.Size);

            foreach (var s in t.ch.Where (s => s != null))
                DrawBounds (s, depth + 1, length);
        }
        #endregion
    }
}
