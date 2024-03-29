﻿using SimpleBVH.Intenals;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace SimpleBVH {

    public static class BVHExtension {
        public static readonly ProfilerMarker P_BvhConstruction 
            = new ProfilerMarker("SimpleBVH.Build.BvhConstruction");

        public readonly static string[] MARKERS = new string[] {
            "SimpleBVH.Build.EncapsulateBounds",
            "SimpleBVH.Build.SortInterval",
            "SimpleBVH.Build.BvhConstruction",
        };

        public static BVH<T> Build<T>(this IList<T> obj, int k_ary = 2) where T : IBV {

            var n = obj.Count;
            var n_inner = k_ary.TotalInnerNodes(n);

            P_BvhConstruction.Begin();
            var bvh = new BVH<T>(
                new MinMaxAABB[n_inner],
                Enumerable.Range(0, obj.Count).ToList(),
                obj,
                k_ary, n, n_inner);
            P_BvhConstruction.End();

            bvh._Build(k_ary, 0, 0, n);

            return bvh;
        }

        public static IEnumerable<FindResult<T>> Find<T>(this BVH<T> bvh, MinMaxAABB bounds) where T : IBV {

            foreach (var v in bvh._Find(bounds, 0))
                yield return v;
        }
    }
}
