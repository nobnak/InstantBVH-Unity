using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleBVH.Comparers;
using SimpleBVH.Models;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

public class PerfSort {
    // A Test behaves as an ordinary method
    [Test]
    [Performance]
    public void PerfSortSimplePasses() {
        var n = 10000;
        var s = 1000f;

        var rand = new Unity.Mathematics.Random(31);
        var objes = new List<BV>();
        for (var i = 0; i < n; i++) {
            var index0 = rand.NextFloat3() * s;
            var index1 = index0 + 1;
            var b = new MinMaxAABB(index0, index1);
            objes.Add(new BV() { Bounds = b });
        }

        var indices0 = Enumerable.Range(0, n).ToList();
        var comp0 = new FuncComparer<int>((i, j) => {
            var a = objes[i].Bounds.Center.x;
            var b = objes[j].Bounds.Center.x;
            return (int)math.sign(a - b);
        });

        var centers = objes.Select(v => v.Bounds.Center).ToList();
        var indices1 = Enumerable.Range(0, centers.Count).ToList();
        var comp1 = new FuncComparer<int>((i, j) => {
            var a = centers[i].x;
            var b = centers[j].x;
            return (int)math.sign(a - b);
        });

        Measure.Method(() => {
            indices0.Sort(comp0);
        }).SampleGroup("Sort with Indices").Run();

        Measure.Method(() => {
            indices1.Sort(comp1);
        }).SampleGroup("Sort centers").Run();

        Measure.Method(() => {
            indices0.Sort();
        }).SampleGroup("Sort int").Run();
    }
}
