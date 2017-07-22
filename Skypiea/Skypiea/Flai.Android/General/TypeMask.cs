using System;

#if COMPONENT_MASK_USE_BIGINTEGER
using SystemBit = System.Numerics.BigInteger;
#else
using Bit = System.UInt64;
#endif

namespace Flai.General
{
    public struct TypeMask<T> : IEquatable<TypeMask<T>>
    {
        public static readonly Type BitType = typeof(Bit);

        private static Bit NextBit = 1;
        private readonly Bit _bits;

        public static readonly TypeMask<T> Empty = new TypeMask<T>();

        public bool IsEmpty
        {
            get { return _bits == TypeMask<T>.Empty._bits; }
        }

        private TypeMask(Bit bits)
        {
            _bits = bits;
        }

        public bool HasBits(TypeMask<T> other)
        {
            return (_bits & other._bits) == other._bits;
        }

        public bool HasBits(Bit other)
        {
            return (_bits & other) == other;
        }

        public bool Equals(TypeMask<T> other)
        {
            return _bits == other._bits;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeMask<T> && this.Equals((TypeMask<T>)obj);
        }

        public override int GetHashCode()
        {
            return _bits.GetHashCode();
        }

        #region Operators

        public static TypeMask<T> operator &(TypeMask<T> left, TypeMask<T> right)
        {
            return new TypeMask<T>(left._bits & right._bits);
        }

        public static TypeMask<T> operator |(TypeMask<T> left, TypeMask<T> right)
        {
            return new TypeMask<T>(left._bits | right._bits);
        }

        public static TypeMask<T> operator ~(TypeMask<T> mask)
        {
            return new TypeMask<T>(~mask._bits);
        }

        public static bool operator ==(TypeMask<T> left, TypeMask<T> right)
        {
            return left._bits == right._bits;
        }

        public static bool operator !=(TypeMask<T> left, TypeMask<T> right)
        {
            return left._bits != right._bits;
        }

        #endregion

        public static TypeMask<T> GetBit<Y>()
            where Y : T
        {
            return new TypeMask<T>(TypeMaskHelperUInt64<T>.GetBit<Y>());
        }
    }
}
