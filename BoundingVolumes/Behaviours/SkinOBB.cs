using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;

namespace Recon.BoundingVolumes.Behaviour {
    
    public class SkinOBB : ConvexBehaviour {
        public Color color = Color.white;

        SkinnedMeshRenderer _attachedSkinmesh;
        OBB _obb;

        protected void OnDrawGizmos() {
            if (!isActiveAndEnabled)
                return;

            AssureUpdateConvex ();
            Gizmos.color = color;
            _obb.DrawGizmos ();

        }

        #region Skinned Mesh
        public SkinnedMeshRenderer AttachedSkinMesh {
            get { return _attachedSkinmesh; }
        }
        #endregion
        #region implemented abstract members of ConvexBehaviour
        public override IConvexPolyhedron GetConvexPolyhedron () {
            AssureUpdateConvex ();
            return _obb;
        }
        public override bool StartConvex () {
            return (_attachedSkinmesh == null ? 
                (_attachedSkinmesh = GetComponentInChildren<SkinnedMeshRenderer> ()) != null : true);
        }
        public override bool UpdateConvex () {
            return (_obb = OBB.Create (_attachedSkinmesh.rootBone, _attachedSkinmesh.localBounds)) != null;
        }
        #endregion
    }
}
