using UnityEngine;
using System.Collections;

public class HandleUI : MonoBehaviour {
	public GameObject[] countdown;
	public GameObject fail;
	public GameObject scoreLabel;
	public GameObject restart;
	public GameObject con;
	public GameObject quit;
	public GameObject board;
	public GameObject score;
	
	public Rect textPos;
	private float time=0;
	
	int i;
	// Use this for initialization
	void Start () {
		i=0;
		None();
	}
	
	void OnGUI(){
		//GUI.Label(textPos,"Score: ");
	}
	public void None(){
		Play();
		((GUITexture)countdown[0].GetComponent("GUITexture")).enabled=false;
		((GUITexture)countdown[1].GetComponent("GUITexture")).enabled=false;
		((GUITexture)countdown[2].GetComponent("GUITexture")).enabled=false;
		((GUITexture)countdown[3].GetComponent("GUITexture")).enabled=false;
	}
	public void Play(){
		((GUITexture)fail.GetComponent("GUITexture")).enabled=false;
		((GUITexture)restart.GetComponent("GUITexture")).enabled=false;
		((GUITexture)con.GetComponent("GUITexture")).enabled=false;
		((GUITexture)quit.GetComponent("GUITexture")).enabled=false;
		((GUITexture)board.GetComponent("GUITexture")).enabled=false;
		((GUIText)score.GetComponent("GUIText")).enabled=false;
		((GUITexture)scoreLabel.GetComponent("GUITexture")).enabled=false;
	}
	public void Pause(){
		((GUITexture)con.GetComponent("GUITexture")).enabled=true;
		((GUITexture)quit.GetComponent("GUITexture")).enabled=true;
		((GUITexture)board.GetComponent("GUITexture")).enabled=true;
		((GUIText)score.GetComponent("GUIText")).enabled=true;
		((GUITexture)scoreLabel.GetComponent("GUITexture")).enabled=true;
	}
	public void Fail(){
		i=0;
		((GUITexture)fail.GetComponent("GUITexture")).enabled=true;
		((GUITexture)restart.GetComponent("GUITexture")).enabled=true;
		((GUITexture)quit.GetComponent("GUITexture")).enabled=true;
		((GUITexture)board.GetComponent("GUITexture")).enabled=true;
		((GUIText)score.GetComponent("GUIText")).enabled=true;
		((GUITexture)scoreLabel.GetComponent("GUITexture")).enabled=true;
	}
	public void Quit(){
		Play();
	}
	public bool CountDown(){
		if(i<4){
			if(i>0)
				((GUITexture)countdown[i-1].GetComponent("GUITexture")).enabled=false;
			((GUITexture)countdown[i].GetComponent("GUITexture")).enabled=true;
			time += Time.deltaTime;
			if(time>1f){
				time=0;
				i++;
			}
			return true;
		}
		else{
			((GUITexture)countdown[3].GetComponent("GUITexture")).enabled=false;	
			return false;
		}
	}
	
}
