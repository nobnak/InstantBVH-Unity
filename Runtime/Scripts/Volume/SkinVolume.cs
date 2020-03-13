using UnityEngine;
using System.Collections;
using Recon.BoundingVolumes;
using Recon.Extension;

namespace Recon.VolumeSys {

    public class SkinVolume : Volume {
        SkinnedMeshRenderer _attachedSkinmesh;

        #region implemented abstract members of IConvex
        public override bool StartConvex () {
			return AttachedSkin != null;
		}
		#endregion

		#region implemented abstract members of AbstractMeshOBB
		protected override Transform RootTransform () {
            var rootBone = AttachedSkin.rootBone;
            return (rootBone == null ? AttachedSkin.transform : rootBone);
        }
        protected override Bounds LocalBounds () {
            return AttachedSkin.localBounds;
        }
		#endregion

		#region member
		private SkinnedMeshRenderer AttachedSkin {
			get {
				if (_attachedSkinmesh == null)
					_attachedSkinmesh = GetComponentInChildren<SkinnedMeshRenderer>();
				return _attachedSkinmesh;
			}
		}
		#endregion
	}
}
