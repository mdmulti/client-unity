using UnityEngine;
using MDMulti;
using MDMulti.LAN.Discovery.Providers;
using System.Threading;

public class DebugScene : MonoBehaviour
{
    public void CreateUser()
    {
        User.Create();
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
        Multicast.StartBroadcasting();
        //Multicast.Start(Multicast.Setup());
    }

    public void MulticastStop()
    {
        Multicast.StopBroadcasting();
        //Multicast.Stop();
    }

    public void BroadcastStart()
    {
        Broadcast.StartBroadcasting();
    }

    public void BroadcastStop()
    {
        Broadcast.StopBroadcasting();
    }

    public void Latest()
    {
        Debug.Log("A");
        //RestHelper.DownloadServerCertificate();
        //Debug.Log(await RestHelper.ConnectionTest());
        //Debug.Log(ConnectionTests.WANIP());
        
        //var cts = new CancellationTokenSource();
        //await Multicast.BeginAnnouncingAsync(cts.Token);
        //cts.Cancel();
    }

    public void GetServerCert()
    {
        RestHelper.DownloadServerCertificate();
    }
}