using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace SimpleBVH.Intenals {

    public static class BVHExtensionInternal {

        /*
         * https://en.wikipedia.org/wiki/D-ary_heap
         * k-ary heap
         * 
         * Index starts from i = 0, for any node i,
         * Parent: ⌊(i-1)/k⌋
         * Children: [ki+1, ki+k]
         * 
         * If there are n leaves, 
         * leaf node starts from depth d = ⌊log_k(n)⌋, {depth starts from 0}
         * number of  leaves in depth d : n_l = k^d - ⌈(n-k^d)/(k-1)⌉
         * total nodes till depth d : s_l = (k^(d+1)-1)/(k-1)
         * total inner nodes : s_l - n_l
         */
        public static void _Build<T>(this BVH<T> bvh, int k_ary, int index, int start, int end) where T : IBV {
            if (index >= bvh.N_Inners || start >= bvh.N) return;
            if (end > bvh.N) end = bvh.N;
            if ((end - start) <= 1) return;

            var b = MinMaxAABB.Empty;
            for (var i = start; i < end; i++)
                b = b.Encapsulate(bvh.Objects[bvh.Indices[i]].Bounds);
            bvh.Heap[index] = b;

            var comp = FindBestComparer(bvh, b);
            bvh.Indices.Sort(start, end - start, comp);

            var stride = math.max(k_ary, (int)math.ceil((float)(end - start) / k_ary));
            var leftmostChild = k_ary.LeftMostChild(index);
            for (var i = 0; i < k_ary; i++) {
                bvh._Build(k_ary, leftmostChild + i, start + stride * i, start + stride * (i + 1));
            }
        }
        public static IEnumerable<FindResult<T>> _Find<T>(this BVH<T> bvh, MinMaxAABB bounds, int i) where T : IBV {
            if (i >= (bvh.N_Inners + bvh.N)) yield break;

            if (i >= bvh.N_Inners) {
                i -= bvh.N_Inners;
                var j = bvh.Indices[i];
                var bv = bvh.Objects[j];
                if (bv.Bounds.Overlaps(bounds))
                    yield return new FindResult<T>(i, j, bv);
                yield break;
            }

            if (bvh.Heap[i].Overlaps(bounds)) {
                var start = bvh.K_Ary.LeftMostChild(i);
                var end = start + bvh.K_Ary;
                for (var j = start; j < end; j++)
                    foreach (var v in bvh._Find(bounds, j))
                        yield return v;
            }
        }

        public static IComparer<int> FindBestComparer<T>(BVH<T> bvh, MinMaxAABB b) where T : IBV {
            IComparer<int> comp;
            var ext = b.Extents;
            if (ext.x >= ext.y) {
                if (ext.x >= ext.z)
                    comp = bvh.CompareOnXAxis;
                else
                    comp = bvh.CompareOnYAxis;
            } else {
                if (ext.y >= ext.z)
                    comp = bvh.CompareOnYAxis;
                else
                    comp = bvh.CompareOnZAxis;
            }
            return comp;
        }

        public static int Parent(this int k_ary, int i) => (int)math.floor((i - 1) / k_ary);
        public static int LeftMostChild(this int k_ary, int i) => k_ary * i + 1;
        public static int RightMostChild(this int k_ary, int i) => k_ary * i + k_ary;

        public static int TotalInnerNodes(this int k_ary, int n) {
            var d = k_ary.MinDepthOfLeaf(n);
            return k_ary.TotalCapacityTillDepth(d) - k_ary.CountLeavesAtMinDepth(n, d);
        }
        public static int TotalCapacityTillDepth(this int k_ary, int d)
            => ((int)math.pow(k_ary, d + 1) - 1) / (k_ary - 1);
        public static int CapacityAtDepth(this int k_ary, int d_l)
            => (int)math.pow(k_ary, d_l);
        public static int MinDepthOfLeaf(this int k_ary, int n) 
            => (int)math.floor(math.log2(n) / math.log2(k_ary));

        public static int CountLeavesAtMinDepth(this int k_ary, int n, int d) {
            var c_l = k_ary.CapacityAtDepth(d);
            return c_l - CountInnersAtMinDepth(k_ary, n, c_l);
        }
        public static int CountInnersAtMinDepth(this int k_ary, int n, int c_l)
            => (int)math.ceil((float)(n - c_l) / (k_ary - 1));
        public static int CountLeavesAtMinDepth(this int k_ary, int n)
            => k_ary.CountLeavesAtMinDepth(n, k_ary.MinDepthOfLeaf(n));
    }
}
