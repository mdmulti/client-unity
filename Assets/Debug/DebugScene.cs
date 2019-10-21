using UnityEngine;
using MDMulti;
using MDMulti.LAN.Discovery.Providers;
using System.Threading;
using MDMulti.LAN.Discovery;

public class DebugScene : MonoBehaviour
{
    public async void CreateUser()
    {
        (await User.CreateNewProfile()).Save();
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

    public async void BroadcastStart()
    {
        Broadcast.StartBroadcasting(new UserFile(await CertHelper.GetCertificateFromFile("p4.crt")));
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

    private ServerFoundEvent.serverFoundDel sfd;

    public async void Latest()
    {
        //Debug.LogError("L");
        //sfd = new ServerFoundEvent.serverFoundDel(l_int);
        //ServerFoundEvent.OnServerFound += sfd;

        //UserFile us = new UserFile("792F1D97A2740D45867B");
        //Debug.Log(us.ID);
        //us.Save();

        MDMulti.Net.SecureClient sc = new MDMulti.Net.SecureClient(new UserFile("792F1D97A2740D45867B"), new System.Net.IPEndPoint(MDMulti.IPHelper.StringToAddressObject("127.0.0.1"), 59655));
        sc.Send("TEST");
    }

    private async void l_int(ServerDetails sfe)
    {
        Debug.LogError("L_INT");
        UnityEngine.Debug.LogError(new PeerConnectionClient(sfe).IsValidPeer());
        Debug.LogError("L_INT_REM_INPROG");
        ServerFoundEvent.OnServerFound -= sfd;
        Debug.LogError("L_INT_REM_DONE");
    }

    public async void Latest2()
    {
        Debug.Log("L2");

        MDMulti.Net.SecureServer ss = new MDMulti.Net.SecureServer(new UserFile("792F1D97A2740D45867B"), 59655);

        ss.StartListening(l2onrecv);

        //System.Net.IPEndPoint ipe = new System.Net.IPEndPoint(IPHelper.ToAddressObject(IPHelper.ToBytes("127.0.0.1")), 27423);
        //NetSend.Send(ipe, NetSend.DataTypes.UnreliableUDP, "TEST");
        //MDMulti.Net.GeneralRecieve gs = new MDMulti.Net.GeneralRecieve(ipe, MDMulti.Net.DataTypes.UnreliableUDP);
        //gs.StartListening();
    }

    public string l2onrecv(string s)
    {
        return "MEMES";
    }

    public void GetServerCert()
    {
        RestHelper.DownloadServerCertificate();
    }
}