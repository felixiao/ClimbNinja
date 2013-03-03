//using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
public static class Client {
	public delegate void ShowEventHandler( ShowEventArgs e );
	
	public static event ShowEventHandler show;
	public class ShowEventArgs
    {
        public readonly string message;
        public ShowEventArgs( string msg )
        {
            message = msg;
        }
    }
	
	private static IPAddress _address;
	private static int _port;
    private static TcpClient _tcpClient;
    private static NetworkStream _networkStream;
    private static byte[] _byteMessage;
	/// <summary>
	/// Register the specified serverIP and port.
	/// </summary>
	/// <param name='serverIP'>
	/// Server IP.
	/// </param>
	/// <param name='port'>
	/// Port.
	/// </param>
	/// <exception cref='PingException'>
	/// Is thrown when the ping exception.
	/// </exception>
	public static void Register( IPAddress serverIP, int port )
    {
        _port = port;
        _address = serverIP;
        Ping p = new Ping();
        PingReply reply = p.Send( _address );
        if(reply.Status != IPStatus.Success)
        {
            throw new PingException( reply.Status.ToString() );
        }

        _tcpClient = new TcpClient( _address.ToString(), _port );
    }
	/// <summary>
	/// Starts the client.
	/// </summary>
	public static void StartClient()
    {
        // TODO: Exceptions need be handled
        _byteMessage = new byte[_tcpClient.ReceiveBufferSize];
        _networkStream = _tcpClient.GetStream();
        _networkStream.BeginRead( _byteMessage, 0, _tcpClient.ReceiveBufferSize, new AsyncCallback( Receive ), null );
    }
	/// <summary>
	/// Send the specified message.
	/// </summary>
	/// <param name='message'>
	/// Message.
	/// </param>
	public static void Send( string message )
    {
        byte[] sendMessage = Encoding.UTF8.GetBytes( message );
        Console.WriteLine( sendMessage.Length );
        _networkStream.Write( sendMessage, 0, sendMessage.Length );
        _networkStream.Flush();
    }
	/// <summary>
	/// Receive the specified iAsyncResult.
	/// </summary>
	/// <param name='iAsyncResult'>
	/// I async result.
	/// </param>/
	private static void Receive( IAsyncResult iAsyncResult )
    {
        try
        {
            int length = 0;
            length = _networkStream.EndRead( iAsyncResult );
            string msg = Encoding.UTF8.GetString( _byteMessage, 0, length );
            show( new ShowEventArgs( msg ) );
            _networkStream.BeginRead( _byteMessage, 0, _tcpClient.ReceiveBufferSize, new AsyncCallback( Receive ), null );
        } catch(Exception e)
        {
            Console.WriteLine( e.Message );
            StopClient();
        }
    }
	/// <summary>
	/// Stops the client.
	/// </summary>
	public static void StopClient()
    {
		try
		{
			_networkStream.Close();
			_tcpClient.Close();
		}
		catch(Exception e)
		{
			Console.WriteLine( e.Message );
		}

    }
}
