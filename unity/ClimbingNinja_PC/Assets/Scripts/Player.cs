using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject player=new GameObject();
	// Use this for initialization
	void Start () {
		player=GameObject.Find("player");
		player.transform.position=((Rocks)GetComponent("Rocks")).rockGroup[2].pos+new Vector3(0,0,-0.05f);
	}
	
	// Update is called once per frame
	void Update () {
		player.transform.position-=player.transform.forward*((CommonVariables)GetComponent("CommonVariables")).speed;
	}
	public void PlayerJump(int index){
		player.transform.position=((Rocks)GetComponent("Rocks")).rockGroup[index].pos+new Vector3(0,0,-0.05f);
	}
}
