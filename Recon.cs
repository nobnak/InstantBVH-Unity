using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gist;

namespace Reconnitioning {
    [ExecuteInEditMode]
    public class Recon : MonoBehaviour {

        Dataset<IVolume> _database;

        GLFigure _fig;

        void OnEnable() {
            _database = new Dataset<IVolume> ();
            _fig = new GLFigure ();
            Instance = this;
        }
        void OnDisable() {
            if (_fig != null) {
                _fig.Dispose ();
                _fig = null;
            }
        }

        #region Public
        public GLFigure Fig { get { return _fig; } }
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
