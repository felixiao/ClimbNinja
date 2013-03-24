using UnityEngine;
using System.Collections;

public class Figures : MonoBehaviour {
	
	public Material figMat;
	Material[] mats=new Material[10];
	public GameObject figPre;
	public Vector3 pos;
	public int number;
	GameObject[] figures;
	// Use this for initialization
	void Start () {
		mats[0]=figMat;
		mats[0].SetTextureOffset("_MainTex",new Vector2(0,0));
		mats[1]=figMat;
		mats[1].mainTextureOffset=new Vector2(0.1f,0);
		mats[2]=figMat;
		mats[2].mainTextureOffset=new Vector2(0.2f,0);
		mats[3]=figMat;
		mats[3].mainTextureOffset=new Vector2(0.3f,0);
		mats[4]=figMat;
		mats[4].SetTextureOffset("_MainTex",new Vector2(0.4f,0));
		mats[5]=figMat;
		mats[5].SetTextureOffset("_MainTex",new Vector2(0.5f,0));
		mats[6]=figMat;
		mats[6].SetTextureOffset("_MainTex",new Vector2(0.6f,0));
		mats[7]=figMat;
		mats[7].SetTextureOffset("_MainTex",new Vector2(0.7f,0));
		mats[8]=figMat;
		mats[8].SetTextureOffset("_MainTex",new Vector2(0.8f,0));
		mats[9]=figMat;
		mats[9].mainTextureOffset=new Vector2(0.9f,0);
		
		string num=number.ToString();
		figures=new GameObject[num.Length];
		Quaternion rotation=Quaternion.identity;
		rotation.eulerAngles=new Vector3(270,0,0);
		for(int i=num.Length-1;i>=0;i--){
			int ind=int.Parse(num.Substring(i,1));
			figures[i]=(GameObject)Instantiate(figPre,new Vector3(pos.x+ind*3f,pos.y,pos.z),rotation);
			figures[i].renderer.material=mats[ind];
			Debug.Log(ind);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
