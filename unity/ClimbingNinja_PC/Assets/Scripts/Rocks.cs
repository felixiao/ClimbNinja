using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Rocks : MonoBehaviour {
	public List<Rock> rockGroup=new List<Rock>();
	
	// Use this for initialization
	void Start () {
		for(int i=0;i<5;i++){
			rockGroup.Add(generateRocks(i));
		}
	}
	// Update is called once per frame
	void Update () {
		foreach(Rock r in rockGroup){
			r.Update();
			if(r.pos.y<=-1f){
				r.Des();
				rockGroup.Remove(r);
				rockGroup.Add(generateRocks(5));
			}
		}
	}
	
	Rock generateRocks(int height){
		Rock r=new Rock();
		float y=-0.3f+((float)height)*0.7f+Random.value*0.1f-0.05f;
		r.Instant(new Vector3(Random.value*2f-1f,y,-6f));
		return r;
	}
}
