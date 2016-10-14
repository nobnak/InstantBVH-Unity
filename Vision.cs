using UnityEngine;
using System.Collections;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Vision : MonoBehaviour {
        public float range = 10f;
        public float angle = 90f;

        #region Unity
    	void Start () {
            Recon.Add (this);
            
    	}
        void OnDestroy() {
            Recon.Remove (this);
        }
        void OnDrawGizmos() {
            var r = Recon.Instance;
            if (r != null)
                r.DrawRange (this);
        }
        #endregion
    }
}