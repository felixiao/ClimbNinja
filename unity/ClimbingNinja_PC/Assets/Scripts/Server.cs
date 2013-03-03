using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
public static class Server {

	private static TcpListener _listener;
    private static int _port;
    private static IPAddress _address;
    public static List<ServerClient> clients = new List<ServerClient>();

    public static void StartListening()
    {
        IPHostEntry IPHost = Dns.Resolve( Dns.GetHostName() );
        //Dns.GetHostAddresses( Dns.GetHostName() );
        
        _address = IPHost.AddressList[0];
        _port = 5000;
        _listener = new TcpListener( _address, _port );
        UnityEngine.Debug.Log( "Host Address: " + _listener.LocalEndpoint );
        _listener.Start();
        try
        {
            while (clients.Count<2)
            {
                clients.Add(new ServerClient(_listener.AcceptTcpClient()));
                //ServerClient client=new ServerClient( _listener.AcceptTcpClient() );
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("On Listening:" + e.Message);
			CloseServer();
        }
	}
	public static void CloseServer(){
		foreach(ServerClient sc in clients){
			sc._tcpClient.Close();
		}
	}
}
