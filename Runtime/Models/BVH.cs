using SimpleBVH.Comparers;
using SimpleBVH.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SimpleBVH.Models {

    public class BVH<T> where T : IBV {

        public readonly int K_Ary;
        public readonly int N_Inners;
        public readonly int N;

        public readonly IList<MinMaxAABB> Heap;
        public readonly List<int> Indices;
        public readonly IList<T> Objects;

        public readonly IComparer<int> CompareOnXAxis;
        public readonly IComparer<int> CompareOnYAxis;
        public readonly IComparer<int> CompareOnZAxis;

        public BVH(
            IList<MinMaxAABB> heap, 
            List<int> indices, 
            IList<T> objects, 
            int k_ary, int n, int nInners) {

            this.Heap = heap;
            this.Indices = indices;
            this.Objects = objects;

            this.K_Ary = k_ary;
            this.N = n;
            this.N_Inners = nInners;

            this.CompareOnXAxis = new ComparerX<T>(this);
            this.CompareOnYAxis = new ComparerY<T>(this);
            this.CompareOnZAxis = new ComparerZ<T>(this);
        }

        #region interfac

        #region object
        public override string ToString() {
            var tmp = new StringBuilder($"{GetType().Name}: {K_Ary}-ary, n={N}, inner={N_Inners}\n");
            tmp.AppendLine($"Heap: {string.Join(", ", Heap)}");
            tmp.AppendLine($"Indices: {string.Join(", ", Indices)}");
            tmp.AppendLine($"Objects: {string.Join(", ", Objects)}");
            return tmp.ToString();
        }
        #endregion

        #endregion
    }
}
