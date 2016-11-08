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

        protected virtual void Awake() {
            _convUp = new ConvexUpdator(this);
        }
         
        void OnDrawGizmos() {
			if (!isActiveAndEnabled || _frustum == null)
                return;
            
            _convUp.AssureUpdateConvex ();            
            Gizmos.color = color;
            _frustum.DrawGizmos ();
        }

        public Camera AttachedCamera {
            get { return _attachedCamera; }
        }

        #region implemented abstract members of IConvex
        public override IConvexPolyhedron GetConvexPolyhedron () {
            _convUp.AssureUpdateConvex ();
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
