using UnityEngine;
using System.Collections.Generic;
using Reconnitioning.Treap;
using Gist;

namespace Reconnitioning.SpacePartition {
    
	public class BVH<Value> where Value : class {
        public static readonly Bounds NULL_BOUNDS = new Bounds ();

        static BVH() {
            NULL_BOUNDS.min = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
            NULL_BOUNDS.max = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
        }

        public readonly BVH<Value>[] ch = new BVH<Value>[2];
        public readonly LinkedList<Value> Values = new LinkedList<Value>();

		public Bounds bb;
        public int offset, length;

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
		public bool IsLeaf() {
			return ch [0] == null && ch [1] == null;
		}
        public Bounds Build(IList<Bounds> bounds, IList<Value> values) {
            bb = bounds [offset];

            if (IsLeaf ()) {
                for (var i = 0; i < length; i++) {
                    bb.Encapsulate (bounds [offset + i]);
                    Values.AddLast (values [offset + i]);
                }
            } else {
                for (var i = 0; i < 2; i++)
                    if (ch [i] != null)
                        bb.Encapsulate (ch [i].Build (bounds, values));
            }
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
