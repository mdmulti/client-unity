using UnityEngine;

namespace MDMulti.Mono
{
    public class Options : MonoBehaviour
    {
        public static Options Instance;

        void Awake()
        {
            Instance = this;
        }

        // Editor Options
        public string appName = "PleaseChangeMe";
    }
}