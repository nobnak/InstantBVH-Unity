using System.Collections;
using System.Collections.Generic;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using UnityEngine;

namespace SimpleBVH.Comparers {

    public class ComparerY<T> : IComparer<int> where T : IBV {

        public readonly BVH<T> BVH;

        public ComparerY(BVH<T> bvh) {
            this.BVH = bvh;
        }

        public int Compare(int x, int y) {
            var a = BVH.Objects[BVH.Indices[x]].Bounds.Center;
            var b = BVH.Objects[BVH.Indices[y]].Bounds.Center;
            var diff = a.y - b.y;
            return (diff < 0) ? -1 : ((diff > 0) ? 1 : 0);
        }
    }
}
