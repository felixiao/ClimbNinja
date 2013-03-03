using System.Collections;
using System.Text;
using System;
using System.Net;
using UnityEngine;
public class ClientHelper {

	public string ID { get; set; }
        public string IP { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHelper"/> class.
        /// </summary>
        public ClientHelper()
        {
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public void Start()
        {
            try
            {
                Client.Register( IPAddress.Parse( IP ), 5000 );
				Client.StartClient();
				Send( "REG|" + ID );
            } catch(Exception e)
            {
				((GUIText)GameObject.Find("GuiText").GetComponent("GUIText")).text="Error "+e;
				Debug.Log("Error in Start:"+e);
            }
        }

        /// <summary>
        /// Send the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <remarks>Commands: 
        /// REG|"id"-> 
        /// QUIT->
        /// MSG|ALL|->    MSG|ID|"id"|->
        /// FILE|INFO|->  FILE|CONT|->	FILE|END|->
        /// SYN|REQ|"id"|"time"->	SYN|INI|"id"->
        /// </remarks>
        public void Send( string message )
        {
            try
            {
                Client.Send( message );
            } catch(Exception e)
            {
                Debug.Log( e.Message );
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        public void SendMsg( string message )
        {
            Send( "MSG|ALL|" + message );
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <param name='id'>
        /// Identifier.
        /// </param>
        public void SendMsg( string message, string id )
        {
            Send( string.Format( "MSG|ID|{0}|{1}", id, message ) );
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        public void Close()
        {
            Send( "QUIT" );
            Client.StopClient();
        }
}
