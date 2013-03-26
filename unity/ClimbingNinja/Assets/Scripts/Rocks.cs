using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rocks{
	public List<Rock> rockGroup;
	float gap=1.2f;
	float gapoffset=0.3f;
	int rockCount=6;
	public Rocks(List<Texture> texs,GameObject colorCub){
		rockGroup=new List<Rock>();
		for(int i=rockCount-1;i>=0;i--){
			rockGroup.Add(generateRocks(i,texs,colorCub));
		}
	}
	public void Reset(float speed){
		for(int i=rockCount-1;i>=0;i--){
			float y=i*gap+Random.value*gapoffset;
			rockGroup[i].pos=new Vector3(Random.value*2.7f-1.35f,y,-1f);
			rockGroup[i].Init();
			rockGroup[i].Play();
			rockGroup[i].Update(speed);
		}
	}
	// Update is called once per frame
	public void Update (float speed) {
		foreach(Rock r in rockGroup){
			r.Update(speed);
		}
		
	}
	public void Paused(){
		foreach(Rock r in rockGroup){
			r.Paused();
		}
	}
	public void Play(){
		foreach(Rock r in rockGroup){
			r.Play();
		}
	}
	//fix bug here: sometime did not match
	public int CheckMatch(int[] order,int cur){
		int matched=-1;
		for(int i=rockCount-1;i>=0;i--){
			bool ma=true;
			if(!rockGroup[i].CheckMatch(order)){
				ma=false;
			}
			float iy=rockGroup[i].Position.y,cury=rockGroup[cur].Position.y;
			if(ma&&i!=cur&&iy>cury){
				Debug.Log("Ma="+matched);
				if(matched==-1)
					matched=i;
				else if(iy>rockGroup[matched].Position.y)
					matched=i;
				Debug.Log("cur="+cur+", i="+i+", ma="+matched);
			}
		}
		return matched;
	}
	Rock generateRocks(int height,List<Texture> texs,GameObject colorCube){
		Rock r=new Rock(texs,colorCube);
		float y=height*gap+Random.value*gapoffset;
		r.Instant(new Vector3(Random.value*2.7f-1.35f,y,-1f));
		return r;
	}
}
// a group of 4 rocks
public class Rock {
	float gapoffset=0.3f;
	GameObject _GOcolor;
	List<Texture> texs;//R,G,B,Y
	public Vector3 Position{get{return pos;}}
	public Vector3 pos=new Vector3();
	Vector3[] rotat=new Vector3[4];
	int[][] rotatRGBY=new int[4][];
	public int[] colorOrder=new int[4];
	private List<GameObject> rocks=new List<GameObject>();
	
	float offset=0.25f;
	//constructor
	public Rock(List<Texture> texs,GameObject color){
		this.texs=texs;
		_GOcolor=color;
		rotat[0]=new Vector3(270,0,0);
		rotat[1]=new Vector3(0,90,270);
		rotat[2]=new Vector3(0,270,90);
		rotat[3]=new Vector3(90,180,0);
		
		rotatRGBY[0]=new int[4]{3,2,1,0};
		rotatRGBY[1]=new int[4]{2,0,3,1};
		rotatRGBY[2]=new int[4]{1,3,0,2};
		rotatRGBY[3]=new int[4]{0,1,2,3};
	}
	public void Instant(Vector3 position){
		this.pos=position;
		
		//instantiate rocks to stage
		Quaternion rotation=Quaternion.identity;
		
		rocks.Add((GameObject)MonoBehaviour.Instantiate(_GOcolor,position+new Vector3(-offset,offset,0f),rotation));
		rocks.Add((GameObject)MonoBehaviour.Instantiate(_GOcolor,position+new Vector3(offset,offset,0f),rotation));
		rocks.Add((GameObject)MonoBehaviour.Instantiate(_GOcolor,position+new Vector3(-offset,-offset,0f),rotation));
		rocks.Add((GameObject)MonoBehaviour.Instantiate(_GOcolor,position+new Vector3(offset,-offset,0f),rotation));
		Init ();
	}
	public void Init(){
		//generate random color order
		colorOrder=new int[4];
		List<int> colorOri=new List<int>();
		colorOri.AddRange(new int[4]{0,1,2,3});
		colorOrder[0]=colorOri[Random.Range(0,colorOri.Count)];
		colorOri.Remove(colorOrder[0]);
		colorOrder[1]=colorOri[Random.Range(0,colorOri.Count)];
		colorOri.Remove(colorOrder[1]);
		colorOrder[2]=colorOri[Random.Range(0,colorOri.Count)];
		colorOri.Remove(colorOrder[2]);
		colorOrder[3]=colorOri[Random.Range(0,colorOri.Count)];
		
		for(int i=0;i<4;i++){
			rocks[i].renderer.material.SetTexture("_MainTex",texs[colorOrder[i]]);
			rocks[i].transform.eulerAngles=rotat[rotatRGBY[colorOrder[i]][i]];
		}
	}
	public void Paused(){
		rocks[0].renderer.enabled=false;
		rocks[1].renderer.enabled=false;
		rocks[2].renderer.enabled=false;
		rocks[3].renderer.enabled=false;
	}
	public void Play(){
		rocks[0].renderer.enabled=true;
		rocks[1].renderer.enabled=true;
		rocks[2].renderer.enabled=true;
		rocks[3].renderer.enabled=true;
	}
	public void Update(float speed){
		if(pos.y<8f){
			pos-=Vector3.up*speed;
			if(pos.y<=-1.5f){
				float y=5.5f+Random.value*gapoffset;
				pos=new Vector3(Random.value*2.7f-1.35f,y,-1f);
				Init();
			}
		}
		//Debug.log("POS: "+pos);
		rocks[0].transform.position=pos+new Vector3(-offset,offset,0);
		rocks[1].transform.position=pos+new Vector3(offset,offset,0);
		rocks[2].transform.position=pos+new Vector3(-offset,-offset,0);
		rocks[3].transform.position=pos+new Vector3(offset,-offset,0);
		
	}
	public bool CheckMatch(int[] order){
		for(int i=0;i<4;i++){
			if(order[i]!=colorOrder[i])
				return false;
		}
		return true;
	}
}
