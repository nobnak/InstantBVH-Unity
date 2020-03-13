using nobnak.Gist.Pooling;

namespace Recon.TreapSys {

    public class CachedTreapController<Value> : ITreapController<Value> {
        public Treap<Value> Root { get { return _root; } }

        Treap<Value> _root;
        MemoryPool<Treap<Value>> _pool = new MemoryPool<Treap<Value>>(
            ()=>new Treap<Value>(), (v)=>v.Clear(), (v)=> { });

        #region ITreapController implementation
        public bool TryGet (ulong key, out Treap<Value> n) {
            return Treap<Value>.TryGet (_root, key, out n);
        }
        public Treap<Value> Insert (ulong key) {
            return Treap<Value>.Insert (ref _root, key, _pool);
        }
        public ITreapController<Value> Clear() {
            Treap<Value>.Clear (Root, _pool);
            return this;
        }
        #endregion
    }
}