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
        System.Net.IPEndPoint ipe = new System.Net.IPEndPoint(IPHelper.ToAddressObject(IPHelper.ToBytes("127.0.0.1")), 27421);
        NetSend.Send(ipe, NetSend.DataTypes.UnreliableUDP, "TEST");
    }

    public void GetServerCert()
    {
        RestHelper.DownloadServerCertificate();
    }
}