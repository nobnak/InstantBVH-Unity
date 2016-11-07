using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Recon.BoundingVolumes {

    public interface IConvexPolyhedron {

        IEnumerable<Vector3> Normals();
        IEnumerable<Vector3> Edges();
        IEnumerable<Vector3> Vertices();

        Bounds LocalBounds();
        Bounds WorldBounds();

		IConvexPolyhedron DrawGizmos();
    }

	public static class ConvexPolyhedronSettings {
		public static Color GizmoAABBColor = Color.gray;
		public static Color GizmoLineColor = new Color (0.6f, 0.8f, 0.2f);
		public static Color GizmoSurfaceColor = new Color(0.6f, 0.8f, 0.2f, 0f);
	}
}
