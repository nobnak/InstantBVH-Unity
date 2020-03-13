using UnityEngine;
using System.Collections;

namespace Recon.TreapSys {

	public interface ITreapController<Value> {
        bool TryGet(ulong key, out Treap<Value> n);
        Treap<Value> Insert(ulong key);
        ITreapController<Value> Clear();
    }
}
