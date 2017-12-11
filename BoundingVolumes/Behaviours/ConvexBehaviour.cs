using UnityEngine;
using System.Collections;
using nobnak.Gist.Intersection;

namespace Recon.BoundingVolumes.Behaviour {
    public interface IConvex {
        IConvex3Polytope GetConvexPolyhedron();
        bool StartConvex();
        bool UpdateConvex();
    }

    public abstract class ConvexBuilder : MonoBehaviour, IConvex {
        public abstract IConvex3Polytope GetConvexPolyhedron();
        public abstract bool StartConvex();
        public abstract bool UpdateConvex();
    }

    public class ConvexUpdator {
        public readonly IConvex Convex;
        bool _initialized = false;
        int _frameCount = int.MinValue;

        public ConvexUpdator(IConvex conv) {
            this.Convex = conv;
        }

        public IConvex3Polytope GetConvexPolyhedron() { return Convex.GetConvexPolyhedron(); }
        public bool StartConvex() { return Convex.StartConvex(); }
        public bool UpdateConvex() { return Convex.UpdateConvex(); }

        public virtual ConvexUpdator AssureStartConvex() {
            if (!_initialized && StartConvex ())
                _initialized = true;
            return this;
        }
        public virtual ConvexUpdator AssureUpdateConvex() {
            AssureStartConvex ();
            if (_frameCount != Time.frameCount && UpdateConvex ())
                _frameCount = Time.frameCount;
            return this;
        }
    }
}