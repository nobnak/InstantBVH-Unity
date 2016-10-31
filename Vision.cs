using UnityEngine;
using System.Collections;
using Gist;
using System.Collections.Generic;
using Reconnitioning.SpacePartition;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Vision : MonoBehaviour {
        public Color colorInsight = new Color (0.654f, 1f, 1f);
        public Color colorSpot = new Color (1f, 0.65f, 1f);

        public float range = 10f;
        public float angle = 90f;

        public IVolumeEvent InSight;

        void Update() {
            foreach (var v in NarrowPhase())
                InSight.Invoke (v);
        }
        void OnDrawGizmos() {
            var r = Recon.Instance;
            if (r == null)
                return;
            
            DrawRange (r.Fig);
        }

        #region Public
        public void DrawRange (GLFigure _fig) {
            if (_fig == null)
                return;
            
            var tr = transform;
            _fig.DrawFan (tr.position, Quaternion.LookRotation (-tr.up, tr.forward), range * Vector3.one, colorSpot, -angle, angle);

            foreach (var v in NarrowPhase())
                DrawInsight (v.GetBounds ().center);
        }
        public void DrawInsight (Vector3 posTo) {
            Gizmos.color = colorInsight;
            Gizmos.DrawLine (transform.position, posTo);
        }
        public Bounds SphereBounds() {
            return new Bounds (transform.position, 2f * range * Vector3.one);
        }
        public bool Intersect(Vector3 position) {
            var ray = position - transform.position;
            return (ray.sqrMagnitude <= (range * range)
                && Mathf.Acos (Vector3.Dot (transform.forward, ray.normalized)) <= (angle * Mathf.Deg2Rad));
        }
        public IEnumerable<IVolume> Broadphase() {
            Recon r;
            BVHController<IVolume> bvh;
            if ((r = Recon.Instance) == null || (bvh = r.BVH) == null)
                yield break;

            foreach (var v in bvh.Intersect(SphereBounds()))
                yield return v;
        }
        public IEnumerable<IVolume> NarrowPhase() {
            foreach (var v in Broadphase())
                if (Intersect (v.GetBounds ().center))
                    yield return v;
        }
        #endregion
    }
}