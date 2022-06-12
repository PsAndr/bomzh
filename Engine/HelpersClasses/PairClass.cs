using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{

    [System.Serializable]
    public sealed class Pair<TFirst, TSecond>
    {
        public TFirst first { get; set; }
        public TSecond second { get; set; }

        public Pair(TFirst first, TSecond second)
        {
            (this.first, this.second) = (first, second);
        }

        public void Deconstruct(out TFirst first, out TSecond second)
        {
            first = this.first;
            second = this.second;
        }
    }
}
