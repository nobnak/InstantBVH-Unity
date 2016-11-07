using UnityEngine;
using System.Collections;
using Gist.Extensions.AABB;

namespace Reconnitioning {
    #region Definitions
    public interface IVolume {
        Bounds GetBounds();
    }
    [System.Serializable]
    public class IVolumeEvent : UnityEngine.Events.UnityEvent<IVolume> {}
    #endregion
        
    [ExecuteInEditMode]
    public class Volume : MonoBehaviour, IVolume {
        public Color colorLocalBounds = Color.gray;
        public Color colorWorldBounds = Color.cyan;
        public Bounds localBounds = new Bounds(Vector3.zero, Vector3.one);

		protected virtual void OnEnable () {
            Recon.Add (this);
        }
		protected virtual void OnDisable() {
            Recon.Remove (this);
        }
        protected virtual void OnDrawGizmos() {
            if (!isActiveAndEnabled)
                return;
            
            var bb = GetBounds ();

            Gizmos.color = colorLocalBounds;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube (localBounds.center, localBounds.size);

            Gizmos.color = colorWorldBounds;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireCube (bb.center, bb.size);
        }

        #region IVolume implementation
		public virtual Bounds GetBounds () {
            return transform.EncapsulateInWorldBounds(localBounds);
        }
        #endregion
    }
}