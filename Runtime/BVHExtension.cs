using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace SimpleBVH {

    public static class BVHExtension {

        /*
         * https://en.wikipedia.org/wiki/D-ary_heap
         * k-ary heap
         * 
         * Index starts from i = 0, for any node i,
         * Parent: ⌊(i-1)/k⌋
         * Children: [ki+1, ki+k]
         * 
         * If there are n leaves, 
         * leaf node starts from depth l = ⌊log_k(n)⌋, {depth starts from 0}
         * number of  leaves in depth l : n_l = k^l - ⌈(n-k^l)/(k-1)⌉
         * total nodes till depth l : s_l = (k^(l+1)-1)/(k-1)
         * total inner nodes : s_l - n_l
         */

        public static BVH<T> Build<T>(IList<T> obj, int k_ary = 2) where T : IBV {

            var n = obj.Count;
            var d_l = (int)math.floor(math.log2(n) / math.log2(k_ary));
            var k_l = (int)math.pow(k_ary, d_l);
            var n_l = k_l - (int)math.ceil((float)(n - k_l) / (k_l - 1));

            var vbh = new BVH<T>(
                new List<MinMaxAABB>(),
                Enumerable.Range(0, obj.Count).ToList(),
                obj,
                k_ary, n_l);
            return vbh;
        }
    }
}
