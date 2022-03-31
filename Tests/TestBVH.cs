using System.Collections.Generic;
using NUnit.Framework;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using SimpleBVH;
using System.Linq;
using UnityEngine;

public class TestBVH {

    [Test]
    public void TestBVHSimplePasses() {

        var objs = new List<BV>(new BV[]{
            new BV(){ Bounds = new MinMaxAABB(-1, -1) },
        });

        var bvh = objs.Build();
        Assert.AreEqual(0, bvh.Heap.Count);
        Assert.AreEqual(1, bvh.Indices.Count);
        Assert.AreEqual(0, bvh.Indices[0]);



        objs = new List<BV>(new BV[] {
            new BV(){ Bounds = new MinMaxAABB(2, 3) },
            new BV(){ Bounds = new MinMaxAABB(-3, -2) },
            new BV(){ Bounds = new MinMaxAABB(-4, -3) },
            new BV(){ Bounds = new MinMaxAABB(3, 4) },
        });
        var indices = new int[] { 2, 1, 0, 3 };
        bvh = objs.Build();
        //Debug.Log(bvh);

        Assert.AreEqual(3, bvh.N_Inners);
        Assert.IsTrue(bvh.Indices.SequenceEqual(indices));
        Assert.AreEqual(new MinMaxAABB(-4, 4), bvh.Heap[0]);
        Assert.AreEqual(new MinMaxAABB(-4, -2), bvh.Heap[1]);
        Assert.AreEqual(new MinMaxAABB(2, 4), bvh.Heap[2]);


        {
            var found = bvh.Find(new MinMaxAABB(-4, 4));
            Assert.AreEqual(4, found.Count());
            Assert.IsTrue(found.Select(v => v.AtObjects).SequenceEqual(indices));
        }

        for (var i = 0; i < objs.Count; i++) {
            var bv = objs[i];
            var found = bvh.Find(bv.Bounds.Center).ToList();
            Assert.AreEqual(1, found.Count());
            Assert.AreEqual(bv, found[0].Obj);
        }

        {
            Assert.AreEqual(2, bvh.Find(objs[0].Bounds.Encapsulate(objs[3].Bounds)).Count());
            Assert.AreEqual(2, bvh.Find(objs[1].Bounds.Encapsulate(objs[2].Bounds)).Count());
        }
    }

}
