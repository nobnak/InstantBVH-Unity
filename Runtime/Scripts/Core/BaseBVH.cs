using nobnak.Gist.Pooling;
using nobnak.Gist.Primitive;
using Recon.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Recon.Core {

    public abstract class BaseBVH<Value> : System.IDisposable {
        public Node<Value> Root { get { return _root; } }

		protected List<IVolume<Value>> volumes = new List<IVolume<Value>>();
        protected Node<Value> _root;
        protected MemoryPool<Node<Value>> _pool = new MemoryPool<Node<Value>> (
            ()=>new Node<Value>(), (v)=>v.Clear(), (v)=> { });

		#region abstract
		public abstract BaseBVH<Value> Update();
		#endregion

		#region interface
		#region IDisposable
		public virtual void Dispose() {
		}
		#endregion

		public virtual void Add(IVolume<Value> v) {
			volumes.Add(v);
		}
		public virtual void Remove(IVolume<Value> v) {
			volumes.Remove(v);
		}
		public virtual BaseBVH<Value> Clear() {
            Node<Value>.Clear (_root, _pool);
            return this;
        }
		public virtual IEnumerable<Value> Intersect(FastBounds bb) {
			return Intersect(_root, bb);
		}
		public virtual IList<Value> Intersect(FastBounds bb, IList<Value> result) {
			return Intersect(_root, bb, result);
		}
		public virtual int Count() { return Count(_root); }
		public virtual int CountValues() {
			return CountValues(_root);
		}
		#endregion

		#region Static
		public static int Count(Node<Value> t) {
            if (t == null)
                return 0;
            return 1 + Count (t.ch [0]) + Count (t.ch [1]);
        }
        public static int CountValues(Node<Value> t) {
            return t == null ? 0 : (t.Values.Count + CountValues (t.ch [0]) + CountValues (t.ch [1]));
        }
        public static IEnumerable<Value> Intersect(Node<Value> t, FastBounds bb) {
            if (t == null || !t.bb.Intersects (bb))
                yield break;

			if (!t.IsLeaf())
				foreach (var s in t.ch)
					if (s != null)
						foreach (var r in Intersect(s, bb))
							yield return r;

            foreach (var v in t.Values)
                if (v != null)
                    yield return v;

        }
		public static IList<Value> Intersect(Node<Value> t, FastBounds bb, IList<Value> result) {
			if (t != null && t.bb.Intersects(bb)) {
				Intersect(t.ch[0], bb, result);
				Intersect(t.ch[1], bb, result);

				foreach (var v in t.Values)
					if (v != null)
						result.Add(v);
			}
			return result;
		}
        public static IList<int> Swap (IList<int> list, int i, int j) {
            var tmp = list [i];
            list [i] = list [j];
            list [j] = tmp;
            return list;
        }
        #endregion
        #region Gizmo
        public virtual void DrawBounds(int depthFrom, int length) {
            Node<Value>.DrawBounds (_root, -depthFrom, length);
        }
		#endregion
	}
}
