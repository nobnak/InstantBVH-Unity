using UnityEngine;
using System.Collections;
using Gist;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Vision : MonoBehaviour {
        public Color colorInsight = new Color (0.654f, 1f, 1f);
        public Color colorSpot = new Color (1f, 0.65f, 1f);

        public float range = 10f;
        public float angle = 90f;

        void OnDrawGizmos() {
            var r = Recon.Instance;
            if (r == null)
                return;
            
            DrawRange (r.Fig);

            var bvh = r.BVH;
            if (bvh == null)
                return;


        }

        #region Public
        public void DrawRange (GLFigure _fig) {
            if (_fig == null)
                return;
            
            var tr = transform;
            _fig.DrawFan (tr.position, Quaternion.LookRotation (-tr.up, tr.forward), range * Vector3.one, colorSpot, -angle, angle);
        }
        public void DrawInsight (Vector3 posTo) {
            Gizmos.color = colorInsight;
            Gizmos.DrawLine (transform.position, posTo);
        }
        #endregion
    }
}