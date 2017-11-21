using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;
using Gist.Extensions.AABB;
using Gist.Intersection;

namespace Recon.BoundingVolumes.Behaviour {
    
    public abstract class AbstractMeshOBB : ConvexBuilder {
        public enum CoordinatesEnum { Skin = 0, Self, World }

        [SerializeField]
        protected CoordinatesEnum targetCoordinates;

        protected ConvexUpdator _convUp;
        protected OBB3 _obb;

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
        public override IConvex3Polytope GetConvexPolyhedron () {
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

        OBB3 CreateOBB() {
            var rootBone = RootTransform();
            var localBounds = LocalBounds();

            switch (targetCoordinates) {
            case CoordinatesEnum.World:
                return new OBB3 (rootBone.EncapsulateInWorldSpace (localBounds), Matrix4x4.identity);
            case CoordinatesEnum.Self:
                var rootToSelfMatrix = transform.worldToLocalMatrix * rootBone.localToWorldMatrix;
                return new OBB3 (localBounds.EncapsulateInTargetSpace (rootToSelfMatrix), 
                    transform.localToWorldMatrix);
            default:
                return OBB3.Create (rootBone, localBounds);
            }
        }
    }
}
