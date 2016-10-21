using UnityEngine;
using System.Collections;

namespace Reconnitioning {
    
    public static class MortonCode {
        public const int STRIDE_BITS = 8;
        public const int STRIDE_LIMIT = 1 << STRIDE_BITS;
        public const int STRIDE_MASK = STRIDE_LIMIT - 1;

        public const int INT_MAX = (1 << 21) - 1;
        public const int INT_MIN = 0;

        static readonly System.UInt32[] MORTON_X = X2Morton (new System.UInt32[STRIDE_LIMIT], 0);
        static readonly System.UInt32[] MORTON_Y = X2Morton (new System.UInt32[STRIDE_LIMIT], 1);
        static readonly System.UInt32[] MORTON_Z = X2Morton (new System.UInt32[STRIDE_LIMIT], 2);

        public static System.UInt64 Encode(int x, int y, int z) {
            return Encode ((System.UInt32)x, (System.UInt32)y, (System.UInt32)z);
        }
        public static System.UInt64 Encode(System.UInt32 x, System.UInt32 y, System.UInt32 z) {
            System.UInt64 m = 0;
            for (var i = 2; i >= 0; i--) {
                m = (m << (3 * STRIDE_BITS))
                    | MORTON_X[(x >> (STRIDE_BITS * i)) & STRIDE_MASK]
                    | MORTON_Y[(y >> (STRIDE_BITS * i)) & STRIDE_MASK]
                    | MORTON_Z[(z >> (STRIDE_BITS * i)) & STRIDE_MASK];
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
            for (var j = 0; j < STRIDE_BITS; j++) {
                var b = (i >> j) & 1;
                m |= b << (3 * j);
            }
            return (System.UInt32)m;
        }
    }
}