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

        public void getTest()
        {
            StartCoroutine(Rest.Get("info", res =>
            {
                UnityEngine.Debug.Log(res.ResponseData());
            }));
        }
    }
}