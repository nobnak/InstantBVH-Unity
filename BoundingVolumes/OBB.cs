using UnityEngine;
using System.Collections;

namespace Recon.BoundingVolumes {

    public class OBB : IConvexPolyhedron {
        public Vector3 center;
        public Vector3 size;
        public Quaternion axis;

        public OBB(Vector3 center, Vector3 size, Quaternion axis) {
            this.center = center;
            this.size = size;
            this.axis = axis;
        }

        public Matrix4x4 ModelMatrix() {
            return Matrix4x4.TRS (center, axis, size);
        }
        public OBB DrawGizmos() {
            Gizmos.matrix = ModelMatrix ();
            Gizmos.DrawWireCube (Vector3.zero, Vector3.one);
            return this;
        }

        #region IConvexPolyhedron implementation
        public System.Collections.Generic.IEnumerable<Vector3> Edges () {
            yield return axis * Vector3.right;
            yield return axis * Vector3.up;
            yield return axis * Vector3.forward;
        }
        public System.Collections.Generic.IEnumerable<Vector3> Vertices () {
            var half = 0.5f * (axis * size);
            for (var i = 0; i < 8; i++)
                yield return new Vector3 (
                    ((i & 1) != 0 ? 1 : -1) * half.x + center.x,
                    ((i & 2) != 0 ? 1 : -1) * half.y + center.y,
                    ((i & 4) != 0 ? 1 : -1) * half.z + center.z);
        }
        #endregion

        #region Static
        public static OBB Create(Transform tr, Bounds localBounds) {
            return new OBB (
                tr.TransformPoint (localBounds.center),
                Vector3.Scale (tr.localScale, localBounds.size),
                tr.rotation);
        }
        #endregion
    }
}
