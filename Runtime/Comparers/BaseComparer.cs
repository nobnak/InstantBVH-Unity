using System.Collections;
using System.Collections.Generic;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using Unity.Mathematics;
using UnityEngine;

namespace SimpleBVH.Comparers {

    public abstract class BaseComparer<T> : IComparer<T> {

        public readonly System.Func<int, float3> GetPosition;

        public BaseComparer(System.Func<int, float3> GetPosition) {
            this.GetPosition = GetPosition;
        }

        public abstract int Compare(T x, T y);
    }
}
