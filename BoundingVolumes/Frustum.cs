using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Recon.BoundingVolumes {
        
    public class Frustum : IConvexPolyhedron {
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

        #region IConvexPolyhedron implementation
        public IEnumerable<Vector3> Edges () {
            yield return axis * Vector3.right;
            yield return axis * Vector3.up;
            var v = axis * farBottomLeft;
            for (var i = 0; i < 4; i++)
                yield return new Vector3 (
                    ((i & 1) != 0 ? 1 : -1) * v.x,
                    ((i & 2) != 0 ? 1 : -1) * v.y,
                    v.z);
        }
        public IEnumerable<Vector3> Vertices () {
            yield return head;
            var v = axis * farBottomLeft;
            for (var i = 0; i < 4; i++)
                yield return new Vector3 (
                    ((i & 1) != 0 ? 1 : -1) * v.x + head.x,
                    ((i & 2) != 0 ? 1 : -1) * v.y + head.y,
                    v.z + head.z);
        }
        #endregion

        #region Static
        public static Frustum Create(Camera cam) {
            var z = cam.farClipPlane;
            var y = z * Mathf.Tan (0.5f * cam.fieldOfView * Mathf.Deg2Rad);
            var x = y * cam.aspect;
            return new Frustum (cam.transform.position, new Vector3 (-x, -y, z), cam.transform.rotation);            
        }
        #endregion
    }
}
