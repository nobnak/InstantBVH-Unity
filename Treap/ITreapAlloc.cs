using UnityEngine;
using System.Collections;

namespace Reconnitioning.Treap {
    
    public interface ITreapAlloc<Value> {
        float Priority();
        Treap<Value> Create(int key);
        void Destroy (Treap<Value> t);
    }
}