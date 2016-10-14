using UnityEngine;
using System.Collections;

namespace Reconnitioning {

    public class GraphBuilder {
        public readonly Dataset<Vision> Dataset;
        public readonly Graph Graph;
        int _frame = int.MinValue;

        public GraphBuilder(Dataset<Vision> dataset, Graph graph) {
            this.Dataset = dataset;
            this.Graph = graph;
        }

        public void Build(bool checkTimestamp = true) {
            if ((checkTimestamp && _frame != Time.frameCount) || Dataset.Reset ()) {
                _frame = Time.frameCount;
                BuildImmediate ();
            }
        }
        public void BuildImmediate() {
            Graph.Clear (Dataset.Count);
            var count = Dataset.Count;
            for (var i = 0; i < count; i++) {
                var v = Dataset [i];
                var posFrom = v.transform.position;
                var rangeSqr = v.range * v.range;
                var forward = v.transform.forward;
                var angle = v.angle * Mathf.Deg2Rad;
                for (var j = 0; j < count; j++) {
                    if (i == j)
                        continue;
                    var u = Dataset [j];
                    var posTo = u.transform.position;
                    var view = posTo - posFrom;
                    if (view.sqrMagnitude > rangeSqr)
                        continue;
                    var viewAngle = Mathf.Acos (Vector3.Dot (forward, view.normalized));
                    if (viewAngle > angle)
                        continue;

                    Graph.Add (i, j);
                }
            }
        }
    }
}