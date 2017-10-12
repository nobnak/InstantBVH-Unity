using UnityEngine;
using System.Collections;
using Gist;
using Gist.Extensions.AABB;
using Recon.Treap;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gist.BoundingVolume;
using Gist.Scoped;

namespace Recon.SpacePartition {

    public class BinnedSAHBVHController<Value> : BaseBVHController<Value> where Value : class {
        public const int K = 8;

        protected IMemoryPool<AABB> boundsPool;
        protected List<int> indices;
        protected BinnedSAH sah;

        protected List<AABB> objectBounds;

        public BinnedSAHBVHController() {
            this.boundsPool = AABB.CreateAABBPool();
            this.indices = new List<int>();
            this.sah = new BinnedSAH(boundsPool);
            this.objectBounds = new List<AABB>();
        }

        public override BaseBVHController<Value> Clear() {
            base.Clear();
            indices.Clear();
            return this;
        }

        public override BaseBVHController<Value> Build(IList<Bounds> bounds, IList<Value> dataset) {
            Clear ();

            sah.Clear();
            for (var i = 0; i < bounds.Count; i++)
                indices.Add(i);

            using (new ScopedPlug<List<AABB>>(objectBounds, obs => MemoryPoolUtil.Free(obs, boundsPool))) {
                foreach (var b in bounds)
                    objectBounds.Add(b);

                if ((_root = Build(objectBounds, indices, 0, indices.Count, sah, _pool)) != null)
                    _root.Build(new IndexedList<Bounds>(indices, bounds), new IndexedList<Value>(indices, dataset));
            }

            return this;
        }

        public static BVH<Value> Build(IList<AABB> bounds, IList<int> indices, int offset, int length, 
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
