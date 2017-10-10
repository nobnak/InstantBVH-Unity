using Gist.BoundingVolume;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recon.SpacePartition {

    public class BinnedSAH {

        protected int k = 0;
        protected AABB[] bb;
        protected List<int>[] bins;

        protected float[] lefts;
        protected float[] rights;

        public void Reset(int binCount) {
            ResetBB(binCount);
            ResetBins(binCount);
            ResetHeuristic(binCount);
        }

        private void ResetHeuristic(int binCount) {
            System.Array.Resize(ref lefts, binCount);
            System.Array.Resize(ref rights, binCount);
            System.Array.Clear(lefts, 0, lefts.Length);
            System.Array.Clear(rights, 0, rights.Length);
        }
        private void ResetBB(int binCount) {
            var prevBBCount = k;
            System.Array.Resize(ref bb, binCount);
            for (var i = prevBBCount; i < binCount; i++)
                bb[i] = new AABB();
        }
        private void ResetBins(int binCount) {
            var prev = k;
            System.Array.Resize(ref bins, binCount);
            for (var i = 0; i < prev; i++)
                bins[i].Clear();
            for (var i = prev; i < binCount; i++)
                bins[i] = new List<int>();
        }
    }
}
