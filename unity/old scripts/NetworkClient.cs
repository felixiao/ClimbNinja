using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Text;
public class NetworkClient{
	ClientHelper tcpclient= new ClientHelper();
	public bool register = false;

	public void Register(string id,string ip){
		if(!register)
        {
            tcpclient.ID = id;
            tcpclient.IP = ip;
            tcpclient.Start();
            Client.show += Receive;
        }
	}
	public void Disconnect(){
		tcpclient.Close();
		register=false;
	}
	public void SendMsg(string msg){
		tcpclient.Send( msg );
	}
	private void Receive( Client.ShowEventArgs e )
    {
        string msg = e.message.Substring(1, e.message.Length - 2);
        string[] message = msg.Split( '|' );
        switch(message[0])
        {
            case "JOIN":
            {
				Debug.Log(message[1] + " comes in! " );
                break;
            }
            case "CON":
            {
                if(message[1] == "REJ")
                {
					Debug.Log("Connection reject! " + message[2] + "Please Enter a new name!" );
                } 
                else if(message[1] == "ACC")
                {
					register = true;
					string[] str = message[2].Split( ',' );
					Debug.Log("Welcome! "+ str.Length+" users online." );
                }
                break;
            }
            case "QUIT":
            {
				Debug.Log(message[1] + " has quited." );
                break;
            }
        }
    }
}
