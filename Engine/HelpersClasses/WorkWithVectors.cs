using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class WorkWithVectors
    {
        public static float[] ConvertToArrayVector3(Vector3 vector)
        {
            float[] result = new float[3];
            result[0] = vector.x;
            result[1] = vector.y;
            result[2] = vector.z;
            return result;
        }

        public static float[] ConvertToArrayVector2(Vector2 vector)
        {
            float[] result = new float[2];
            result[0] = vector.x;
            result[1] = vector.y;
            return result;
        }

        public static float[] ConvertToArrayVector(Vector2 vector)
        {
            float[] result = new float[2];
            result[0] = vector.x;
            result[1] = vector.y;
            return result;
        }

        public static float[] ConvertToArrayVector(Vector3 vector)
        {
            float[] result = new float[3];
            result[0] = vector.x;
            result[1] = vector.y;
            result[2] = vector.z;
            return result;
        }

        public static Vector3 ConvertArrayToVector3(float[] array)
        {
            Vector3 result = new Vector3();
            result = Vector3.zero;

            if (array == null || array.Length == 0)
            {

            }
            else if (array.Length == 1)
            {
                result.x = array[0];
            }
            else if (array.Length == 2)
            {
                result.x = array[0];
                result.y = array[1];
            }
            else
            {
                result.x = array[0];
                result.y = array[1];
                result.z = array[2];
            }

            return result;
        }

        public static Vector2 ConvertArrayToVector2(float[] array)
        {
            Vector2 result = new Vector2();
            result = Vector2.zero;

            if (array == null || array.Length == 0)
            {

            }
            else if (array.Length == 1)
            {
                result.x = array[0];
            }
            else
            {
                result.x = array[0];
                result.y = array[1];
            }

            return result;
        }
    }
}
