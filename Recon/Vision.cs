using UnityEngine;
using System.Collections;

namespace Reconnitioning {
    public class Vision : MonoBehaviour {
        public float range = 10f;
        public float angle = 90f;

    	void Start () {
            Recon.Add (this);
            
    	}
        void OnDestroy() {
            Recon.Remove (this);
        }
    }
}