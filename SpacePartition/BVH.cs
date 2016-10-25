using UnityEngine;
using System.Collections.Generic;
using Reconnitioning.Treap;

namespace Reconnitioning.SpacePartition {
    
	public class BVH<Value> where Value : class {
		public readonly BVH<Value>[] ch = new BVH<Value>[2];

		public Bounds bb;
		public Value value;

		public BVH() {
		}
		public BVH(Bounds bb, Value val) {
			Reset (bb, val);
		}

		public BVH<Value> Reset(Bounds bb, Value val) {
			this.bb = bb;
			this.value = val;
			return this;
		}
		public bool Leaf() {
			return ch [0] == null && ch [1] == null;
		}
    }
}
