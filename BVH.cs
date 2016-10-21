using UnityEngine;
using System.Collections.Generic;

namespace Reconnitioning {
    
    public class BVH<Value> {


        public class Builder {
            public readonly List<Value> Vals = new List<Value>();

            public BVH<Value> Build(BVH<Value> bvh, System.Func<Value, Vector3> Pos) {
                var poss = new Vector3[Vals.Count];
                for (var i = 0; i < Vals.Count; i++)
                    poss[i] = Pos (Vals [i]);



                return bvh;
            }
        }
    }
}
