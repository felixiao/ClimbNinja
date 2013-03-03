using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ColorPanel : MonoBehaviour {
	public List<int> colorOrder=new List<int>();
	public List<GameObject> cube=new List<GameObject>();
	
	private float offset=0.15f;
	private GameObject colorCube;
	private List<Material> m_colors=new List<Material>();
	// Use this for initialization
	void Start () {
		colorCube=((CommonVariables)GetComponent("CommonVariables")).colorCube;
		m_colors=((CommonVariables)GetComponent("CommonVariables")).m_colors;
		Instantiate();
	}
	public void Instantiate(){
		Vector3 position=new Vector3(0f,-0.35f,-7f);
		
		//instantiate rocks to stage
		Quaternion rotation=new Quaternion(0f,0f,0f,0f);
		
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(-offset,offset,0),rotation));
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(offset,offset,0),rotation));
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(-offset,-offset,0),rotation));
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(offset,-offset,0),rotation));
		
		resetColor();
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void resetColorAs(int a,int b,int c,int d){
		colorOrder[0]=a;
		colorOrder[1]=b;
		colorOrder[2]=c;
		colorOrder[4]=d;
		ApplyColor();
	}
	public void resetColor(){
		//generate random color order
		List<int> colorOri=new List<int>();
		colorOri.AddRange(new int[4]{0,1,2,3});
		colorOrder.Add(colorOri[Random.Range(0,3)]);
		colorOri.Remove(colorOrder[0]);
		colorOrder.Add(colorOri[Random.Range(0,2)]);
		colorOri.Remove(colorOrder[1]);
		colorOrder.Add(colorOri[Random.Range(0,1)]);
		colorOri.Remove(colorOrder[2]);
		colorOrder.Add(colorOri[0]);
		ApplyColor();
	}
	public void ExchangeColor(int i,int j){
		int c=colorOrder[i];
		colorOrder[i]=colorOrder[j];
		colorOrder[j]=c;
		ApplyColor();
	}
	private void ApplyColor(){
		for(int i=0;i<cube.Count;i++){
			cube[i].renderer.material=m_colors[colorOrder[i]];
		}
	}
	public void MeltColor(List<int> cubes){
	
	}
	
	public bool CheckMatch(){
		//print("Jump?"+colorOrder[0]+","+colorOrder[1]+","+colorOrder[2]+","+colorOrder[3]);
		for(int i=0;i<((Rocks)GetComponent("Rocks")).rockGroup.Count;i++){
			if(((Rocks)GetComponent("Rocks")).rockGroup[i].pos.y<=((Player)GetComponent("Player")).player.transform.position.y){
				continue;
			}
			int j=0;
			bool same=true;
			foreach(int c in ((Rocks)GetComponent("Rocks")).rockGroup[i].colorOrder){
				if(c!=colorOrder[j++]){
					same=false;
					break;
				}
			}
			if(same){
				((Player)GetComponent("Player")).PlayerJump(i);
				return true;
			}
		}
		return false;
	}
}