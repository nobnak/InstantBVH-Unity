using UnityEngine;
using System.Collections;

namespace Recon.BoundingVolumes {

    public class OBB {
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
