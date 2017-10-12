using UnityEngine;
using System.Collections;
using Gist;
using Gist.Extensions.AABB;
using Recon.Treap;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Recon.SpacePartition {

    public abstract class BaseBVHController<Value> where Value : class {
        public BVH<Value> Root { get { return _root; } }

        protected BVH<Value> _root;
        protected MemoryPool<BVH<Value>> _pool = new MemoryPool<BVH<Value>> (
            ()=>new BVH<Value>(), (v)=>v.Clear(), (v)=> { });

        public virtual BaseBVHController<Value> Clear() {
            BVH<Value>.Clear (_root, _pool);
            return this;
        }
        public abstract BaseBVHController<Value> Build(IList<Bounds> Bous, IList<Value> Vals);

        public virtual IEnumerable<Value> Intersect(Bounds bb) {
            return Intersect (_root, bb);
        }
        public virtual int Count() { return Count(_root); }
        public virtual int CountValues() {
            return CountValues (_root);
        }

        #region Static
        public static int Count(BVH<Value> t) {
            if (t == null)
                return 0;
            return 1 + Count (t.ch [0]) + Count (t.ch [1]);
        }
        public static int CountValues(BVH<Value> t) {
            return t == null ? 0 : (t.Values.Count + CountValues (t.ch [0]) + CountValues (t.ch [1]));
        }
        public static IEnumerable<Value> Intersect(BVH<Value> t, Bounds bb) {
            if (t == null || !t.bb.Intersects (bb))
                yield break;
            
            foreach (var s in t.ch)
                foreach (var r in Intersect(s, bb))
                    yield return r;

            foreach (var v in t.Values)
                if (v != null)
                    yield return v;

        }
        public static IList<int> Swap (IList<int> list, int i, int j) {
            var tmp = list [i];
            list [i] = list [j];
            list [j] = tmp;
            return list;
        }
        #endregion
        #region Gizmo
        public virtual void DrawBounds(int depthFrom, int length) {
            BVH<Value>.DrawBounds (_root, -depthFrom, length);
        }
        #endregion
    }
}
