using UnityEngine;
using System.Collections;

namespace Reconnitioning {
    
    public static class MortonCode {
        const int BIT_COUNT = 8;
        const int BIT_LIMIT = 1 << BIT_COUNT;
        const int BIT_MASK = BIT_LIMIT - 1;
        static readonly System.UInt32[] MORTON_X = X2Morton (new System.UInt32[BIT_LIMIT], 0);
        static readonly System.UInt32[] MORTON_Y = X2Morton (new System.UInt32[BIT_LIMIT], 1);
        static readonly System.UInt32[] MORTON_Z = X2Morton (new System.UInt32[BIT_LIMIT], 2);

        public static System.UInt64 Encode(int x, int y, int z) {
            return Encode ((System.UInt32)x, (System.UInt32)y, (System.UInt32)z);
        }
        public static System.UInt64 Encode(System.UInt32 x, System.UInt32 y, System.UInt32 z) {
            System.UInt64 m = 0;
            for (var i = 2; i >= 0; i--) {
                m = (m << (3 * BIT_COUNT))
                    | MORTON_X[(x >> (BIT_COUNT * i)) & BIT_MASK]
                    | MORTON_Y[(y >> (BIT_COUNT * i)) & BIT_MASK]
                    | MORTON_Z[(z >> (BIT_COUNT * i)) & BIT_MASK];
            }
            return m;
        }

        static System.UInt32[] X2Morton(System.UInt32[] mortons, int offset) {
            for (var i = 0; i < mortons.Length; i++)
                mortons[i] = X2Morton (i) << offset;
            return mortons;
        }

        static System.UInt32 X2Morton(int i) {
            var m = 0;
            for (var j = 0; j < BIT_COUNT; j++) {
                var b = (i >> j) & 1;
                m |= b << (3 * j);
            }
            return (System.UInt32)m;
        }
    }
}