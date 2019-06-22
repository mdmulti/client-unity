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

        [System.Serializable]
        public class Multicast
        {
            [Help("Please do not edit this section unless you know what you are doing.", UnityEditor.MessageType.Warning)]
            public string ip = "224.5.125.85";
            public int port = 29571;
            public float broadcastDelay = 1.5f;
        }

        // Editor Options
        public string appName = "PleaseChangeMe";
        [Space]
        public Multicast multicast = new Multicast();
    }
}