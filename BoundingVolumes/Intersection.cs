using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Recon.BoundingVolumes {
        
    public static class Intersection {
        public const float E = 1e-6f;

        public static void RangeAlongAxis(Vector3 axis, IEnumerable<Vector3> points, out float min, out float max) {
            min = float.MaxValue;
            max = float.MinValue;
            foreach (var p in points) {
                var v = Vector3.Dot (axis, p);
                if (v < min)
                    min = v;
                else if (max < v)
                    max = v;
            }
        }

        public static bool Intersect(Vector3 axis, IEnumerable<Vector3> v0, IEnumerable<Vector3> v1) {
            float s0, e0, s1, e1;
            RangeAlongAxis(axis, v0, out s0, out e0);
            RangeAlongAxis(axis, v1, out s1, out e1);
            return s0 <= e1 && s1 <= e0;
        }
        public static bool Intersect(this IConvexPolyhedron a, IConvexPolyhedron b) {
            if (!a.WorldBounds ().Intersects (b.WorldBounds ()))
                return false;
            
            foreach (var ae in a.Edges()) {
                foreach (var be in b.Edges()) {
                    if (ae.sqrMagnitude > E && !Intersect (ae, a.Vertices (), b.Vertices ()))
                        return false;
                    if (be.sqrMagnitude > E && !Intersect (be, a.Vertices (), b.Vertices ()))
                        return false;
                    var ce = Vector3.Cross (ae, be);
                    if (ce.sqrMagnitude > E && !Intersect (ce, a.Vertices (), b.Vertices ()))
                        return false;
                }
            }
            return true;
        }
    }
}
