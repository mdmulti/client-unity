using UnityEngine;

namespace MDMulti
{
    public class MainMono : MonoBehaviour
    {
        public static MonoBehaviour Mono;

        public void Start()
        {
            Mono = this;
        }

        public static bool InScene()
        {
            if (Mono == null)
            {
                Debug.LogError("MDMulti Services Prefab not in Scene!");
                return false;
            } else
            {
                return true;
            }
        }
    }
}