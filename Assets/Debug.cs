using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MDMulti.Mono
{
    public class Debug : MonoBehaviour
    {
        void Start()
        {
            
        }

        public void CreateUser()
        {
            User.Create("unity_test");
        }

        public void GetTest()
        {
            StartCoroutine(Rest.Get("info", res =>
            {
                UnityEngine.Debug.Log(res.ResponseData());
            }));
        }

        public void MulticastBroadcastStart()
        {
            Multicast.StartBroadcasting(Multicast.SetupForBroadcast());
        }

        public void MulticastBroadcastStop()
        {
            Multicast.StopBroadcasting();
        }
    }
}