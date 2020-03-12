using UnityEngine;
using System.Collections;
using nobnak.Gist;
using System.Collections.Generic;
using Recon.SpacePartition;
using Recon.VisibleArea;
using Recon.BoundingVolumes.Behaviour;
using Recon.BoundingVolumes;
using System.Linq;
using nobnak.Gist.Intersection;
using nobnak.Gist.Extensions.ComponentExt;

namespace Recon {
    [ExecuteInEditMode]
    public class Vision : MonoBehaviour, IConvex {
        [Header("Filter")]
        public int mask = -1;
        [Header("Debug")]
        public Color colorInsight = new Color (0.654f, 1f, 1f);
        public Color colorSpot = new Color (1f, 0.65f, 1f);
        [Header("Frustum")]
        public float range = 10f;
        public float nearClip = 1f;
        public float angle = 90f;
        public float vertAngle = 45f;

        public VolumeEvent InSight;

		protected Reconner reconner;
		protected List<Volume> _insightVolumes;
		protected Volume[] _selfVolumes;
		protected ConvexUpdator _convUp;
		protected Frustum _frustum;

        void Awake() {
			reconner = this.Parent<Reconner>().FirstOrDefault();

            _insightVolumes = new List<Volume> ();
            _selfVolumes = GetComponentsInChildren<Volume> ();
        }
        void Update() {
            _insightVolumes.Clear ();
            foreach (var v in reconner.Find(GetConvexPolyhedron(), FilterSelfIntersection, FilterMask))
                _insightVolumes.Add (v);
            foreach (var v in _insightVolumes)
                InSight.Invoke (v);            
        }
        void OnDrawGizmos() {
            if (!isActiveAndEnabled || reconner == null)
                return;
            
            ConvUp.AssureUpdateConvex ();
            _frustum.DrawGizmos ();
            foreach (var v in reconner.Find(GetConvexPolyhedron(), FilterSelfIntersection, FilterMask))
                DrawInsight (v.Bounds.Center);
        }

		public bool IsInSight(Volume v) {
			return _insightVolumes.Contains (v);
		}
        public Frustum CreateFrustum () {
            return Frustum.Create (transform.position, transform.rotation, angle, vertAngle, nearClip, range);
        }        
        public bool FilterSelfIntersection(Volume v) {
            foreach (var w in _selfVolumes)
                if (v == w)
                    return false;
            return true;
        }
        public bool FilterMask(Volume v) {
            return (v.mask & mask) != 0;
        }

        #region ConvexUpdator
        public ConvexUpdator ConvUp {
            get { return _convUp == null ? (_convUp = new ConvexUpdator (this)) : _convUp; }
        }
        #endregion

        #region Gizmos
        public void DrawInsight (Vector3 posTo) {
            Gizmos.color = colorInsight;
            Gizmos.DrawLine (transform.position, posTo);
        }
        #endregion

        #region IConvex implementation
        public IConvex3Polytope GetConvexPolyhedron () {
            ConvUp.AssureUpdateConvex ();
            return _frustum;
        }
        public bool StartConvex () {
            range = Mathf.Clamp (range, 0f, float.MaxValue);
            angle = Mathf.Clamp (angle, 0f, 179f);
            vertAngle = Mathf.Clamp (vertAngle, 0f, 179f);
            return true;
        }
        public bool UpdateConvex () {
            return (_frustum = CreateFrustum ()) != null;
        }
        #endregion
    }
}
