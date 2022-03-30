using System.Collections.Generic;
using NUnit.Framework;
using SimpleBVH;

public class TestKAryHeap {

    [Test]
    public void TestKAryHeapSimplePasses() {

        var k = 3;
        var n = 10;
        var karyHeap = new List<int>(
            new int[] {
                    0,
                    1, 2, 3,
                    4, 5, 6, 7, 8, 9, 10, 11, 12,
                    13, 14
            });


        // Parent
        Assert.AreEqual(4, karyHeap[k.Parent(13)]);

        // Left-most child
        Assert.AreEqual(4, karyHeap[k.LeftMostChild(1)]);

        // Right-most child
        Assert.AreEqual(6, karyHeap[k.RightMostChild(1)]);



        // Minimum depth of leaf
        Assert.AreEqual(2, k.MinDepthOfLeaf(n));

        // Capacity at depth
        Assert.AreEqual(9, k.CapacityAtDepth(2));

        // Total Capacity till depth
        Assert.AreEqual(13, k.TotalCapacityTillDepth(2));

        // Count inners at the min depth
        Assert.AreEqual(1, k.CountInnersAtMinDepth(n, k.CapacityAtDepth(2)));

        // Count leaves at the min depth
        Assert.AreEqual(8, k.CountLeavesAtMinDepth(n));

        // Total inners
        Assert.AreEqual(5, k.TotalInnerNodes(n));
    }
}
