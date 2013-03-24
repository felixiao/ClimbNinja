using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ColorPanel : MonoBehaviour {
	public List<int> colorOrder=new List<int>();
	public List<GameObject> cube=new List<GameObject>();
	public Vector3 position=new Vector3(0f,-0.35f,-7f);
	public float offset=0.15f;
	public float scale=0.1f;
	private GameObject colorCube;
	private List<Material> m_colors=new List<Material>();
	// Use this for initialization
	void Start () {
		
		colorCube=((CommonVariables)GetComponent("CommonVariables")).colorCube;
		m_colors=((CommonVariables)GetComponent("CommonVariables")).m_colors;
		
		Texture2D text=(Texture2D)Resources.LoadAssetAtPath("Assets/Textures/Red.png",typeof(Texture2D));
		Material mater=(Material)Resources.LoadAssetAtPath("Assets/Materials/Blue.mat",typeof(Material));
		mater.mainTexture=text;
		GameObject.Find("Wall").renderer.material=mater;
		Instantiate();
	}
	public void Instantiate(){
		if(Application.loadedLevelName=="MultiOneScreen")
			position=new Vector3(0f,1f,-9.2f);
		
		//instantiate rocks to stage
		Quaternion rotation=new Quaternion(0f,0f,0f,0f);
		
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(-offset,offset,0),rotation));
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(offset,offset,0),rotation));
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(-offset,-offset,0),rotation));
		cube.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(offset,-offset,0),rotation));
		cube[0].transform.localScale.Scale(new Vector3(scale,scale,0.01f));
		
		resetColor();
	}
	// Update is called once per frame
	void Update () {
		
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
		cube[i].renderer.material=m_colors[colorOrder[i]];
		cube[j].renderer.material=m_colors[colorOrder[j]];
	}
	private void ApplyColor(){
		for(int i=0;i<cube.Count;i++){
			cube[i].renderer.material=m_colors[colorOrder[i]];
		}
	}
	public void MeltColor(List<int> cubes){
	
	}
	
	public bool CheckMatch(){
		if(Application.loadedLevelName=="SingleNormal"){
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
		if(Application.loadedLevelName=="MultiOneScreen"){
			return true;
		}
		else 
			return false;
	}
}