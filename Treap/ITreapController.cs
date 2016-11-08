using UnityEngine;
using System.Collections;

namespace Recon.Treap {

    public interface ITreapController<Value> {
        bool TryGet(ulong key, out Treap<Value> n);
        Treap<Value> Insert(ulong key);
        ITreapController<Value> Clear();
    }
}
