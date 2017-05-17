using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;
using Gist.Extensions.AABB;

namespace Recon.BoundingVolumes.Behaviour {
    
    public class SkinOBB : ConvexBuilder {
        public enum CoordinatesEnum { Skin = 0, Self, World }

        [SerializeField]
        CoordinatesEnum targetCoordinates;

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
            return (_obb = CreateOBB()) != null;
        }
        #endregion

        OBB CreateOBB() {
            var rootBone = _attachedSkinmesh.RootBone ();
            var localBounds = _attachedSkinmesh.localBounds;

            switch (targetCoordinates) {
            case CoordinatesEnum.World:
                return new OBB (rootBone.EncapsulateInWorldSpace (localBounds), Matrix4x4.identity);
            case CoordinatesEnum.Self:
                var rootToSelfMatrix = transform.worldToLocalMatrix * rootBone.localToWorldMatrix;
                return new OBB (localBounds.EncapsulateInTargetSpace (rootToSelfMatrix), 
                    transform.localToWorldMatrix);
            default:
                return OBB.Create (rootBone, localBounds);
            }
        }
    }
}
