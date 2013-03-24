using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {
	public GUITexture start;
	public GUITexture quit;
	public GUITexture help;
	public GameObject player;
	public GUIText score;
	public GUIText endlessMode;
	public GUIText normalMode;
	public GUIText back;
	Player _player;
	// Use this for initialization
	void Start () {
		start.enabled=true;
		quit.enabled=true;
		help.enabled=true;
		endlessMode.enabled=false;
		normalMode.enabled=false;
		back.enabled=false;
		
		_player=new Player(player,player.transform.position);
		if(!PlayerPrefs.HasKey("Score")){
			PlayerPrefs.SetInt("Score",0);
			PlayerPrefs.Save();
		}
		score.text=PlayerPrefs.GetInt("Score").ToString();
		
	}
	
	// Update is called once per frame
	void Update () {
		#region handle input
		if(Application.platform==RuntimePlatform.Android){
			if(Input.touchCount>0){
				if(Input.GetTouch(0).phase==TouchPhase.Ended){
					// press start
					if(start.enabled&&start.HitTest(Input.touches[0].position)){
						OnStart();
					}
					// press help
					if(help.enabled&&help.HitTest(Input.touches[0].position)){
						OnHelp();
					}
					// press quit
					if(quit.enabled&&quit.HitTest(Input.touches[0].position)){
						OnQuit();
					}
					// press back
					if(back.enabled&&back.HitTest(Input.touches[0].position)){
						OnBack();
					}
					// press endless
					if(endlessMode.enabled&&endlessMode.HitTest(Input.touches[0].position)){
						Application.LoadLevel("SingleEndless");
					}
					// press normal
					if(normalMode.enabled&&normalMode.HitTest(Input.touches[0].position)){
						Application.LoadLevel("SingleNormal");
					}
				}
			}
		}
		else if(Application.platform==RuntimePlatform.WindowsEditor){
			if(Input.GetMouseButtonUp(0)){
				// press start
				if(start.enabled&&start.HitTest(Input.mousePosition)){
					OnStart();
				}
				// press help
				if(help.enabled&&help.HitTest(Input.mousePosition)){
					OnHelp();
				}
				// press quit
				if(quit.enabled&&quit.HitTest(Input.mousePosition)){
					OnQuit();
				}
				// press back
				if(back.enabled&&back.HitTest(Input.mousePosition)){
					OnBack();
				}
				// press endless
				if(endlessMode.enabled&&endlessMode.HitTest(Input.mousePosition)){
					Application.LoadLevel("SingleEndless");
				}
				// press normal
				if(normalMode.enabled&&normalMode.HitTest(Input.mousePosition)){
					Application.LoadLevel("SingleNormal");
				}
			}
		}
		# endregion
		DoAni();
		_player.Update(new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z+0.1f));
		
	}
	void DoAni(){
		
	}
	void OnBack(){
		endlessMode.enabled=false;
		normalMode.enabled=false;
		back.enabled=false;
		start.enabled=true;
		quit.enabled=true;
		help.enabled=true;
	}
	void OnStart(){
		
		quit.enabled=false;
		help.enabled=false;
		endlessMode.enabled=true;
		normalMode.enabled=true;
		back.enabled=true;
	}
	void OnQuit(){
		OnSave();	
		Application.Quit();
	}
	
	void OnHelp(){
		Application.LoadLevel("Help");
	}
	void OnSave(){
	}
}
