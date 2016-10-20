using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Reconnitioning {
    public class Treap<Value> {
        Node<Value> _root;
        Stack<Node<Value>> _cache;

        public Treap() {
            _cache = new Stack<Node<Value>> ();
        }

        int Count(Node<Value> t) {
            return (t == null ? 0 : t.count);
        }
        Node<Value> Update(Node<Value> t) {
            t.count = Count (t.left) + Count (t.right) + 1;
            return t;
        }

        #region Merge / Split
        Node<Value> Merge(Node<Value> l, Node<Value> r) {
            if (l == null)
                return r;
            else if (r == null)
                return l;

            if (l.pri > r.pri) {
                l.right = Merge (l.right, r);
                return Update (l);
            } else {
                r.left = Merge (l, r.left);
                return Update (r);
            }
        }
        Pair<Node<Value>> Split(Node<Value> t, int i) {
            if (t == null)
                return new Pair<Node<Value>> ();

            if (i <= Count (t.left)) {
                var s = Split (t.left, i);
                t.left = s.right;
                return new Pair<Node<Value>> (s.left, Update(t));                    
            } else {
                var s = Split (t.right, i - (Count (t.left) + 1));
                t.right = s.left;
                return new Pair<Node<Value>> (Update(t), s.right);
            }
        }
        #endregion

        #region Create, Destroy
        Node<Value> Create(int key) {
            return new Node<Value> (key, Random.value);
        }
        void Destroy(Node<Value> t) {
            _cache.Push (t.Remove());
            if (t.left != null)
                Destroy (t.left);
            if (t.right != null)
                Destroy (t.right);
        }
        #endregion
    }

    #region Class
    public struct Pair<V> {
        public readonly V left, right;

        public Pair(V l, V r) {
            this.left = l;
            this.right = r;
        }
    }

    public class Node<Value> {
        public int key;
        public float pri;
        public Node<Value> left, right;
        public int count;

        public readonly LinkedList<Value> values;

        public Node() : this(default(int), default(float)) {}
        public Node(int key, float pri) {
            Reset (key, pri);
            this.values = new LinkedList<Value> ();
        }

        public Node<Value> Reset (int key, float pri) {
            this.key = key;
            this.pri = pri;
            return Remove();
        }

        public Node<Value> Remove () {
            this.left = this.right = null;
            this.count = 1;
            return this;
        }
    }
    #endregion
}
