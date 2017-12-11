using UnityEngine;
using System.Collections;
using nobnak.Gist.Extensions.AABB;
using Recon.BoundingVolumes;
using Recon.BoundingVolumes.Behaviour;

namespace Recon.VisibleArea {
    
    #region Definitions
    public interface IVolume {
        Bounds GetBounds();
    }
    [System.Serializable]
    public class VolumeEvent : UnityEngine.Events.UnityEvent<Volume> {}
    #endregion
        
    [ExecuteInEditMode]
    public abstract class Volume : AbstractMeshOBB, IVolume {
        public int mask = -1;

        #region Unity
		protected virtual void OnEnable () {
            _convUp = null;
            Reconner.Add (this);
        }
		protected virtual void OnDisable() {
            Reconner.Remove (this);
        }
        #endregion

        #region IVolume implementation
		public virtual Bounds GetBounds () {
            return GetConvexPolyhedron ().WorldBounds ();
        }
        #endregion
    }
}