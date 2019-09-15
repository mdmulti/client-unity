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

    public void BroadcastRXStart()
    {
        Broadcast.StartListening();
    }

    public void BroadcastRXStop()
    {
        Broadcast.StopListening();
    }

    public async void Latest()
    {
        Debug.Log("A");
        //Debug.Log(await CertHelper.VerifyUserCert(await CertHelper.GetCertificateFromFile(CertHelper.GetFirstUserCertFileName())));
        //await CertHelper.GetCertificateFromFile("meme");
        System.Security.Cryptography.X509Certificates.X509Certificate2 user = await CertHelper.GetCertificateFromFile("y3.crt");
        //Debug.Log(CertHelper.GetIssuer(await CertHelper.GetCertificateFromFile(CertHelper.GetFirstUserCertFileName())).GetSerialNumberString());

        //System.Security.Cryptography.X509Certificates.X509Certificate2 root = CertHelper.GetIssuer(user);

        //Debug.Log(root.FriendlyName);
        //Debug.Log(user.Issuer);

        UserFile u = new UserFile(user);
        Debug.Log(u.DisplayName);
        Debug.Log(u.ID);
        Debug.Log(u.ServerID);

        u.Save();
    }

    public void GetServerCert()
    {
        RestHelper.DownloadServerCertificate();
    }
}