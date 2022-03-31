using System.Collections.Generic;
using NUnit.Framework;
using SimpleBVH.Models;
using Unity.PerformanceTesting;
using SimpleBVH;

public class TestPerformance
{
    // A Test behaves as an ordinary method
    [Test]
    [Performance]
    public void TestPerformanceSimplePasses() {
        var n = 100;
        var s = 1000f;
        var k = 2;

        var rand = new Unity.Mathematics.Random(31);
        var objes = new List<BV>();
        for (var i = 0; i < n; i++) {
            var index0 = rand.NextFloat3() * s;
            var index1 = index0 + 1;
            var b = new MinMaxAABB(index0, index1);
            objes.Add(new BV() { Bounds = b });
        }

        Measure.Method(() => {
            var bvh = objes.Build();
        })
            .WarmupCount(3)
            .MeasurementCount(100)
            .Run();
    }
}
