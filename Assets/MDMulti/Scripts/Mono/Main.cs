using UnityEngine;

namespace MDMulti.Mono
{
    public class Main : MonoBehaviour
    {
        public static MonoBehaviour Inst;

        public void Start()
        {
            Inst = this;
        }

        public static bool InScene()
        {
            if (Inst == null)
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