using System.Collections;
using UnityEngine;

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
    }
}