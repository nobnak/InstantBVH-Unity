using UnityEngine;
using System.Collections;

namespace Recon.BoundingVolumes {
        
    public static class Intersection {

        public static bool Intersect(IConvexPolyhedron a, IConvexPolyhedron b) {
			foreach (var ae in a.Edges())
				foreach (var be in b.Edges())
					
        }
    }
}
