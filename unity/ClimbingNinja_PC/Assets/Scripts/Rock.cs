using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rock {
	public Vector3 pos=new Vector3();
	public List<int> colorOrder=new List<int>();
	
	private float offset=0.15f;
	private GameObject colorCube;
	private List<GameObject> rocks=new List<GameObject>();
	private List<Material> m_colors=new List<Material>();
	
	//constructor
	public Rock(){
		colorCube=((CommonVariables)Camera.mainCamera.GetComponent("CommonVariables")).colorCube;
		m_colors=((CommonVariables)Camera.mainCamera.GetComponent("CommonVariables")).m_colors;
	}
	
	public void Instant(Vector3 position){
		this.pos=position;
		
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
		
		//instantiate rocks to stage
		Quaternion rotation=new Quaternion(0f,0f,0f,0f);
		
		colorCube.renderer.material=m_colors[colorOrder[0]];
		rocks.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(-offset,offset,0),rotation));
		colorCube.renderer.material=m_colors[colorOrder[1]];
		rocks.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(offset,offset,0),rotation));
		colorCube.renderer.material=m_colors[colorOrder[2]];
		rocks.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(-offset,-offset,0),rotation));
		colorCube.renderer.material=m_colors[colorOrder[3]];
		rocks.Add((GameObject)MonoBehaviour.Instantiate(colorCube,position+new Vector3(offset,-offset,0),rotation));
	}
	
	public void Update(){
		pos-=Vector3.up*((CommonVariables)Camera.mainCamera.GetComponent("CommonVariables")).speed;
		//MonoBehaviour.print("POS: "+pos);
		rocks[0].transform.position=pos+new Vector3(-offset,offset,0);
		rocks[1].transform.position=pos+new Vector3(offset,offset,0);
		rocks[2].transform.position=pos+new Vector3(-offset,-offset,0);
		rocks[3].transform.position=pos+new Vector3(offset,-offset,0);
	}
	//destroy rocks
	public void Des(){
		foreach(GameObject r in rocks){
			MonoBehaviour.Destroy(r);
		}
		rocks.Clear();
	}
}
