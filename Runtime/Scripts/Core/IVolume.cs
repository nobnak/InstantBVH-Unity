using UnityEngine;
using System.Collections;
using nobnak.Gist.Extensions.AABB;
using Recon.BoundingVolumes;
using Recon.BoundingVolumes.Behaviour;
using nobnak.Gist.Primitive;
using nobnak.Gist.Extensions.ComponentExt;

namespace Recon.Core {
    
    public interface IVolume<V> {
		FastBounds Bounds { get; }
		V Value { get; }
    }
}