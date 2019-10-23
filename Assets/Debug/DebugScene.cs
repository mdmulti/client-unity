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
        Debug.Log("L");
        

        // ---------- SECURE CLIENT TESTING ----------
        //MDMulti.Net.SecureClient sc = new MDMulti.Net.SecureClient(new UserFile("792F1D97A2740D45867B"), new System.Net.IPEndPoint(MDMulti.IPHelper.StringToAddressObject("127.0.0.1"), 59655));
        //Debug.LogWarning("Final: " + await sc.Send("TEST"));


        // ---------- PLAIN CLIENT PEER CONNECTION TESTING ----------
        sfd = new ServerFoundEvent.serverFoundDel(l_int);
        ServerFoundEvent.OnServerFound += sfd;
    }

    private async void l_int(ServerDetails sfe)
    {
        // ---------- PLAIN CLIENT PEER CONNECTION TESTING ----------
        Debug.LogError("L_INT");
        Debug.LogError(await new PeerConnectionClient(sfe).IsValidPeer());
        Debug.LogError("L_INT_REM_INPROG");
        ServerFoundEvent.OnServerFound -= sfd;
        Debug.LogError("L_INT_REM_DONE");
    }

    public async void Latest2()
    {
        Debug.Log("L2");


        // ---------- SECURE CLIENT TESTING ----------
        //MDMulti.Net.SecureServer ss = new MDMulti.Net.SecureServer(new UserFile("792F1D97A2740D45867B"), 59655);
        //ss.StartListening(l2onrecv);
    }
    
    /*
    ---------- SECURE CLIENT TESTING ----------
    public string l2onrecv(string s)
    {
        return "MEMES";
    }
    */

    public void GetServerCert()
    {
        RestHelper.DownloadServerCertificate();
    }
}