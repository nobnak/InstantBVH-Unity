using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Reconnitioning {

    public class Graph {
        List<List<int>> _neighbors = new List<List<int>>();

        public void Clear(int vertexCount) {
            for (var i = 0; i < _neighbors.Count; i++)
                _neighbors [i].Clear ();
            for (var i = _neighbors.Count; i < vertexCount; i++)
                _neighbors.Add(new List<int> ());
        }
        public void Add(int i, int j) {
            _neighbors [i].Add (j);
        }
        public IEnumerable<int> Edges(int i) {
            foreach (var j in _neighbors[i])
                yield return j;
        }
    }
}