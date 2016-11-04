using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;

namespace Recon.BoundingVolumes.Behaviour {
    [RequireComponent(typeof(Camera))]
    public class CameraFrustum : ConvexBehaviour {
        public Color color;

        Camera _attachedCamera;
        Frustum _frustum;
         
        void OnDrawGizmos() {
            if (!isActiveAndEnabled)
                return;
            
            AssureUpdateConvex ();            
            Gizmos.color = color;
            _frustum.DrawGizmos ();
        }

        public Camera AttachedCamera {
            get { return _attachedCamera; }
        }

        #region implemented abstract members of ConvexBehaviour
        public override IConvexPolyhedron GetConvexPolyhedron () {
            AssureUpdateConvex ();
            return _frustum;
        }
        #endregion

        #region implemented abstract members of ConvexBehaviour
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
