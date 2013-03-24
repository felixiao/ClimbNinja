using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class ControlPanel
{
	List<Texture> texs;
	GameObject[] highlight;
	//Vector3 pos=new Vector3(0,-0.52f,-2);
	//float offset=0.25f;
	int[] order;
	private bool[] pressed=new bool[4]{false,false,false,false};
	public GameObject[] colors=new GameObject[4];
	/// <summary>
	/// Initializes a new instance of the <see cref="ControlPanel"/> class.
	/// </summary>
	/// <param name='tex'>
	/// the texture of colors
	/// </param>
	/// <param name='colorPre'>
	/// Color prefab
	/// </param>
	/// <param name='highlight'>
	/// Highlight
	/// </param>
	public ControlPanel(List<Texture> tex, GameObject colorPre,GameObject[] highlight,Vector3 pos,float offset){
		this.texs=tex;
		this.highlight=highlight;
		
		Quaternion rotation=Quaternion.identity;
		rotation.eulerAngles=new Vector3(270,0,0);
		
		colors[0]=(GameObject)MonoBehaviour.Instantiate(colorPre,pos+new Vector3(-offset,offset,-1),rotation);
		colors[1]=(GameObject)MonoBehaviour.Instantiate(colorPre,pos+new Vector3(offset,offset,-1),rotation);
		colors[2]=(GameObject)MonoBehaviour.Instantiate(colorPre,pos+new Vector3(-offset,-offset,-1),rotation);
		colors[3]=(GameObject)MonoBehaviour.Instantiate(colorPre,pos+new Vector3(offset,-offset,-1),rotation);
		
		InitColor();
		SetColor();
	}
	
	public int[] ChangeColor(int a,int b){
		Debug.Log("Changed color: "+a+" - "+b);
		order[a]=order[a]+order[b];
		order[b]=order[a]-order[b];
		order[a]=order[a]-order[b];
		
		pressed=new bool[4]{false,false,false,false};
		return order;
	}
	public int[] ChangeColor(){
		int a=-1;
		for(int i=0;i<4;i++){
			if(pressed[i]){
				if(a==-1)
					a=i;
				else{
					ChangeColor(a,i);
					break;
				}
			}
		}
		return order;
	}
	public int[] MixColor(int a, int b){
		Debug.Log("Mixed color: "+a+" - "+b);
		pressed=new bool[4]{false,false,false,false};
		return order;
	}
	public int[] MixColor(){
		int a=-1;
		for(int i=0;i<4;i++){
			if(pressed[i]){
				if(a==-1)
					a=i;
				else{
					MixColor(a,i);
					break;
				}
			}
		}
		return order;
	}
	
	public void InitColor(){
		order=new int[4]{0,1,2,3};
	}
	public void SetColor(){
		//colors[0].transform.position=pos+new Vector3(-offset,offset,-1);
		//colors[1].transform.position=pos+new Vector3(offset,offset,-1);
		//colors[2].transform.position=pos+new Vector3(-offset,-offset,-1);
		//colors[3].transform.position=pos+new Vector3(offset,-offset,-1);
		for(int i=0;i<4;i++){
			colors[i].renderer.material.SetTexture("_MainTex",texs[order[i]]);
			highlight[i].guiTexture.enabled=pressed[i];
		}
	}
	public void ResetPress(){
		pressed=new bool[4]{false,false,false,false};
	}
	public void UnPressed(int index){
		pressed[index]=false;
	}
	public void Pressed(int index){
		pressed[index]=true;
		//handle other feedback
	}

}

