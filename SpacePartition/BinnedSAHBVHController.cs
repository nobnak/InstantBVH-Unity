using UnityEngine;
using System.Collections;
using Gist;
using Gist.Extensions.AABB;
using Recon.Treap;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Recon.SpacePartition {

    public class BinnedSAHBVHController<Value> : BaseBVHController<Value> where Value : class {
        public const int K = 8;

        public override BaseBVHController<Value> Build(IList<Bounds> Bous, IList<Value> Vals) {
            Clear ();



            return this;
        }


    }
}
