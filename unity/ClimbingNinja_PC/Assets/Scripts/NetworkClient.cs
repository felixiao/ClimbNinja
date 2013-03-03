using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Text;
public class NetworkClient : MonoBehaviour {
	ClientHelper tcpclient;
	bool register = false;
	// Use this for initialization
	void Start () {
		tcpclient = new ClientHelper();
		if(!register)
        {
            tcpclient.ID = "Felix";
            tcpclient.IP = "192.168.137.1";
            tcpclient.Start();
            Client.show += Receive;
            register = true;
        }
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
            case "MSG":
            {
				Debug.Log(message[1]+":\t@ "+ DateTime.UtcNow+"\n\t"+message[2] );
                break;
            }
            case "SYN":
            {
				Debug.Log("\nServer:\t@ " + DateTime.UtcNow+"\n\t" + e.message);
                break;
            }
        }
    }
}
