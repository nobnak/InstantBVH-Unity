using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Recon : MonoBehaviour {
        public Color colorInsight = new Color (0.654f, 1f, 1f);
        public Color colorSpot = new Color (1f, 0.65f, 1f);

        Dataset<Vision> _database;
        GraphBuilder _builder;
        Graph _graph;

        GLFigure _fig;

        void OnEnable() {
            _database = new Dataset<Vision> ();
            _graph = new Graph ();
            _builder = new GraphBuilder (_database, _graph);
            _fig = new GLFigure ();
            Instance = this;
        }
        void OnDisable() {
            if (_fig != null) {
                _fig.Dispose ();
                _fig = null;
            }
        }

        #region Gizmo
        void OnDrawGizmos() {
            if (_builder == null)
                return;
            _builder.Build ();

            var count = _database.Count;

            for (var i = 0; i < count; i++) {
                var v = _database [i];
                var posFrom = v.transform.position;
                foreach (var j in _graph.Edges(i)) {
                    var u = _database [j];
                    var posTo = u.transform.position;
                    DrawRay (posFrom, posTo);
                }                    
            }
        }
        #endregion

        #region Public
        public void DrawRange (Vision v) {
            var tr = v.transform;
            _fig.DrawFan (tr.position, Quaternion.LookRotation (-tr.up, tr.forward), v.range * Vector3.one, colorSpot, -v.angle, v.angle);
        }
        public void DrawRay (Vector3 posFrom, Vector3 posTo) {
            Gizmos.color = colorInsight;
            Gizmos.DrawLine (posFrom, posTo);
        }
        #endregion

        #region Static
        public static Recon Instance { get; protected set; }
        public static void Add(Vision vision) {
            if (Instance == null)
                Debug.LogFormat ("There is no instance");
            else
                Instance._database.Add (vision);
        }
        public static bool Remove(Vision vision) {
            return (Instance != null && Instance._database.Remove (vision) >= 0);
        }
        #endregion

    }
}
