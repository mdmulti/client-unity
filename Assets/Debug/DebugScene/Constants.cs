using MDMulti;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class Constants : MonoBehaviour
    {
        public void LogTest()
        {
            MDMulti.Constants.ConstantsObject c = ConstantsHelper.Get();
            Debug.Log("ConstantsLogTest: Multicast Information\n" + c.Lan.Multicast.Address + ":" + c.Lan.Multicast.Port);
        }
    }
}