using UnityEngine;
using System.Collections.Generic;

namespace Reconnitioning {
    
    public class BVH<Value> {


        public class Builder {
			public BVH<Value> Build(BVH<Value> bvh, IList<Bounds> Bous, IList<Value> Vals) {
				var min = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
				var max = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
				for (var i = 0; i < Bous.Count; i++) {
					var p = Bous [i].center;
					for (var j = 0; j < 3; j++) {
						min [j] = Mathf.Min (min [j], p [j]);
						max [j] = Mathf.Max (max [j], p [j]);
					}
				}

				var size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
				var sizeInv = new Vector3(1f / size.x, 1f / size.y, 1f / size.z);

				var ids = new System.UInt64[Bous.Count];
				for (var i = 0; i < ids.Length; i++) {
					var p = Vector3.Scale(Bous[i].center - min, sizeInv);
					ids[i] = MortonCode.Encode(p.x, p.y, p.z);
				}



                return bvh;
            }
        }
    }
}
