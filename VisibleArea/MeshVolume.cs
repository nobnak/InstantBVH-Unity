using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;

namespace Recon.VisibleArea {

    public class MeshVolume : Volume {
		MeshFilter _mfilter;
        OBB _obb;

        #region implemented abstract members of Volume
        public override IConvexPolyhedron GetConvexPolyhedron () {
            ConvUp.AssureUpdateConvex ();
            return _obb;
        }
        public override bool StartConvex () {
			return _mfilter != null || (_mfilter = GetComponentInChildren<MeshFilter> ()) != null;
        }
        public override bool UpdateConvex () {
			return (_obb = OBB.Create (_mfilter.transform, _mfilter.sharedMesh.bounds)) != null;
        }
        #endregion
    }
}
