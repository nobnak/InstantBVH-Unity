using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist;
using Recon.SpacePartition;
using Recon.VisibleArea;

namespace Recon {
    [ExecuteInEditMode]
    public class Reconner : MonoBehaviour {
        void OnEnable() {
            Instance = this;
        }
        void Update() {
            RebuildBVH ();
        }
        void OnDrawGizmos() {
            if (Reconner._bvh == null)
                return;
            Gizmos.color = gizmoColorBounds;
            Reconner._bvh.DrawBounds (0, 10);
        }

		public Color gizmoColorBounds = Color.green;
        public BVHController<Volume> BVH { get { return Reconner._bvh; } }

        public BVHController<Volume> RebuildBVH () {
            Reconner._bounds.Clear ();
            var vals = Reconner._database.GetList ();
            for (var i = 0; i < vals.Count; i++)
                Reconner._bounds.Add (vals [i].GetBounds ());
            return Reconner._bvh.Build (Reconner._bounds, vals);
        }

        #region Static
		static Dataset<Volume> _database;
		static BVHController<Volume> _bvh;
		static List<Bounds> _bounds;

		static Reconner() {
			_database = new Dataset<Volume> ();
			_bvh = new BVHController<Volume> ();
			_bounds = new List<Bounds> ();
		}

        public static Reconner Instance { get; protected set; }
        public static void Add(Volume vol) {
            _database.Add (vol);
        }
        public static bool Remove(Volume vol) {
            return _database.Remove (vol) >= 0;
        }
        #endregion

    }
}
