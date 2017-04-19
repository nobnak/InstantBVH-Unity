using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;

namespace Recon.VisibleArea {

    public class SkinVolume : Volume {
        SkinnedMeshRenderer _skin;
        OBB _obb;

        #region implemented abstract members of Volume
        public override IConvexPolyhedron GetConvexPolyhedron () {
            ConvUp.AssureUpdateConvex ();
            return _obb;
        }
        public override bool StartConvex () {
            return _skin != null || (_skin = GetComponentInChildren<SkinnedMeshRenderer> ()) != null;
        }
        public override bool UpdateConvex () {
            return (_obb = OBB.Create (_skin.RootBone(), _skin.localBounds)) != null;
        }
        #endregion
    }
}
