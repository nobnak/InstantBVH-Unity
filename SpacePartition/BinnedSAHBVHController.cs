using nobnak.Gist;
using nobnak.Gist.Intersection;
using nobnak.Gist.Pooling;
using nobnak.Gist.Scoped;
using System.Collections.Generic;
using UnityEngine;

namespace Recon.SpacePartition {

    public class BinnedSAHBVHController<Value> : BaseBVHController<Value> where Value : class {
        public const int K = 8;

        protected IMemoryPool<AABB3> boundsPool;
        protected List<int> indices;
        protected BinnedSAH sah;

        protected List<AABB3> objectBounds;

        public BinnedSAHBVHController() {
            this.boundsPool = AABB3.CreateAABBPool();
            this.indices = new List<int>();
            this.sah = new BinnedSAH(boundsPool);
            this.objectBounds = new List<AABB3>();
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

            using (new ScopedPlug<List<AABB3>>(objectBounds, obs => MemoryPoolUtil.Free(obs, boundsPool))) {
                foreach (var b in bounds)
                    objectBounds.Add(b);

                if ((_root = Build(objectBounds, indices, 0, indices.Count, sah, _pool)) != null)
                    _root.Build(new IndexedList<Bounds>(indices, bounds), new IndexedList<Value>(indices, dataset));
            }

            return this;
        }

        public static BVH<Value> Build(IList<AABB3> bounds, IList<int> indices, int offset, int length, 
            BinnedSAH sah, IMemoryPool<BVH<Value>> alloc) {
            if (length <= 0)
                return null;

            int countFromLeft;
            if (length <= 2 || !sah.Build(bounds, indices, offset, length, out countFromLeft))
                return alloc.New().Reset(offset, length);

            var l = Build(bounds, indices, offset, countFromLeft, sah, alloc);
            var r = Build(bounds, indices, offset + countFromLeft, length - countFromLeft, sah, alloc);
            if (l != null ^ r != null)
                return (l != null) ? l : r;
            return alloc.New().Reset(offset, length).SetChildren(l, r);
        }
    }
}
