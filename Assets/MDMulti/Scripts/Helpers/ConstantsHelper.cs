using UnityEngine;
using MDMulti.Constants;

namespace MDMulti
{
    public class ConstantsHelper
    {
        public static ConstantsObject Get()
        {
            string data = Resources.Load<TextAsset>("constants").text;
            return ConstantsObject.FromJson(data);
        }

        public static void Test()
        {
            Debug.Log(Get().Lan.Multicast.Address + ":" + Get().Lan.Multicast.Port);
        }
    }
}