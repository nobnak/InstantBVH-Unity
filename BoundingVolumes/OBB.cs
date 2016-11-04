using UnityEngine;
using System.Collections;
using Gist.Extensions.AABB;
using System.Collections.Generic;

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
        #region IConvexPolyhedron implementation
        public IEnumerable<Vector3> Normals () {
            yield return axis * Vector3.right;
            yield return axis * Vector3.up;
            yield return axis * Vector3.forward;
        }
        public IEnumerable<Vector3> Edges() {
            return Normals ();
        }
        public IEnumerable<Vector3> Vertices () {
            var x = 0.5f * size.x * (axis * Vector3.right);
            var y = 0.5f * size.y * (axis * Vector3.up);
            var z = 0.5f * size.z * (axis * Vector3.forward);

            for (var i = 0; i < 8; i++)
                yield return center + ((i & 1) != 0 ? x : -x) + ((i & 2) != 0 ? y : -y) + ((i & 4) != 0 ? z : -z);
        }
        public Bounds LocalBounds() {
            return new Bounds (Vector3.zero, size);
        }
        public Bounds WorldBounds() {
            return LocalBounds().EncapsulateInWorldBounds(Matrix4x4.TRS(center, axis, Vector3.one));
        }
        public IConvexPolyhedron DrawGizmos() {
            var color = Gizmos.color;
            var aabb = WorldBounds ();
            Gizmos.color = Color.gray;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireCube (aabb.center, aabb.size);

            Gizmos.color = color;
            foreach (var v in Vertices())
                Gizmos.DrawSphere (v, 0.5f);

            Gizmos.matrix = ModelMatrix ();
            Gizmos.DrawWireCube (Vector3.zero, Vector3.one);

            return this;
        }
        #endregion

        #region Static
        public static OBB Create(Transform tr, Bounds localBounds) {
            return new OBB (
                tr.TransformPoint (localBounds.center),
                Vector3.Scale (tr.lossyScale, localBounds.size),
                tr.rotation);
        }
        #endregion
    }
}
