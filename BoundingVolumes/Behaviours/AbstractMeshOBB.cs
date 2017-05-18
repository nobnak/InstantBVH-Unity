using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;
using Gist.Extensions.AABB;

namespace Recon.BoundingVolumes.Behaviour {
    
    public abstract class AbstractMeshOBB : ConvexBuilder {
        public enum CoordinatesEnum { Skin = 0, Self, World }

        [SerializeField]
        protected CoordinatesEnum targetCoordinates;

        protected ConvexUpdator _convUp;
        protected OBB _obb;

        #region Abstract
        protected abstract Transform RootTransform();
        protected abstract Bounds LocalBounds();
        #endregion

        #region Unity
        protected virtual void OnDrawGizmos() {
			if (!isActiveAndEnabled || _obb == null)
                return;

            ConvUp.AssureUpdateConvex ();
            _obb.DrawGizmos ();
        }
        #endregion

        #region implemented abstract members of IConvex
        public override IConvexPolyhedron GetConvexPolyhedron () {
            ConvUp.AssureUpdateConvex ();
            return _obb;
        }
        public override bool UpdateConvex () {
            return (_obb = CreateOBB()) != null;
        }
        #endregion

        public ConvexUpdator ConvUp {
            get { return (_convUp == null ? (_convUp = new ConvexUpdator (this)) : _convUp); }
        }

        OBB CreateOBB() {
            var rootBone = RootTransform();
            var localBounds = LocalBounds();

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
