using UnityEngine;

namespace Engine
{
    public static class GetNearHierarchyObjectOfType<T> where T : Object
    {
        public static T Get(Transform obj)
        {
            if (obj == null)
            {
                throw new System.Exception
                (
                    $"Don`t find object of this type: {typeof(T)}"
                );
            }

            T result = obj.GetComponent<T>();

            if (result == null)
            {
                result = obj.GetComponentInChildren<T>();
            }

            if (result == null)
            {
                result = obj.GetComponentInParent<T>();
            }

            if (result == null)
            {
                result = Object.FindObjectOfType<T>();
            }

            if (result == null)
            {
                throw new System.Exception
                (
                    $"Don`t find object of this type: {typeof(T)}"
                );
            }

            return result;
        }

        public static T GetWithoutException(Transform obj)
        {
            try
            {
                return Get(obj);
            }
            catch (System.Exception ex)
            {
                DebugEngine.LogException(ex);
                return null;
            }
        }
    }
}
