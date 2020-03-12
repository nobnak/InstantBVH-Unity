using nobnak.Gist.Intersection;
using NUnit.Framework;
using Recon.Core;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BinnedSAHTest {

	[Test]
	public void BinnedSAHTestSimplePasses() {

        var bounds = new List<AABB3>();
        var indices = new List<int>();

        for (var i = 0; i < 4; i++) {
            var b = new Bounds(new Vector3(((i % 2) == 0 ? 1f : -1f) * (i + 1f), 0f, 0f), Vector3.one);
			var aabb = new AABB3(b);
            bounds.Add(aabb);
        }
        for (var i = 0; i < bounds.Count; i++)
            indices.Add(i);

        var pool = AABB3.CreateAABBPool();
        var sah = new BinnedSAH<int>(pool);

        int countFromLeft;
        sah.Build(bounds, indices, 0, bounds.Count, out countFromLeft);

        Assert.AreEqual(2, countFromLeft);
        AreEqual(new int[] { 3, 1, 0, 2 }, indices);
        Assert.AreEqual(indices.Count, bounds.Count);
    }

    private static string FormatArray<T>(IList<T> indices) {
        var log = new StringBuilder("<");
        for (var i = 0; i < indices.Count; i++)
            log.AppendFormat("{0}, ", indices[i]);
        log.Append(">");
        return log.ToString();
    }

    private static void AreEqual<T>(IList<T> a, IList<T> b) {
        Assert.AreEqual(a.Count, b.Count);
        for (var i = 0; i < a.Count; i++)
            Assert.AreEqual(a[i], b[i], string.Format("{0} and {1} at {2}", a[i], b[i], i));
    }
}
