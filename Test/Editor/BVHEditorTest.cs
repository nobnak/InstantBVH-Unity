using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Reconnitioning.SpacePartition;

namespace Reconnitioning {
        
    public class BVHEditorTest {

    	[Test]
    	public void EditorTest() {
            var n = 4;

            var bounds = new Bounds[n];
            var vals = new Value[n];
            for (var i = 0; i < n; i++) {
                bounds [i] = new Bounds (Vector3.one * i, Vector3.one);
                vals [i] = new Value (){ id = i };
            }

            var bvh = new BVHController<Value> ();
            bvh.Build (bounds, vals);


    	}

        public class Value {
            public int id;
        }
    }
}