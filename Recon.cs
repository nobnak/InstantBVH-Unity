using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist;
using Reconnitioning.SpacePartition;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Recon : MonoBehaviour {
        void Update() {
            RebuildBVH ();
        }
        void OnDrawGizmos() {
            if (Recon._bvh == null)
                return;
            Gizmos.color = gizmoColorBounds;
            Recon._bvh.DrawBounds (0, 10);
        }

		public Color gizmoColorBounds = Color.green;
        public BVHController<IVolume> BVH { get { return Recon._bvh; } }

        public BVHController<IVolume> RebuildBVH () {
            Recon._bounds.Clear ();
            var vals = Recon._database.GetList ();
            for (var i = 0; i < vals.Count; i++)
                Recon._bounds.Add (vals [i].GetBounds ());
            return Recon._bvh.Build (Recon._bounds, vals);
        }

        #region Static
		static Dataset<IVolume> _database;
		static BVHController<IVolume> _bvh;
		static List<Bounds> _bounds;

		static Recon() {
			_database = new Dataset<IVolume> ();
			_bvh = new BVHController<IVolume> ();
			_bounds = new List<Bounds> ();
		}

        public static Recon Instance { get; protected set; }
        public static void Add(IVolume vol) {
            _database.Add (vol);
        }
        public static bool Remove(IVolume vol) {
            return _database.Remove (vol) >= 0;
        }
        #endregion

    }
}
