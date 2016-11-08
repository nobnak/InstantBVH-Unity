using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;

namespace Recon.BoundingVolumes.Behaviour {
    [RequireComponent(typeof(Camera))]
    public class CameraFrustum : ConvexBuilder {
        public Color color;

        ConvexUpdator _convUp;
        Camera _attachedCamera;
        Frustum _frustum;

        void OnDrawGizmos() {
			if (!isActiveAndEnabled || _frustum == null)
                return;
            
            ConvUp.AssureUpdateConvex ();            
            Gizmos.color = color;
            _frustum.DrawGizmos ();
        }

        public Camera AttachedCamera {
            get { return _attachedCamera; }
        }
        public ConvexUpdator ConvUp {
            get { return (_convUp == null ? (_convUp = new ConvexUpdator (this)) : _convUp); }
        }

        #region implemented abstract members of IConvex
        public override IConvexPolyhedron GetConvexPolyhedron () {
            ConvUp.AssureUpdateConvex ();
            return _frustum;
        }
        public override bool StartConvex () {
            if (_attachedCamera == null)
                _attachedCamera = GetComponent<Camera> ();
            return _attachedCamera != null;
        }
        public override bool UpdateConvex () {
            return (_frustum = Frustum.Create (_attachedCamera)) != null;
        }
        #endregion
    }
}
