using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;

namespace Recon.VolumeSys {

    public class MeshVolume : Volume {
        MeshFilter _attachedMFilter;

        #region implemented abstract members of IConvex
        public override bool StartConvex () {
            return (_attachedMFilter == null ? 
                (_attachedMFilter = GetComponent<MeshFilter> ()) != null : true);
        }
        #endregion

        #region implemented abstract members of AbstractMeshOBB
        protected override Transform RootTransform () {
            return _attachedMFilter.transform;
        }
        protected override Bounds LocalBounds () {
            return _attachedMFilter.sharedMesh.bounds;
        }
        #endregion
    }
}
