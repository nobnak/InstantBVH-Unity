using UnityEngine;
using System.Collections;
using Gist;
using Gist.Extensions.AABB;
using Recon.Treap;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gist.BoundingVolume;

namespace Recon.SpacePartition {

    public class BinnedSAHBVHController<Value> : BaseBVHController<Value> where Value : class {
        public const int K = 8;

        protected IMemoryPool<AABB> pool;
        protected List<int> indices;
        protected BinnedSAH sah;

        public BinnedSAHBVHController() {
            this.pool = AABB.CreateAABBPool();
            this.indices = new List<int>();
            this.sah = new BinnedSAH(pool);
        }

        public override BaseBVHController<Value> Clear() {
            base.Clear();
            indices.Clear();
            return this;
        }

        public override BaseBVHController<Value> Build(IList<Bounds> Bous, IList<Value> Vals) {
            Clear ();

            sah.Clear();
            for (var i = 0; i < Bous.Count; i++)
                indices.Add(i);

            _root = Build(Bous, indices, 0, indices.Count, sah, _pool);

            return this;
        }

        public static BVH<Value> Build(IList<Bounds> bounds, IList<int> indices, int offset, int length, 
            BinnedSAH sah, IMemoryPool<BVH<Value>> alloc) {
            if (length <= 0)
                return null;

            if (length <= 2)
                return alloc.New().Reset(offset, length);

            int countFromLeft;
            sah.Build(bounds, indices, offset, length, out countFromLeft);

            var l = Build(bounds, indices, offset, countFromLeft, sah, alloc);
            var r = Build(bounds, indices, offset + countFromLeft, length - countFromLeft, sah, alloc);
            if (l != null ^ r != null)
                return (l != null) ? l : r;
            return alloc.New().Reset(offset, length).SetChildren(l, r);
        }
    }
}
