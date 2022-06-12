using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class SwapClass
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            (b, a) = (a, b);
        }
    }
}
