using UnityEngine;
using System.Collections;

namespace Reconnitioning.Treap {

    public class SimpleTreapAlloc<Value> : ITreapAlloc<Value> {
        public SimpleTreapAlloc() {}

        #region ITreapAlloc implementation
        public float Priority () { return Random.value; }
        public Treap<Value> Create (int key) {
            return new Treap<Value> (key, Priority());
        }
        public void Destroy (Treap<Value> t) {}
        #endregion
    }
}
