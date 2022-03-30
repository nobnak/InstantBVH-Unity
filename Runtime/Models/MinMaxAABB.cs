using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace SimpleBVH.Models {

    public struct MinMaxAABB {

        public readonly float3 Min;
        public readonly float3 Max;

        public MinMaxAABB(float3 min, float3 max) {
            this.Min = min;
            this.Max = max;
        }

        public readonly float3 Center => (Min + Max) * 0.5f;
        public readonly float3 Extents => Max - Min;

        public readonly float SurfaceArea {
            get {
                float3 diff = Max - Min;
                return 2 * math.dot(diff, diff.yzx);
            }
        }
    }

    public static class MinMaxAABBExtension {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MinMaxAABB Encapsulate(this MinMaxAABB a, MinMaxAABB b)
            => new MinMaxAABB(math.min(a.Min, b.Min), math.max(a.Max, b.Max));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MinMaxAABB Encapsulate(this MinMaxAABB a, float3 p)
            => new MinMaxAABB(math.min(a.Min, p), math.max(a.Max, p));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this MinMaxAABB a, float3 p)
            => math.all(p >= a.Min & p <= a.Max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this MinMaxAABB a, MinMaxAABB b) 
            => math.all((a.Min <= b.Min) & (a.Max >= b.Max));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Overlaps(this MinMaxAABB a, MinMaxAABB b)
            => math.all(a.Max >= b.Min & a.Min <= b.Max);
    }
}
