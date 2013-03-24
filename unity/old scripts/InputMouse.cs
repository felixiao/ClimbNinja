using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
public class InputMouse : MonoBehaviour {
	private Transform selectedObj;
	private List<int> clickList=new List<int>();
	private List<int> pressed=new List<int>();
	private bool dragging= false;
	private bool touchBegin=false;
	private List<GameObject> cube=new List<GameObject>();
	private string ip="";
	private string id="";
	private NetworkClient netClient=new NetworkClient();
	// Use this for initialization
	void Start () {
		cube=((ColorPanel)GetComponent("ColorPanel")).cube;
	}
	void OnGUI(){
		if(Application.loadedLevelName=="MultiOneScreen"){
			if (GUI.Button(new Rect(380, 10, 100, 100), "MainMenu"))
	        	Application.LoadLevel("MainMenu");
			//need valid data
			if(!netClient.register){
				GUI.Label(new Rect(10, 10, 50, 30),"ID:");
				id=GUI.TextField(new Rect(80, 10, 100, 50),id);
				GUI.Label(new Rect(10, 80, 50, 30),"Host IP:");
				ip=GUI.TextField(new Rect(80, 70, 100, 50),ip);
				if(GUI.Button(new Rect(200, 10, 100, 100), "Connect"))
					netClient.Register(id,ip);
			}
			if(netClient.register)
				if(GUI.Button(new Rect(200, 10, 100, 100), "DisConnect"))
					netClient.Disconnect();
			
//			IPHostEntry IPHost = Dns.Resolve( Dns.GetHostName() );
//          IPAddress _address = IPHost.AddressList[0];
//          GUI.Label( new Rect(0,0,300,20),"Local IP: " + _address );
		}
	}
	// Update is called once per frame
	void Update () {
		if(Application.platform==RuntimePlatform.Android){
			Touch t=new Touch();
			
			if(Input.touchCount>0){
				Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
				RaycastHit rayHit=new RaycastHit();
				
				if(Physics.Raycast(ray,out rayHit)){
					selectedObj=rayHit.transform;
					for(int i=0;i<cube.Count;i++){
						if(selectedObj==cube[i].transform&&!pressed.Contains(i)){
							pressed.Add(i);
						}
					}
				}
				if(Input.GetTouch(0).phase==TouchPhase.Ended){
					HandleInputResult();
					pressed.Clear();
				}
			}
		}
		else if(Application.platform==RuntimePlatform.WindowsEditor){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit=new RaycastHit();
			if(Physics.Raycast(ray,out rayHit)){
				selectedObj=rayHit.transform;
				if(Input.GetMouseButtonDown(0)){
					//鼠标左键按下事件
					for(int i=0;i<cube.Count;i++){
						if(selectedObj==cube[i].transform){
							pressed.Add(i);
						}
					}
					dragging=true;
				}
				if(Input.GetMouseButton(0)){
					if(dragging){
						for(int i=0;i<cube.Count;i++){
							if(selectedObj==cube[i].transform&&!pressed.Contains(i)){
								pressed.Add(i);
							}
						}
					}
				}
				if(Input.GetMouseButtonUp(0)){
					//bool endAtCube=false;
					if(dragging){
						for(int i=0;i<cube.Count;i++){
							if(selectedObj==cube[i].transform&&!pressed.Contains(i)){
								pressed.Add(i);
								//endAtCube=true;
							}
						}
					}
					dragging=false;
					HandleInputResult();
					pressed.Clear();
				}
			}
		}
	}
	
	void HandleInputResult(){
		if(pressed.Count>1){
			//((ColorPanel)GetComponent("ColorPanel")).MeltColor(pressed);
			//((ColorPanel)GetComponent("ColorPanel")).CheckMatch();
			clickList.Clear();
		}
		else if(pressed.Count==1){
			clickList.Add(pressed[0]);
			if(clickList.Count==2){
				((ColorPanel)GetComponent("ColorPanel")).ExchangeColor(clickList[0],clickList[1]);
				if(Application.loadedLevelName=="SingleNormal"){
					((ColorPanel)GetComponent("ColorPanel")).CheckMatch();
				}
				else if(Application.loadedLevelName=="MultiOneScreen"){
					if(netClient.register){
						List<int> colorOrder=((ColorPanel)GetComponent("ColorPanel")).colorOrder;	
						netClient.SendMsg("CLI|"+colorOrder[0]+","+colorOrder[1]+","+colorOrder[2]+","+colorOrder[3]);
					}
				}
				clickList.Clear();
			}
		}
	}
}
