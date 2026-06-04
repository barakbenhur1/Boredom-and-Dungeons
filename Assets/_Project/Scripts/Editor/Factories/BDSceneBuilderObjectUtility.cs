using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    internal static class BDSceneBuilderObjectUtility
    {
        public static void DestroyUnityObjectSafely(Object obj)
        {
            if (obj == null)
                return;

            if (Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
        }

        public static void RemoveColliderSafely(GameObject go)
        {
            if (go == null)
                return;

            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                DestroyUnityObjectSafely(collider);
        }
    }
}
