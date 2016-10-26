using UnityEngine;
using System.Collections.Generic;
using Reconnitioning.Treap;
using System.Linq;
using Gist;
using Gist.Extensions.AABB;
using Gist.Extensions.Range;

namespace Reconnitioning.SpacePartition {
    
	public class BVH<Value> where Value : class {

        public readonly BVH<Value>[] ch = new BVH<Value>[2];
        public readonly LinkedList<Value> Values = new LinkedList<Value>();

		public Bounds bb;
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
        public Bounds Build(IReadOnlyList<Bounds> bounds, IReadOnlyList<Value> values) {
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
            return alloc.Free (t);
        }
        #endregion
    }
}
