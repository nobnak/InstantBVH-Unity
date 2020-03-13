using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recon.Extension {

    public static class SkinnedMeshRendererExtension {
        public static Transform RootBone(this SkinnedMeshRenderer skin) {
            return (skin.rootBone != null ? skin.rootBone : skin.transform);
        }
	}
}
