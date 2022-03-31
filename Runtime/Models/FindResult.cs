using System.Collections;
using System.Collections.Generic;
using SimpleBVH.Interfaces;
using UnityEngine;

namespace SimpleBVH.Models {

    public struct FindResult<T> where T : IBV {

        public readonly int AtIndices;
        public readonly int AtObjects;
        public readonly T Obj;

        public FindResult(int atIndices, int atObjects, T obj) {
            this.AtIndices = atIndices;
            this.AtObjects = atObjects;
            this.Obj = obj;
        }
    }
}
