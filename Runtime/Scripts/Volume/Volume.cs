using nobnak.Gist.Extensions.ComponentExt;
using nobnak.Gist.Primitive;
using Recon.BehaviourSys;
using Recon.Core;
using UnityEngine;

namespace Recon.VolumeSys {

	#region Definitions
	[System.Serializable]
    public class VolumeEvent : UnityEngine.Events.UnityEvent<Volume> {}
    #endregion
        
    [ExecuteAlways]
    public abstract class Volume : AbstractMeshOBB, IVolume<Volume> {
        public int mask = -1;

		#region Unity
		protected virtual void OnEnable () {
            _convUp = null;
            this.CallbackParent<Reconner>(v => v.Add (this));
        }
		protected virtual void OnDisable() {
			this.CallbackParent<Reconner>(v => v.Remove (this));
        }
        #endregion

        #region IVolume implementation
		public virtual FastBounds Bounds {
			get { return GetConvexPolyhedron().WorldBounds(); }
		}
		public Volume Value { get { return this; } }
		#endregion
	}
}