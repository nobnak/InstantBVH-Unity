using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Reconnitioning.SpacePartition;
using System.Text;

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

        #region Static
        public static StringBuilder List(StringBuilder buf, BVH<Value> t, int depth) {
            if (t == null)
                return buf;

            for (var i = 0; i < depth; i++)
                buf.Append ("        ");
            buf.AppendFormat ("({0},{1})={2}", t.offset, t.length, t.bb);
            if (t.IsLeaf ())
                buf.AppendFormat (" Values={0}", t.Values.First.Value.id);
            buf.AppendLine ();

            for (var i = 0; i < 2; i++)
                List (buf, t.ch [i], depth+1);

            return buf;
        }
        #endregion

        public class Value {
            public int id;
        }
    }
}