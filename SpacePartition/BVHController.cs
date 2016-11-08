using UnityEngine;
using System.Collections;
using Gist;
using Gist.Extensions.AABB;
using Recon.Treap;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Recon.SpacePartition {

    public class BVHController<Value> where Value : class {
        public BVH<Value> Root { get { return _root; } }

        BVH<Value> _root;
        MemoryPool<BVH<Value>> _pool = new MemoryPool<BVH<Value>> ();
        List<int> _indices = new List<int>();
        List<int> _ids = new List<int>();

        public BVHController<Value> Clear() {
            BVH<Value>.Clear (_root, _pool);
            _indices.Clear ();
            _ids.Clear ();
            return this;
        }
        public BVHController<Value> Build(IList<Bounds> Bous, IList<Value> Vals) {
            Clear ();

            var galaxy = Bous.Select (b => b.center).Encapsulate ();
            var min = galaxy.min;
            var size = galaxy.size;
            var sizeInv = new Vector3 (1f / size.x, 1f / size.y, 1f / size.z);

            for (var i = 0; i < Bous.Count; i++) {
                var bb = Bous [i];
                var p = Vector3.Scale (bb.center - min, sizeInv);
                var id = MortonCodeInt.Encode (p.x, p.y, p.z);
                _indices.Add (i);
                _ids.Add (id);
            }

            _root = Sort (_indices, _ids, 0, _indices.Count, _pool, MortonCodeInt.STRIDE_BITS);
            if (_root != null)
                _root.Build (new IndexedList<Bounds> (_indices, Bous), new IndexedList<Value> (_indices, Vals));

            return this;
        }
        public IEnumerable<Value> Intersect(Bounds bb) {
            return Intersect (_root, bb);
        }

        #region Static
        public static IEnumerable<Value> Intersect(BVH<Value> t, Bounds bb) {
            if (t == null || !t.bb.Intersects (bb))
                yield break;
            
            foreach (var i in t.ch)
                foreach (var j in Intersect(i, bb))
                    yield return j;

            foreach (var i in t.Values)
                if (i != null)
                    yield return i;

        }
        public static IList<int> Swap (IList<int> list, int i, int j) {
            var tmp = list [i];
            list [i] = list [j];
            list [j] = tmp;
            return list;
        }
        public static BVH<Value> Sort(IList<int> indices, IList<int> ids, int offset, int length, IMemoryPool<BVH<Value>> alloc, int height) {
            if (length <= 0 || height <= 0)
                return null;

            var ileft = offset;
            var iright = offset + length;
            while (ileft < iright) {
                var id = ids[indices [ileft]];
                var bit = id & (1 << (height-1));
                if (bit != 0)
                    Swap (indices, ileft, --iright);
                else
                    ileft++;
            }

            var leftLen = ileft - offset;
            var l = Sort (indices, ids, offset, leftLen, alloc, --height);
            var r = Sort (indices, ids, ileft, length - leftLen, alloc, height);
            if (l != null ^ r != null)
                return (l != null ? l : r);
            return alloc.New ().Reset (offset, length).SetChildren (l, r);
        }
        #endregion
        #region Gizmo
        public void DrawBounds(int depthFrom, int length) {
            BVH<Value>.DrawBounds (_root, -depthFrom, length);
        }
        #endregion
    }
}
