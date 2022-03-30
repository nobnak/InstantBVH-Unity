using SimpleBVH.Comparers;
using SimpleBVH.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace SimpleBVH.Models {

    public class BVH<T> where T : IBV {

        public readonly int K_Ary;
        public readonly int N_Inners;
        public readonly int N;

        public readonly IList<MinMaxAABB> Heap;
        public readonly List<int> Indices;
        public readonly IList<T> Objects;

        public readonly FuncComparer<int> CompareOnXAxis;
        public readonly FuncComparer<int> CompareOnYAxis;
        public readonly FuncComparer<int> CompareOnZAxis;

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

            this.CompareOnXAxis = new FuncComparer<int>((i, j) => {
                var a = Objects[i].Bounds.Center;
                var b = Objects[j].Bounds.Center;
                return (int)math.sign(a.x - b.x);
            });
            this.CompareOnYAxis = new FuncComparer<int>((i,j) => {
                var a = Objects[i].Bounds.Center;
                var b = Objects[j].Bounds.Center;
                return (int)math.sign(a.y - b.y);
            });
            this.CompareOnZAxis = new FuncComparer<int>((i, j) => {
                var a = Objects[i].Bounds.Center;
                var b = Objects[j].Bounds.Center;
                return (int)math.sign(a.z - b.z);
            });
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
