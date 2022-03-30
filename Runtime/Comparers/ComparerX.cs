using System.Collections;
using System.Collections.Generic;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using Unity.Mathematics;
using UnityEngine;

namespace SimpleBVH.Comparers {

    public class ComparerX : BaseComparer<int> {

        public ComparerX(System.Func<int, float3> GetPosition) : base(GetPosition) {
        }

        public override int Compare(int i, int j) {
            var a = GetPosition(i);
            var b = GetPosition(j);
            var diff = a.x - b.x;
            return (diff < 0) ? -1 : ((diff > 0) ? 1 : 0);
        }
    }
}
