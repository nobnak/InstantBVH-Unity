using System.Collections;
using System.Collections.Generic;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using Unity.Mathematics;
using UnityEngine;

namespace SimpleBVH.Comparers {

    public class FuncComparer<T> : IComparer<T> {

        public readonly System.Func<T, T, int> DoCompare;

        public FuncComparer(System.Func<T, T, int> DoCompare) {
            this.DoCompare = DoCompare;
        }

        public int Compare(T i, T j) => DoCompare(i, j);
    }
}
