using UnityEngine;
using System.Collections;
using Gist;
using Reconnitioning.Treap;
using System.Collections.Generic;
using System.Text;

namespace Reconnitioning.SpacePartition {

    public class BVHController<Value> where Value : class {
        public BVH<Value> Root { get { return _root; } }

        BVH<Value> _root;
        MemoryPool<BVH<Value>> _pool = new MemoryPool<BVH<Value>> ();
        List<int> _indices = new List<int>();
        List<int> _ids = new List<int>();
        List<Bounds> _bounds = new List<Bounds>();
        List<Value> _values = new List<Value>();

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

            _bounds.Clear ();
            _values.Clear ();
            for (var i = 0; i < _indices.Count; i++) {
                _bounds.Add (Bous [_indices [i]]);
                _values.Add (Vals [_indices [i]]);
            }
            _root.Build (_bounds, _values);

            return this;
        }

        #region Static
        public static IList<int> Swap (IList<int> list, int i, int j) {
            var tmp = list [i];
            list [i] = list [j];
            list [j] = tmp;
            return list;
        }
        public static BVH<Value> Compress(BVH<Value> t, IMemoryPool<BVH<Value>> alloc) {
            for (var i = 0; i < 2; i++) {
                if (t.ch [i] != null && t.ch [1 - i] == null) {
                    var s = t.ch [i];
                    alloc.Free (t);
                    return s;
                }
            }
            return t;
        }
        public static BVH<Value> Sort(IList<int> indices, IList<int> ids, int offset, int length, IMemoryPool<BVH<Value>> alloc, int height) {
            if (length <= 0 || height <= 0)
                return null;

            var left = offset;
            var right = offset + length;
            while (left < right) {
                var id = ids[indices [left]];
                var bit = id & (1 << (height-1));
                if (bit != 0)
                    Swap (indices, left, --right);
                else
                    left++;
            }

            var leftLen = left - offset;
            var t = alloc.New ().Reset (offset, length);
            t.ch [0] = Sort (indices, ids, offset, leftLen, alloc, --height);
            t.ch [1] = Sort (indices, ids, left, length - leftLen, alloc, height);
            return Compress (t, alloc);
        }
        #endregion
    }
}
