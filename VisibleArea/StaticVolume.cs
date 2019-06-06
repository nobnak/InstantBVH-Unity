using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;

namespace Recon.VisibleArea {

	[ExecuteInEditMode]
    public class StaticVolume : Volume {
		[SerializeField]
		protected Transform rootBone = null;
		[SerializeField]
		protected Bounds localBounds = new Bounds(Vector3.zero, Vector3.one);

        #region implemented abstract members of IConvex
        public override bool StartConvex () {
			return true;
		}
		#endregion

		#region implemented abstract members of AbstractMeshOBB
		protected override Transform RootTransform () {
            return (rootBone == null ? transform : rootBone);
        }
        protected override Bounds LocalBounds () {
            return localBounds;
        }
		#endregion

		#region member
		#endregion
	}
}
