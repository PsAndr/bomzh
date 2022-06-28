using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{

    [System.Serializable]
    public struct Pair<TFirst, TSecond>
    {
        public TFirst first;
        public TSecond second;

        public Pair(TFirst first, TSecond second)
        {
            (this.first, this.second) = (first, second);
        }

        public void Deconstruct(out TFirst first, out TSecond second)
        {
            first = this.first;
            second = this.second;
        }

        public static implicit operator Pair<TFirst, TSecond>((TFirst, TSecond) tuple)
        {
            return new Pair<TFirst, TSecond>(tuple.Item1, tuple.Item2);
        }

        public override string ToString()
        {
            return $"({first}, {second})";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = first.GetHashCode();
                int hash2 = second.GetHashCode();

                int toReturn = 101;

                toReturn = toReturn * 13 + hash;
                toReturn = toReturn * 13 + hash2;

                int toReturn2 = 89;

                toReturn2 = toReturn2 * 29 + hash2;
                toReturn2 = toReturn2 * 29 + hash;

                int Hash = (int)2166136261;
                Hash = (Hash * 16777619) ^ toReturn;
                Hash = (Hash * 16777619) ^ toReturn2;

                return Hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not Pair<TFirst, TSecond>)
            {
                return false;
            }

            Pair<TFirst, TSecond> other = (Pair<TFirst, TSecond>)obj;

            return other.first.Equals(this.first) && other.second.Equals(this.second);
        }

        public static bool operator !=(Pair<TFirst, TSecond> pr1, Pair<TFirst, TSecond> pr2)
        {
            return !pr1.Equals(pr2);
        }

        public static bool operator ==(Pair<TFirst, TSecond> pr1, Pair<TFirst, TSecond> pr2)
        {
            return pr1.Equals(pr2);
        }
    }
}
