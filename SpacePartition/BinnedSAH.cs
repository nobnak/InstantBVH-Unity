using Gist.BoundingVolume;
using Gist.Pooling;
using System.Collections.Generic;
using System.Linq;

namespace Recon.SpacePartition {

    public class BinnedSAH : System.IDisposable {
        public const float SMALLER_THAN_ONE = 0.999f;
        public const int DEFAULT_K = 8;

        protected int binCount = 0;
        protected AABB[] bbs;
        protected List<int>[] bins;

        protected float[] lefts;
        protected float[] rights;

        protected bool cleared;

        protected IMemoryPool<AABB> aabbPool;

        public BinnedSAH(int binCount, IMemoryPool<AABB> aabbPool) {
            this.aabbPool = aabbPool;
            Reset(binCount);
        }
        public BinnedSAH(IMemoryPool<AABB> aabbPool) : this(DEFAULT_K, aabbPool) { }

        public bool Build(IList<AABB> objectBounds, IList<int> indices, int offset, int length, out int countFromLeft) {
            Clear();
            cleared = false;

            var world = aabbPool.New();
            foreach (var j in Enumerable.Range(offset, length).Select(i => indices[i])) { 
                var ob = objectBounds[j];
                world.Encapsulate(ob.Center);
            }

            var size = world.Size;
            var longestAxis = (size.x > size.z ? (size.x > size.y ? 0 : 1) : (size.y > size.z ? 1 : 2));
            var longest = size[longestAxis];
            var worldMin = world.Min[longestAxis];
            if (longest < 1e-4f) {
                countFromLeft = -1;
                return false;
            }

            var invLongest = 1f / longest;
            foreach (var j in Enumerable.Range(offset, length).Select(i => indices[i])) { 
                var ob = objectBounds[j];
                var c = ob.Center;
                var k = (int)(binCount * (c[longestAxis] - worldMin) * invLongest * SMALLER_THAN_ONE);
                bins[k].Add(j);
                bbs[k].Encapsulate(ob);
            }

            var leftCount = 0;
            var leftBounds = aabbPool.New();
            for (var i = 0; i < lefts.Length; i++) {
                leftCount += bins[i].Count;
                leftBounds.Encapsulate(bbs[i]);
                lefts[i] = leftCount * leftBounds.SurfaceArea;
            }

            var rightCount = 0;
            var rightBounds = aabbPool.New();
            for (var i = 0; i < rights.Length; i++) {
                var j = binCount - i - 1;
                rightCount += bins[j].Count;
                rightBounds.Encapsulate(bbs[j]);
                rights[j] = rightCount * rightBounds.SurfaceArea;
            }

            var bestIndex = -1;
            var bestCost = float.MaxValue;
            for (var i = 1; i < binCount; i++) {
                var cost = lefts[i - 1] + rights[i];
                if (cost < bestCost) {
                    bestCost = cost;
                    bestIndex = i;
                }
            }

            countFromLeft = 0;
            for (var i = 0; i < bestIndex; i++)
                countFromLeft += bins[i].Count;

            var offsetOfIndices = offset;
            foreach (var bin in bins)
                foreach (var i in bin)
                    indices[offsetOfIndices++] = i;

            aabbPool.Free(world);
            aabbPool.Free(leftBounds);
            aabbPool.Free(rightBounds);

            return true;
        }

        public void Reset(int nextBinCount) {
            if (binCount == nextBinCount)
                return;
            var prevBinCount = binCount;
            binCount = nextBinCount;
            cleared = false;

            if (bbs != null)
                MemoryPoolUtil.Free(bbs, aabbPool);
            System.Array.Resize(ref bbs, nextBinCount);
            for (var i = 0; i < bbs.Length; i++)
                bbs[i] = aabbPool.New();

            System.Array.Resize(ref bins, nextBinCount);
            for (var i = prevBinCount; i < nextBinCount; i++)
                bins[i] = new List<int>();

            System.Array.Resize(ref lefts, nextBinCount);
            System.Array.Resize(ref rights, nextBinCount);
        }

        public void Clear() {
            if (cleared)
                return;
            cleared = true;

            for (var i = 0; i < binCount; i++) {
                bins[i].Clear();
                bbs[i].Clear();
            }

            System.Array.Clear(lefts, 0, lefts.Length);
            System.Array.Clear(rights, 0, rights.Length);
        }

        public void Dispose() {
            MemoryPoolUtil.Free(bbs, aabbPool);
        }

    }
}
