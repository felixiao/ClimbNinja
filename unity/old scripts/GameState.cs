using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public bool isGameOver=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckGameOver();
	}
	void CheckGameOver(){
		if(Application.loadedLevelName=="SingleNormal"){
			//lower than base line
			if(((Player)GetComponent("Player")).player.transform.position.y<=-0.25f){
				GameOver();	
			}
		}
	}
	void GameOver(){
		if(Application.loadedLevelName=="SingleNormal"){
			isGameOver=true;
			((CommonVariables)GetComponent("CommonVariables")).speed=0;
			print("Gameover");
			Application.LoadLevel("MainMenu");
		}
	}
}
