using UnityEngine;
using System.Collections;
using Gist;
using Reconnitioning.Treap;
using System.Collections.Generic;

namespace Reconnitioning.SpacePartition {

    public class BVHController<Value> where Value : class {
        public BVH<Value> Root { get { return _root; } }

        BVH<Value> _root;
        MemoryPool<BVH<Value>> _pool = new MemoryPool<BVH<Value>> ();
        List<int> _indices = new List<int>();
        List<int> _ids = new List<int>();

        public BVHController<Value> Clear() {
            BVH<Value>.Clear (_root, _pool);
            return this;
        }

        public BVHController<Value> Build(IList<Bounds> Bous, IList<Value> Vals) {
            var min = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
            for (var i = 0; i < Bous.Count; i++) {
                var p = Bous [i].center;
                for (var j = 0; j < 3; j++) {
                    min [j] = Mathf.Min (min [j], p [j]);
                    max [j] = Mathf.Max (max [j], p [j]);
                }
            }

            var size = new Vector3 (max.x - min.x, max.y - min.y, max.z - min.z);
            var sizeInv = new Vector3 (1f / size.x, 1f / size.y, 1f / size.z);

            _indices.Clear ();
            _ids.Clear ();
            for (var i = 0; i < Bous.Count; i++) {
                var bb = Bous [i];
                var p = Vector3.Scale (bb.center - min, sizeInv);
                var id = MortonCodeInt.Encode (p.x, p.y, p.z);
                _indices.Add (i);
                _ids.Add (id);
            }

            Clear ();
            _root = Sort (_indices, _ids, 0, _indices.Count, _pool, MortonCodeInt.STRIDE_BITS);

            return this;
        }

        #region Static
        public static IList<int> Swap (IList<int> indices, int i, int j) {
            var tmp = indices [i];
            indices [i] = indices [j];
            indices [j] = tmp;
            return indices;
        }
        public static BVH<Value> Sort(IList<int> indices, IList<int> ids, int offset, int length, IMemoryPool<BVH<Value>> alloc, int height) {
            if (length <= 0 || height <= 0)
                return null;

            var left = offset;
            var right = offset + length;
            while (left < right) {
                var i = indices [left];
                var id = ids [i];
                var bit = id & (1 << (height-1));
                if (bit > 0)
                    Swap (indices, left, --right);
                else
                    left++;
            }

            var t = alloc.New ().Reset (offset, length);
            t.ch [0] = Sort (indices, ids, offset, left, alloc, --height);
            t.ch [1] = Sort (indices, ids, offset + left, length - left, alloc, height);
            return t;
        }
        #endregion
    }
}
