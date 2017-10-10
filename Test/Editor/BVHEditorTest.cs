using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Recon.SpacePartition;
using System.Text;

namespace Recon {
        
    public class BVHEditorTest {

    	[Test]
    	public void EditorTest() {
            var delta = 1e-4f;
            var n = 4;

            var bounds = new Bounds[n];
            var vals = new Value[n];
            for (var i = 0; i < n; i++) {
                bounds [i] = new Bounds (Vector3.one * i, Vector3.one);
                vals [i] = new Value (){ id = i };
            }

            var bvh = new MortonBVHController<Value> ();
            bvh.Build (bounds, vals);

            var nodeCount = bvh.Count ();
            var valueCount = bvh.CountValues ();
            bvh.Build (bounds, vals);
            Assert.AreEqual(nodeCount, bvh.Count ());
            Assert.AreEqual (valueCount, bvh.CountValues ());

            Assert.AreEqual (0, bvh.Root.offset);
            Assert.AreEqual (n, bvh.Root.length);
            for (var i = 0; i < 3; i++) {
                Assert.AreEqual (1.5f, bvh.Root.bb.center [i], delta);
                Assert.AreEqual (2f, bvh.Root.bb.extents [i], delta);
            }

            Assert.AreEqual (0, bvh.Root.ch [0].offset);
            Assert.AreEqual (2, bvh.Root.ch [0].length);

            Assert.AreEqual (2, bvh.Root.ch [1].offset);
            Assert.AreEqual (2, bvh.Root.ch [1].length);

            for (var j = 0; j < n; j++) {
                var s = (j & 2) != 0 ? 1 : 0;
                var t = (j & 1) != 0 ? 1 : 0;

                Assert.AreEqual (1, bvh.Root.ch [s].ch [t].Values.Count);
                Assert.AreEqual (j, bvh.Root.ch [s].ch [t].Values.First.Value.id);
                    
                for (var i = 0; i < 3; i++) {
                    Assert.AreEqual (j, bvh.Root.ch [s].ch [t].bb.center [i], delta);
                    Assert.AreEqual (1f, bvh.Root.ch [s].ch [t].bb.size [i], delta);
                }
            }
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