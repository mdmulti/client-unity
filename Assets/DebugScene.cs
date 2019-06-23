using UnityEngine;
using MDMulti;
using MDMulti.LAN.Discovery.Providers;

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

    public void MulticastStart()
    {
        Multicast.Start(Multicast.Setup());
    }

    public void MulticastStop()
    {
        Multicast.Stop();
    }

    private Broadcast.Opts broadcastOpts;

    public void BroadcastStart()
    {
        broadcastOpts = Broadcast.Setup();
        Broadcast.Start(broadcastOpts);
    }

    public void BroadcastStop()
    {
        Broadcast.Stop(broadcastOpts);
    }
}