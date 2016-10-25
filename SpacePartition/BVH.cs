using UnityEngine;
using System.Collections.Generic;
using Reconnitioning.Treap;
using Gist;

namespace Reconnitioning.SpacePartition {
    
	public class BVH<Value> where Value : class {
        public readonly BVH<Value>[] ch = new BVH<Value>[2];

		public Bounds bb;
        public int offset, length;
        public IList<Value> Values;

        public BVH<Value> Reset(int offset, int length) {
            this.offset = offset;
            this.length = length;
			return this;
		}
        public BVH<Value> Clear() {
            System.Array.Clear (ch, 0, ch.Length);
            Values.Clear ();
            return this;
        }
		public bool Leaf() {
			return ch [0] == null && ch [1] == null;
		}
        public BVH<Value> Single() {
            return (ch [0] == null ? ch [1] : ch [0]);
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
