using UnityEngine;
using System.Collections;
using System.Threading;
public class GameState : MonoBehaviour {
	public bool isGameOver=false;
	Thread newSeverThread;
	
	
	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName=="MultiOneScreen"){
			newSeverThread=new Thread(new ThreadStart(Server.StartListening));
			newSeverThread.Start();
			ServerClient.color+=HandleColorCube;
		}
	}
	void HandleColorCube(ServerClient.DoColorCubeArgs e ){
		((ColorPanel)Camera.mainCamera.GetComponent("ColorPanel")).resetColorAs(e.a,e.b,e.c,e.d);
		((ColorPanel)Camera.mainCamera.GetComponent("ColorPanel")).CheckMatch();
	}
	// Update is called once per frame
	void Update () {
		CheckGameOver();
	}
	void CheckGameOver(){
		//lower than base line
		if(((Player)GetComponent("Player")).player.transform.position.y<=-0.25f){
			GameOver();	
		}
	}
	void GameOver(){
		isGameOver=true;
		((CommonVariables)GetComponent("CommonVariables")).speed=0;
		print("Gameover");
		if(Application.loadedLevelName=="MultiOneScreen")
		{
			
			newSeverThread.Abort();
		}
		Application.LoadLevel("MainMenu");
	}
}
