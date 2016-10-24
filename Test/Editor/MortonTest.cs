using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Text;

namespace Reconnitioning {

    public class MortonTest {

    	[Test]
    	public void EditorTest() {
            Assert.AreEqual (0, MortonCodeInt.Encode (0, 0, 0));

			Assert.AreEqual (1, MortonCodeInt.Encode (1, 0, 0));
			Assert.AreEqual (8, MortonCodeInt.Encode (2, 0, 0));

			Assert.AreEqual (2, MortonCodeInt.Encode (0, 1, 0));
			Assert.AreEqual (16, MortonCodeInt.Encode (0, 2, 0));

			Assert.AreEqual (4, MortonCodeInt.Encode (0, 0, 1));
			Assert.AreEqual (32, MortonCodeInt.Encode (0, 0, 2));

			Assert.AreEqual (7, MortonCodeInt.Encode (1, 1, 1));
			Assert.AreEqual ((1 << 24), MortonCodeInt.Encode (256, 0, 0));
    	}
    }
}
