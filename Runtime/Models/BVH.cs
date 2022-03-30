using SimpleBVH.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBVH.Models {

    public class BVH<T> where T : IBV {

        public readonly int K_Ary;
        public readonly int LeafFrom;

        public readonly IList<MinMaxAABB> Heap;
        public readonly IList<int> Indices;
        public readonly IList<T> Objects;

        public BVH(
            IList<MinMaxAABB> heap, 
            IList<int> indices, 
            IList<T> objects, 
            int k_ary, int leafFrom) {

            this.Heap = heap;
            this.Indices = indices;
            this.Objects = objects;
            this.K_Ary = k_ary;
            this.LeafFrom = leafFrom;
        }
    }
}
