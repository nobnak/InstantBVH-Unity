using UnityEngine;
using System.Collections;

namespace Recon.BoundingVolumes {
        
    public class Frustum {
        public Vector3 head;
        public Vector3 farBottomLeft;
        public Quaternion axis;

        public Frustum(Vector3 head, Vector3 farBottomLeft, Quaternion axis) {
            this.head = head;
            this.farBottomLeft = farBottomLeft;
            this.axis = axis;
        }

        public float FoV() {
            return 2f * Mathf.Atan2 (farBottomLeft.y, farBottomLeft.z) * Mathf.Rad2Deg;
        }
        public float Aspect() {
            return farBottomLeft.x / farBottomLeft.y;
        }
        public float MaxRange() {
            return farBottomLeft.z;
        }
        public Frustum DrawGizmos() {
            Gizmos.matrix = Matrix4x4.TRS (head, axis, Vector3.one);
            Gizmos.DrawLine (Vector3.zero, farBottomLeft);
            Gizmos.DrawFrustum (Vector3.zero, FoV (), MaxRange (), 1e-6f, Aspect ());
            return this;
        }
    }
}
