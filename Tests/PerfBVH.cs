using System.Collections.Generic;
using NUnit.Framework;
using SimpleBVH.Models;
using Unity.PerformanceTesting;
using SimpleBVH;
using SimpleBVH.Intenals;
using System.Linq;

public class PerfBVH {
    // A Test behaves as an ordinary method
    [Test]
    [Performance]
    public void TestPerformanceSimplePasses() {
        var n = 10000;
        var s = 1000f;
        var k = 2;

        var markers = BVHExtension.MARKERS.Select(v => new SampleGroup(v, SampleUnit.Microsecond)).ToArray();

        var rand = new Unity.Mathematics.Random(31);
        var objes = new List<BV>();
        for (var i = 0; i < n; i++) {
            var index0 = rand.NextFloat3() * s;
            var index1 = index0 + 1;
            var b = new MinMaxAABB(index0, index1);
            objes.Add(new BV() { Bounds = b });
        }

        Measure.Method(() => {
            var bvh = objes.Build(k);
        }).ProfilerMarkers(markers)
            .Run();
    }
}
