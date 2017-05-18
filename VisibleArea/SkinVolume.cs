using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;

namespace Recon.VisibleArea {

    public class SkinVolume : Volume {
        SkinnedMeshRenderer _attachedSkinmesh;

        #region implemented abstract members of IConvex
        public override bool StartConvex () {
            return (_attachedSkinmesh == null ? 
                (_attachedSkinmesh = GetComponentInChildren<SkinnedMeshRenderer> ()) != null : true);
        }
        #endregion

        #region implemented abstract members of AbstractMeshOBB
        protected override Transform RootTransform () {
            return _attachedSkinmesh.rootBone;
        }
        protected override Bounds LocalBounds () {
            return _attachedSkinmesh.localBounds;
        }
        #endregion
    }
}
