﻿using System.Text;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class Latest : MonoBehaviour
    {
        public void Latest1()
        {
            Debug.Log("L");
            //Debug.Log(MDMulti.SHA2Helper.ComputeHash(new byte[0]));
            //Debug.Log(MDMulti.SHA2Helper.ComputeHashStr(""));
            MDMulti.ConstantsHelper.Test();
        }

        public void Latest2()
        {
            Debug.Log("L2");
            byte[] s = MDMulti.Net.Core.GenerateAndAddHash(Encoding.UTF8.GetBytes("This is a SHA256 function."));
            Debug.Log(MDMulti.Net.Core.GetHash(s));
            Debug.Log("3");

            byte[] s3 = MDMulti.Net.Core.GenerateAndAddHash(Encoding.UTF8.GetBytes("This is a SHA256 function."));
            var ss = MDMulti.Net.Core.SplitHashAndMessage(s3);

            Debug.Log("MSG: " + Encoding.UTF8.GetString(ss.Item1));
            Debug.Log("HASH: " + ss.Item2);
        }
    }
}