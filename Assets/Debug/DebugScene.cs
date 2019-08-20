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
            Debug.Log(res.ResponseData());
        }));
    }

    public void MulticastStart()
    {
        Multicast.StartBroadcasting();
    }

    public void MulticastStop()
    {
        Multicast.StopBroadcasting();
    }

    public void MulticastRXStart()
    {
        Multicast.StartListening();
    }

    public void MulticastRXStop()
    {
        Multicast.StopListening();
    }

    public void BroadcastStart()
    {
        Broadcast.StartBroadcasting();
    }

    public void BroadcastStop()
    {
        Broadcast.StopBroadcasting();
    }

    public async void Latest()
    {
        Debug.Log("A");
        //Debug.Log(await CertHelper.VerifyUserCert(await CertHelper.GetCertificateFromFile(CertHelper.GetFirstUserCertFileName())));
        await CertHelper.GetCertificateFromFile("meme");
        
    }

    public void GetServerCert()
    {
        RestHelper.DownloadServerCertificate();
    }
}