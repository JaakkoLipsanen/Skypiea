
using System;

namespace Flai.General
{
#if !WINDOWS_PHONE
    
    using System.Numerics;
    // Two helper classes to make type specific "masks". Use: ulong bit = TypeMaskUInt64<Component>.GetBit<GravityComponent>
    // however it is advisable to not use this directly in the code, but rather wrap it like this: Component<T>.SystemBit for example
    internal static class TypeMaskHelperBigInt<T>
    {
        private static BigInteger NextBit = 1;

        public static BigInteger GetBit<Y>()
            where Y : T
        {
            return TypeMaskHelperInner<Y>.Bit;
        }

        private static class TypeMaskHelperInner<Y>
            where Y : T
        {
            public static readonly BigInteger Bit;
            static TypeMaskHelperInner()
            {
                TypeMaskHelperBigInt<T>.TypeMaskHelperInner<Y>.Bit = TypeMaskHelperBigInt<T>.NextBit;
                TypeMaskHelperBigInt<T>.NextBit <<= 1; // move the bits one to the right
            }
        }
    }

    #endif

    internal static class TypeMaskHelperUInt64<T>
    {
        private static UInt64 NextBit = 1;

        public static UInt64 GetBit<Y>()
            where Y : T
        {
            return TypeMaskHelperInner<Y>.Bit;
        }

        private static class TypeMaskHelperInner<Y>
            where Y : T
        {
            public static readonly UInt64 Bit;
            static TypeMaskHelperInner()
            {
                // hmm.. will it actually be uint.MaxValue? I guess it will..?
                Ensure.True(TypeMaskHelperInner<Y>.Bit != uint.MaxValue, "No more Bits available, use TypeMaskHelperBigInt<T>!");

                TypeMaskHelperInner<Y>.Bit = TypeMaskHelperUInt64<T>.NextBit;
                TypeMaskHelperUInt64<T>.NextBit <<= 1; // move the bits one to the right
            }
        }
    }
}