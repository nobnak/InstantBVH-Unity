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

        #region interface
        #region object
        public override string ToString() {
            return $"[{Min}, {Max}]";
        }
        #endregion

        public readonly float3 Center => (Min + Max) * 0.5f;
        public readonly float3 Extents => Max - Min;

        public readonly float SurfaceArea {
            get {
                float3 diff = Max - Min;
                return 2 * math.dot(diff, diff.yzx);
            }
        }
        #endregion

        #region static
        public static MinMaxAABB Empty {
            get => new MinMaxAABB(
                new float3(float.MaxValue, float.MaxValue, float.MaxValue),
                new float3(float.MinValue, float.MinValue, float.MinValue));
        }
        #endregion
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(this MinMaxAABB a, MinMaxAABB b) {
            var dmin = a.Min - b.Min;
            var dmax = a.Max - b.Max;
            var eps = 1e-3f;
            return math.dot(dmin, dmin) < eps && math.dot(dmax, dmax) < eps;
        }
    }
}
