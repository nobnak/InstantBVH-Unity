using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist.Extensions.AABB;

namespace Recon.BoundingVolumes {
        
    public class Frustum : IConvexPolyhedron {
        public const float MIN_NEAR_PLANE = 1e-6f;

        public Vector3 head;
        public Vector3 farBottomLeft;
        public Vector3 nearBottomLeft;
        public Quaternion axis;

        public Frustum(Vector3 head, Vector3 farBottomLeft, Quaternion axis)
            : this(head, farBottomLeft, MIN_NEAR_PLANE, farBottomLeft.z, axis) {
        }
        public Frustum(Vector3 head, Vector3 farBottomLeft, float nearPlane, float farPlane, Quaternion axis) {
            farBottomLeft *= 1f / farBottomLeft.z;

            this.head = head;
            this.farBottomLeft = farPlane * farBottomLeft;
            this.nearBottomLeft = nearPlane * farBottomLeft;
            this.axis = axis;
        }

        public float FoV() { return 2f * Mathf.Atan2 (farBottomLeft.y, farBottomLeft.z) * Mathf.Rad2Deg; }
        public float Aspect() { return farBottomLeft.x / farBottomLeft.y; }

        public float FarPlane() { return farBottomLeft.z; }
        public float NearPlane() { return nearBottomLeft.z; }

        #region IConvexPolyhedron implementation
        public IEnumerable<Vector3> Normals () {
            yield return axis * Vector3.forward;
            yield return axis * Vector3.back;

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
            yield return head + axis * farBottomLeft;
            yield return head + axis * new Vector3 (farBottomLeft.x, -farBottomLeft.y, farBottomLeft.z);
            yield return head + axis * new Vector3 (-farBottomLeft.x, farBottomLeft.y, farBottomLeft.z);
            yield return head + axis * new Vector3 (-farBottomLeft.x, -farBottomLeft.y, farBottomLeft.z);

            yield return head + axis * nearBottomLeft;
            yield return head + axis * new Vector3 (nearBottomLeft.x, -nearBottomLeft.y, nearBottomLeft.z);
            yield return head + axis * new Vector3 (-nearBottomLeft.x, nearBottomLeft.y, nearBottomLeft.z);
            yield return head + axis * new Vector3 (-nearBottomLeft.x, -nearBottomLeft.y, nearBottomLeft.z);
        }
        public Bounds LocalBounds() {
            return new Bounds (
                new Vector3(0f, 0f, 0.5f * (nearBottomLeft.z + farBottomLeft.z)),
                new Vector3(-2f * farBottomLeft.x, -2f * farBottomLeft.y, farBottomLeft.z - nearBottomLeft.z));
        }
        public Bounds WorldBounds() {
            return LocalBounds().EncapsulateInTargetSpace (Matrix4x4.TRS (head, axis, Vector3.one));
        }
		public Matrix4x4 ModelMatrix() { return Matrix4x4.TRS (head, axis, Vector3.one); }

        public void DrawFrustum(Matrix4x4 modelmat, Color color) {
            var nearPlane = NearPlane ();

            Gizmos.color = color;
            Gizmos.matrix = modelmat;
            Gizmos.DrawFrustum (new Vector3(0f, 0f, nearPlane), FoV (), FarPlane (), nearPlane, Aspect ());
            Gizmos.matrix = Matrix4x4.identity;
        }
        public void DrawFrustum(Color color) { DrawFrustum (ModelMatrix (), color); }

        public IConvexPolyhedron DrawGizmos() {
            var aabb = WorldBounds ();
			var modelmat = ModelMatrix ();

			Gizmos.color = ConvexPolyhedronSettings.GizmoAABBColor;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireCube (aabb.center, aabb.size);

            Gizmos.color = ConvexPolyhedronSettings.GizmoLineColor;

            #if false
			Gizmos.matrix = Matrix4x4.identity;
            foreach (var v in Vertices())
                Gizmos.DrawSphere (v, 0.5f);
            #endif
            
            DrawFrustum (modelmat, ConvexPolyhedronSettings.GizmoLineColor);


            return this;
        }
        #endregion

        #region Static
        public static Frustum Create(Camera cam) {
            var z = cam.farClipPlane;
            var y = z * Mathf.Tan (0.5f * cam.fieldOfView * Mathf.Deg2Rad);
            var x = y * cam.aspect;
            return new Frustum (cam.transform.position, new Vector3 (-x, -y, z), 
                cam.nearClipPlane, cam.farClipPlane, cam.transform.rotation);            
        }
        public static Frustum Create(Vector3 position, Quaternion rotation, float horAngle, float verAngle, float range) {
            var z = range;
            var y = z * Mathf.Tan (0.5f * verAngle * Mathf.Deg2Rad);
            var x = z * Mathf.Tan (0.5f * horAngle * Mathf.Deg2Rad);
            return new Frustum (position, new Vector3 (-x, -y, z), rotation);
        }
        public static Frustum Create(Vector3 position, Quaternion rotation, float horAngle, float verAngle, float nearPlane, float farPlane) {
            var z = farPlane;
            var y = z * Mathf.Tan (0.5f * verAngle * Mathf.Deg2Rad);
            var x = z * Mathf.Tan (0.5f * horAngle * Mathf.Deg2Rad);
            return new Frustum (position, new Vector3 (-x, -y, z), nearPlane, farPlane, rotation);
        }
        #endregion
    }
}
