using static nobnak.Gist.LengthOfCommonPrefix;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recon {

	public class LCPTest {

		[Test]
		public void TestLength() {
			Assert.AreEqual(0, Length(0u));
			Assert.AreEqual(1, Length(1u << 31));
			Assert.AreEqual(31, Length((~0u) << 1));
		}

		[Test]
		public void TestLCP() {

			Assert.AreEqual(32, LCP(0u, 0u));
			Assert.AreEqual(32, LCP(1u, 1u));
			Assert.AreEqual(0, LCP(0u, ~0u));
			Assert.AreEqual(1, LCP(1u << 31, ~0u));
		}
	}
}
