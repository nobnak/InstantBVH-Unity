using UnityEngine;
using System.Collections;

namespace Reconnitioning.Treap {

    public interface ITreapController<Value> {
        bool TryGet(int key, out Treap<Value> n);
        Treap<Value> Insert(int key);
    }
}