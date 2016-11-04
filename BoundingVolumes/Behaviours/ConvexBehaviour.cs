using UnityEngine;
using System.Collections;

namespace Recon.BoundingVolumes.Behaviour {

    [ExecuteInEditMode]
    public abstract class ConvexBehaviour : MonoBehaviour {
        bool _initialized = false;
        int _frameCount = int.MinValue;

        public abstract IConvexPolyhedron GetConvexPolyhedron();
        public abstract bool StartConvex();
        public abstract bool UpdateConvex();

        public virtual ConvexBehaviour AssureStartConvex() {
            if (!_initialized && StartConvex ())
                _initialized = true;
            return this;
        }
        public virtual ConvexBehaviour AssureUpdateConvex() {
            AssureStartConvex ();
            if (_frameCount != Time.frameCount && UpdateConvex ())
                _frameCount = Time.frameCount;
            return this;
        }
    }
}