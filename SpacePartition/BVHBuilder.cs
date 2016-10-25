﻿using System.Collections.Generic;
using Reconnitioning.Treap;
using UnityEngine;

namespace Reconnitioning.SpacePartition {

    public class Builder<Value> where Value : class {
        CachedTreapController<BVH<Value>> _treap = new CachedTreapController<BVH<Value>>();

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

            _treap.Clear ();
            for (var i = 0; i < Bous.Count; i++) {
                var bb = Bous [i];
                var val = Vals [i];
                var p = Vector3.Scale(bb.center - min, sizeInv);
                var id = MortonCode.Encode(p.x, p.y, p.z);
                Treap<BVH<Value>> t;
                if (!_treap.TryGet (id, out t))
                    t = _treap.Insert (id);
                t.Values.AddLast (new BVH<Value> (bb, val));
            }

            return bvh;
        }
    }
}
