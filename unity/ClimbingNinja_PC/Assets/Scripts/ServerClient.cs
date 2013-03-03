using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
public class ServerClient{

	public delegate void ShowEventHandler( ShowEventArgs e );
    /// <summary>Show message event
    /// </summary>
    public static event ShowEventHandler show;
    /// <summary>Define a ShowEventArgs
    /// to set message</summary>
    /// <remarks>Message is set once client gets all data in one read stream in receive method</remarks>
    public class ShowEventArgs
    {
        public readonly string message;
        public ServerClient SC;
        public ShowEventArgs( string msg ,ServerClient sc)
        {
            message = msg;
            SC = sc;
        }
    }
	
	public delegate void DoColorCubeHandler( DoColorCubeArgs e);
	
	public static event DoColorCubeHandler color;
	
	public class DoColorCubeArgs{
		public readonly int a,b,c,d;
		public ServerClient SC;
		public DoColorCubeArgs(int a1,int b2,int c3,int d4, ServerClient sc){
			a=a1;
			b=b2;
			c=c3;
			d=d4;
			SC=sc;
		}
	}
	
    public TcpClient _tcpClient;
    private string _clientEndPoint;
    private byte[] _byteMessage;
    public volatile string message;
    public string ID = null;
    public string Client = null;
    public static ArrayList listId = new ArrayList();
    //Save clients into static hashtable
    public static Hashtable _clients = new Hashtable();

	/// <summary>
	/// Initializes a new instance of the <see cref="ServerClient"/> class.
	/// </summary>
	/// <param name='tcpClient'>
	/// Tcp client.
	/// </param>
    public ServerClient( TcpClient tcpClient )
    {
        _tcpClient = tcpClient;
        _clientEndPoint = _tcpClient.Client.RemoteEndPoint.ToString();
        Debug.Log( "\nNew Connection, Client EndPoint: " + _clientEndPoint );
        //add this client to clients hashtable when an instance created.
        _clients.Add(_clientEndPoint, this );
        _byteMessage = new byte[_tcpClient.ReceiveBufferSize];
        show += new ShowEventHandler( HandleMessage );
        //Accept message, using lock to avoid conflict
        lock(_tcpClient.GetStream())
        {
            //using asynchronous IO thread for each client to R / W data
            //it will release the main thread as soon as possible so that release server as well.
            _tcpClient.GetStream().BeginRead( _byteMessage, 0,
                _tcpClient.ReceiveBufferSize, new AsyncCallback( Receive ), null );
        }
    }
	/// <summary>
	/// Handles the message.
	/// </summary>
	/// <param name='e'>
	/// E.
	/// </param>
	/// /// <remarks>Commands: 
    /// ->REG|"id"    
    /// CON|REJ|->           CON|ACC|->     
    /// JOIN|"id"->
    /// ->QUIT             QUIT|"id"->       
    /// ->MSG|ALL|         ->MSG|ID|"id"|       MSG|"id"|->	   
    /// ->SYN|INI|"id"     ->SYN|REQ|"id"|"time"	    SYN|CAST->          SYN|RECE->
    /// ->SYN|NAME:"name"|POUND:"pound"->
    /// ->Update|"client"|"pa"|"value"->
    /// </remarks>
    private void HandleMessage( ServerClient.ShowEventArgs e )
    {
        string[] msg = e.message.Split( '|' );
        if(this != e.SC)
            return;
        
        switch(msg[0])
        {
            case "REG":
            {
                //check if the ID is unique on server 
                if(listId.Contains( msg[1] ))
                {
                    //if true then reject this name and remove 
                    SendTo( "CON|REJ|Name already exist.", e.SC._tcpClient );
					//((GUIText)GameObject.Find("GUI Text").GetComponent("GUIText")).text="Conn";
                } 
                else
                {
                    e.SC.Client = msg[1].Substring(msg[1].Length - 2);
                    string s="";
                    foreach(string id in listId)
                    {
                        if(id!=null)
                            s+= id + "," ;
                    }
                    // if false then broadcast this connection and add this ID to listId
                    SendTo( "CON|ACC|" + s, e.SC._tcpClient );
                    
                    e.SC.ID = msg[1];
                    listId.Add( e.SC.ID );
                    SendToAll( e.SC.ID,"JOIN|" + e.SC.ID );
                }
                break;
            }
			case "CLI":
			{
				string[] index=((string)msg[1]).Split(',');
				int a=int.Parse(index[0]);
				int b=int.Parse(index[1]);
				int c=int.Parse(index[2]);
				int d=int.Parse(index[3]);
				color(new DoColorCubeArgs(a,b,c,d,e.SC));
				
				break;
			}
            case "QUIT":
            {
                e.SC._tcpClient.Close();
                Server.clients.Remove(e.SC);
                SendToAll( e.SC.ID,"QUIT|" + e.SC.ID );
                _clients.Remove( e.SC._clientEndPoint );
                listId.Remove( e.SC.ID );
                e.SC.ID = null;
                break;
            }
        }
    }

    /// <summary>Receive messages
    /// from client
    /// </summary>
    /// <param name="iAsyncResult"></param>
    public void Receive( IAsyncResult iAsyncResult )
    {
        try
        {
            int length = 0;
            //Accept message, using lock to avoid conflict
            lock(_tcpClient.GetStream())
            {
                try
                {
                    length = _tcpClient.GetStream().EndRead( iAsyncResult );
                } catch(Exception E)
                {
                    Debug.Log("onGetstream:"+E.Message );
                    SendToAll(ID, "QUIT|" + ID);
                    Server.clients.Remove(this);
                    _clients.Remove(_clientEndPoint);
                    listId.Remove(ID);
                    //_tcpClient.GetStream().Close();
                    _tcpClient.Close();
                    return;
                }
            }
            if(length > 0)
            {
                string message = Encoding.UTF8.GetString( _byteMessage, 0, length );
                show( new ShowEventArgs( message,this) );
            } else if(length <= 0)
            {
                return;
            }
            if (_tcpClient != null && _tcpClient.Connected)
            {
                //listen for client again, using lock to avoid conflict
                lock (_tcpClient.GetStream())
                {
                    //call back Stream.BeginRead method，to listen next incoming stream
                    _tcpClient.GetStream().BeginRead(_byteMessage, 0, _tcpClient.ReceiveBufferSize,
                                                    new AsyncCallback(Receive), null);
                }
            }
            
        } catch(Exception ex)
        {
            Debug.Log( "OnReceive:"+ex.ToString()+". "+ex.Source );
            SendToAll(ID, "QUIT|" + ID);
            Server.clients.Remove(this);
            _clients.Remove( _clientEndPoint );
            listId.Remove( ID );
            _tcpClient.Close();
			Server.CloseServer();
        }
    }

    /// <summary>Transfer message
    /// to bytedata then send to client
    /// </summary>
    /// <param name="message"></param>
    /// <param name="client"></param>
    public void SendTo( string message,TcpClient client )
    {
        try
        {
            message = "{" + message + "}";
            Debug.Log("SendTo: "+message);
            NetworkStream ns;
            lock (client.GetStream())
            {
                ns = client.GetStream();
            }
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            ns.Write(byteMessage, 0, byteMessage.Length);
            ns.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("On sendmsg:"+e.Message);
            Server.clients.Remove(this);
            _clients.Remove(_clientEndPoint);
            listId.Remove(ID);
            //_tcpClient.GetStream().Close();
            _tcpClient.Close();
        }
    }

    /// <summary>Send message
    /// to all clients
    /// </summary>
    /// <param name="message"></param>
    public void SendToAll( string id,string message )
    {
        foreach (ServerClient sc in Server.clients)
        {
            //avoid sending message to itself
            if (sc.ID != id)
            {
                Debug.Log(sc._clientEndPoint + ":" + sc.ID + ": " + message);
                SendTo(message, sc._tcpClient);
            }
        }
    }
    
}