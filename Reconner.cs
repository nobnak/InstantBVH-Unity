using nobnak.Gist.Intersection;
using nobnak.Gist.Primitive;
using Recon.Core;
using Recon.VisibleArea;
using System.Collections.Generic;
using UnityEngine;

namespace Recon {
	[ExecuteAlways]
	[DefaultExecutionOrder(-1000)]
    public class Reconner : MonoBehaviour {
		protected BaseBVH<Volume> _bvh = new BinnedSAH<Volume>(AABB3.CreateAABBPool());

		#region unity
		void Update() {
            _bvh.Update ();
        }
        void OnDrawGizmos() {
            if (_bvh == null)
                return;
            Gizmos.color = gizmoColorBounds;
            _bvh.DrawBounds (0, 10);
        }
		#endregion

		#region interface
		public Color gizmoColorBounds = Color.green;
        public BaseBVH<Volume> BVH { get { return _bvh; } }

        public void Add(Volume vol) {
            _bvh.Add (vol);
        }
        public void Remove(Volume vol) {
            _bvh.Remove (vol);
        }

		#region Intersection
		protected List<Volume> tmpList = new List<Volume>();

        public readonly System.Func<Volume, bool> Pass = v => true;
        public IEnumerable<Volume> Find(IConvex3Polytope conv) { return Find(conv, Pass, Pass); }
        public IEnumerable<Volume> Find(IConvex3Polytope conv, System.Func<Volume, bool> NarrowFilter) {
            return Find(conv, NarrowFilter, Pass);
        }
        public IEnumerable<Volume> Find(IConvex3Polytope conv,
            System.Func<Volume, bool> NarrowFilter, System.Func<Volume, bool> BroadFilter) {
            var r = this;
            BaseBVH<Volume> bvh;
            if (r == null || (bvh = r.BVH) == null)
                yield break;

            var bb = conv.WorldBounds ();
			tmpList.Clear();
            foreach (var v in Broadphase(bvh, bb, tmpList))
                if (BroadFilter (v) && v.GetConvexPolyhedron ().Intersect (conv) && NarrowFilter (v))
                    yield return v;
        }
		public IEnumerable<Volume> Broadphase(BaseBVH<Volume> bvh, FastBounds bb) {
			foreach (var v in bvh.Intersect(bb))
				yield return v;
		}
		public IList<Volume> Broadphase(BaseBVH<Volume> bvh, FastBounds bb, IList<Volume> result) {
			return bvh.Intersect(bb, result);
		}
		public IEnumerable<Volume> NarrowPhase(IConvex3Polytope conv, IEnumerable<Volume> broadphased) {
            foreach (var v in broadphased)
                if (v.GetConvexPolyhedron().Intersect(conv))
                    yield return v;
        }
		#endregion

		#endregion
    }
}
