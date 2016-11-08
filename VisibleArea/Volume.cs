using UnityEngine;
using System.Collections;
using Gist.Extensions.AABB;
using Recon.BoundingVolumes;
using Recon.BoundingVolumes.Behaviour;

namespace Recon.VisibleArea {
    
    #region Definitions
    public interface IVolume {
        Bounds GetBounds();
    }
    [System.Serializable]
    public class IVolumeEvent : UnityEngine.Events.UnityEvent<IVolume> {}
    #endregion
        
    [ExecuteInEditMode]
    public abstract class Volume : MonoBehaviour, IVolume, IConvex {
        ConvexUpdator _convUp;

		protected virtual void OnEnable () {
            Reconner.Add (this);
        }
		protected virtual void OnDisable() {
            Reconner.Remove (this);
        }
        protected virtual void OnDrawGizmos() {
            IConvexPolyhedron conv;
            if (!isActiveAndEnabled || (conv = GetConvexPolyhedron()) == null)
                return;
            conv.DrawGizmos ();
        }

        #region ConvexUpdator
        public ConvexUpdator ConvUp {
            get { return _convUp == null ? (_convUp = new ConvexUpdator (this)) : _convUp; }
        }
        #endregion

        #region IConvex implementation
        public abstract IConvexPolyhedron GetConvexPolyhedron ();
        public abstract bool StartConvex ();
        public abstract bool UpdateConvex ();
        #endregion

        #region IVolume implementation
		public virtual Bounds GetBounds () {
            return GetConvexPolyhedron ().WorldBounds ();
        }
        #endregion
    }
}