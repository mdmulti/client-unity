using UnityEngine;
using MDMulti;

public class DebugScene : MonoBehaviour
{
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