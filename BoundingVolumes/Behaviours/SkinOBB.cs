using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;
using nobnak.Gist.Extensions.AABB;

namespace Recon.BoundingVolumes.Behaviour {
    
    public class SkinOBB : AbstractMeshOBB {
        SkinnedMeshRenderer _attachedSkinmesh;

        #region Skinned Mesh
        public SkinnedMeshRenderer AttachedSkinMesh {
            get { return _attachedSkinmesh; }
        }
        #endregion

        #region implemented abstract members of IConvex
        public override bool StartConvex () {
            return (_attachedSkinmesh == null ? 
                (_attachedSkinmesh = GetComponentInChildren<SkinnedMeshRenderer> ()) != null : true);
        }
        #endregion

        #region implemented abstract members of AbstractMeshOBB
        protected override Transform RootTransform () {
            return _attachedSkinmesh.rootBone;
        }
        protected override Bounds LocalBounds () {
            return _attachedSkinmesh.localBounds;
        }
        #endregion
    }
}
