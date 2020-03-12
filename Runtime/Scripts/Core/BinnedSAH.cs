using nobnak.Gist;
using nobnak.Gist.Intersection;
using nobnak.Gist.Pooling;
using nobnak.Gist.Scoped;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recon.Core {

    public class BinnedSAH<Value> : BaseBVH<Value> {
        public const float SMALLER_THAN_ONE = 0.999f;
        public const int DEFAULT_K = 8;
		public const int K = 8;
		
		protected List<int> indices;

		protected List<AABB3> objectBounds;

		protected int binCount = 0;
        protected AABB3[] bbs;
        protected List<int>[] bins;

        protected float[] lefts;
        protected float[] rights;

        protected bool cleared;

        protected IMemoryPool<AABB3> aabbPool;

		public BinnedSAH(int binCount, IMemoryPool<AABB3> aabbPool) {
            this.aabbPool = aabbPool;
			this.indices = new List<int>();
			this.objectBounds = new List<AABB3>();
			Reset(binCount);
        }
        public BinnedSAH(IMemoryPool<AABB3> aabbPool) : this(DEFAULT_K, aabbPool) { }

		#region interface

		#region IDisposable
		public override void Dispose() {
			base.Dispose();
			MemoryPoolUtil.Free(bbs, aabbPool);
		}
		#endregion
		#region core
		public void ClearForBuild() {
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
		public bool Build(IList<AABB3> objectBounds, IList<int> indices, int offset, int length, out int countFromLeft) {
			ClearForBuild();
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
		#endregion

		public override BaseBVH<Value> Clear() {
			base.Clear();
			indices.Clear();
			return this;
		}
		public override BaseBVH<Value> Update() {
			Clear();

			using (new ScopedPlug<List<AABB3>>(objectBounds, obs => MemoryPoolUtil.Free(obs, aabbPool))) {
				for (var i = 0; i < volumes.Count; i++) {
					var v = volumes[i];
					var aabb = aabbPool.New().Set(v.Bounds);
					objectBounds.Add(aabb);
					indices.Add(i);
				}
				if ((_root = BuildHierachy(objectBounds, indices, 0, indices.Count, _pool)) != null)
					_root.Recalculate(new IndexedList<IVolume<Value>>(indices, volumes));
			}

			return this;
		}
		public Node<Value> BuildHierachy(
			IList<AABB3> bounds, IList<int> indices, int offset, int length,
			IMemoryPool<Node<Value>> alloc) {
			if (length <= 0)
				return null;

			int countFromLeft;
			if (length <= 2 || !Build(bounds, indices, offset, length, out countFromLeft))
				return alloc.New().Reset(offset, length);

			var l = BuildHierachy(bounds, indices, offset, countFromLeft, alloc);
			var r = BuildHierachy(bounds, indices, offset + countFromLeft, length - countFromLeft, alloc);
			if (l != null ^ r != null)
				return (l != null) ? l : r;
			return alloc.New().Reset(offset, length).SetChildren(l, r);
		}
		#endregion

	}
}
