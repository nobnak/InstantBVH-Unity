using nobnak.Gist.Primitive;
using NUnit.Framework;
using Recon.Core;
using System.Text;
using UnityEngine;

namespace Recon.Editor.Test {

	public class BVHEditorTest {

    	[Test]
    	public void EditorTest() {
            var delta = 1e-4f;
            var n = 4;

			var bvh = new MortonBVH<int>();
            for (var i = 0; i < n; i++) {
                var b = new Bounds (Vector3.one * i, Vector3.one);
                var v = new Volume(b, i);
				bvh.Add(v);
            }
            bvh.UpdateTree ();

            var nodeCount = bvh.Count ();
            var valueCount = bvh.CountValues ();
            bvh.UpdateTree ();
            Assert.AreEqual(nodeCount, bvh.Count ());
            Assert.AreEqual (valueCount, bvh.CountValues ());

            Assert.AreEqual (0, bvh.Root.offset);
            Assert.AreEqual (n, bvh.Root.length);
            for (var i = 0; i < 3; i++) {
                Assert.AreEqual (1.5f, bvh.Root.bb.Center [i], delta);
                Assert.AreEqual (2f, bvh.Root.bb.Extents [i], delta);
            }

            Assert.AreEqual (0, bvh.Root.ch [0].offset);
            Assert.AreEqual (2, bvh.Root.ch [0].length);

            Assert.AreEqual (2, bvh.Root.ch [1].offset);
            Assert.AreEqual (2, bvh.Root.ch [1].length);

            for (var j = 0; j < n; j++) {
                var s = (j & 2) != 0 ? 1 : 0;
                var t = (j & 1) != 0 ? 1 : 0;

                Assert.AreEqual (1, bvh.Root.ch [s].ch [t].Values.Count);
                Assert.AreEqual (j, bvh.Root.ch [s].ch [t].Values.First.Value);
                    
                for (var i = 0; i < 3; i++) {
                    Assert.AreEqual (j, bvh.Root.ch [s].ch [t].bb.Center [i], delta);
                    Assert.AreEqual (1f, bvh.Root.ch [s].ch [t].bb.Size [i], delta);
                }
            }
    	}

        #region Static
        public static StringBuilder List(StringBuilder buf, Node<int> t, int depth) {
            if (t == null)
                return buf;

            for (var i = 0; i < depth; i++)
                buf.Append ("        ");
            buf.AppendFormat ("({0},{1})={2}", t.offset, t.length, t.bb);
            if (t.IsLeaf ())
                buf.AppendFormat (" Values={0}", t.Values.First.Value);
            buf.AppendLine ();

            for (var i = 0; i < 2; i++)
                List (buf, t.ch [i], depth+1);

            return buf;
        }
        #endregion

        public class Volume : IVolume<int> {
            public int id;
			public FastBounds Bounds { get; protected set; }
			public int Value { get { return id; } }

			public Volume(FastBounds b, int id) {
				Bounds = b;
				this.id = id;
			}
		}
    }
}