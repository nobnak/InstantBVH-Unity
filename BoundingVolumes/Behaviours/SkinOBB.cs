using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;

namespace Recon.BoundingVolumes.Behaviour {
    
    public class SkinOBB : ConvexBuilder {
        ConvexUpdator _convUp;
        SkinnedMeshRenderer _attachedSkinmesh;
        OBB _obb;

        protected void OnDrawGizmos() {
			if (!isActiveAndEnabled || _obb == null)
                return;

            ConvUp.AssureUpdateConvex ();
            _obb.DrawGizmos ();
        }

        public ConvexUpdator ConvUp {
            get { return (_convUp == null ? (_convUp = new ConvexUpdator (this)) : _convUp); }
        }

        #region Skinned Mesh
        public SkinnedMeshRenderer AttachedSkinMesh {
            get { return _attachedSkinmesh; }
        }
        #endregion
        #region implemented abstract members of IConvex
        public override IConvexPolyhedron GetConvexPolyhedron () {
            ConvUp.AssureUpdateConvex ();
            return _obb;
        }
        public override bool StartConvex () {
            return (_attachedSkinmesh == null ? 
                (_attachedSkinmesh = GetComponentInChildren<SkinnedMeshRenderer> ()) != null : true);
        }
        public override bool UpdateConvex () {
            return (_obb = OBB.Create (_attachedSkinmesh.RootBone(), _attachedSkinmesh.localBounds)) != null;
        }
        #endregion

    }
}
