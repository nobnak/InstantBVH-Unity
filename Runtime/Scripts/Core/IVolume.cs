using nobnak.Gist.Primitive;

namespace Recon.Core {

	public interface IVolume<V> {
		FastBounds Bounds { get; }
		V Value { get; }
    }
}