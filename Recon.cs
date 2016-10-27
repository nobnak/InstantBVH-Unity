using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist;
using Reconnitioning.SpacePartition;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Recon : MonoBehaviour {
        public Color gizmoColorBounds = Color.green;

        Dataset<IVolume> _database;
        BVHController<IVolume> _bvh;
        List<Bounds> _bounds;

        GLFigure _fig;

        void OnEnable() {
            _database = new Dataset<IVolume> ();
            _fig = new GLFigure ();
            _bounds = new List<Bounds> ();
            _bvh = new BVHController<IVolume> ();
            Instance = this;
        }
        void OnDisable() {
            if (_fig != null) {
                _fig.Dispose ();
                _fig = null;
            }
        }
        void Update() {
            RebuildBVH ();
        }
        void OnDrawGizmos() {
            if (_bvh == null)
                return;
            Gizmos.color = gizmoColorBounds;
            _bvh.DrawBounds (0, 10);
        }

        #region Public
        public GLFigure Fig { get { return _fig; } }
        public BVHController<IVolume> BVH { get { return _bvh; } }

        public BVHController<IVolume> RebuildBVH () {
            _bounds.Clear ();
            var vals = _database.GetList ();
            for (var i = 0; i < vals.Count; i++)
                _bounds.Add (vals [i].GetBounds ());
            return _bvh.Build (_bounds, vals);
        }
        #endregion

        #region Static
        public static Recon Instance { get; protected set; }
        public static void Add(IVolume vol) {
            if (Instance == null)
                Debug.LogFormat ("There is no instance");
            else
                Instance._database.Add (vol);
        }
        public static bool Remove(IVolume vol) {
            return (Instance != null && Instance._database.Remove (vol) >= 0);
        }
        #endregion

    }
}
