using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist.Extensions.AABB;

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

        #region IConvexPolyhedron implementation
        public IEnumerable<Vector3> Normals () {
            yield return axis * Vector3.forward;
            yield return axis * new Vector3 (farBottomLeft.z, 0f, -farBottomLeft.x);
            yield return axis * new Vector3 (farBottomLeft.z, 0f, farBottomLeft.x);
            yield return axis * new Vector3 (0f, farBottomLeft.z, -farBottomLeft.y);
            yield return axis * new Vector3 (0f, farBottomLeft.z, farBottomLeft.y);
        }
        public IEnumerable<Vector3> Edges() {
            yield return axis * Vector3.right;
            yield return axis * Vector3.up;

            yield return axis * farBottomLeft;
            yield return axis * new Vector3 (-farBottomLeft.x, farBottomLeft.y, farBottomLeft.z);
            yield return axis * new Vector3 (farBottomLeft.x, -farBottomLeft.y, farBottomLeft.z);
            yield return axis * new Vector3 (-farBottomLeft.x, -farBottomLeft.y, farBottomLeft.z);
        }
        public IEnumerable<Vector3> Vertices () {
            yield return head;

            var x = axis * new Vector3 (-farBottomLeft.x, 0f, 0f);
            var y = axis * new Vector3 (0f, -farBottomLeft.y, 0f);
            var z = head + axis * new Vector3 (0f, 0f, farBottomLeft.z);
            for (var i = 0; i < 4; i++)
                yield return z + ((i & 1) != 0 ? x : -x) + ((i & 2) != 0 ? y : -y);
        }
        public Bounds LocalBounds() {
            return new Bounds (
                new Vector3(0f, 0f, 0.5f * farBottomLeft.z),
                new Vector3(-2f * farBottomLeft.x, -2f * farBottomLeft.y, farBottomLeft.z));
        }
        public Bounds WorldBounds() {
            return LocalBounds().EncapsulateInWorldBounds (Matrix4x4.TRS (head, axis, Vector3.one));
        }
		public Matrix4x4 ModelMatrix() { return Matrix4x4.TRS (head, axis, Vector3.one); }
        public IConvexPolyhedron DrawGizmos() {
            var aabb = WorldBounds ();
			var modelmat = ModelMatrix ();

			Gizmos.color = ConvexPolyhedronSettings.GizmoAABBColor;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireCube (aabb.center, aabb.size);

			Gizmos.color = ConvexPolyhedronSettings.GizmoLineColor;
			Gizmos.matrix = Matrix4x4.identity;
            foreach (var v in Vertices())
                Gizmos.DrawSphere (v, 0.5f);
            
			Gizmos.matrix = modelmat;
            Gizmos.DrawFrustum (Vector3.zero, FoV (), MaxRange (), 1e-6f, Aspect ());

            Gizmos.matrix = Matrix4x4.identity;

            return this;
        }
        #endregion

        #region Static
        public static Frustum Create(Camera cam) {
            var z = cam.farClipPlane;
            var y = z * Mathf.Tan (0.5f * cam.fieldOfView * Mathf.Deg2Rad);
            var x = y * cam.aspect;
            return new Frustum (cam.transform.position, new Vector3 (-x, -y, z), cam.transform.rotation);            
        }
        public static Frustum Create(Vector3 position, Quaternion rotation, float horAngle, float verAngle, float range) {
            var z = range;
            var y = z * Mathf.Tan (0.5f * verAngle * Mathf.Deg2Rad);
            var x = z * Mathf.Tan (0.5f * horAngle * Mathf.Deg2Rad);
            return new Frustum (position, new Vector3 (-x, -y, z), rotation);
        }
        #endregion
    }
}
