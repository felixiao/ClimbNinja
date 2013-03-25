using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
/* UI-
 * 	-resolution
 * Bug-
 * 	-color matching
 * 
 * 
 */
public class SystemEntry : MonoBehaviour {
	public List<Rock> rockGroup=new List<Rock>();
	public List<Texture> texsRock;
	public List<Texture> texsPanel;
	
	public GameObject[] panelHighlight;
	public GameObject panelPre;
	public GameObject rockPre;
	public GameObject player;
	public MeshRenderer uiBackground;
	
	public GUIText uiScore;
	public GUITexture uiPause;
	public GUITexture uiContinue;
	public GUITexture uiRestart;
	public GUITexture uiQuit;
	public GUIText uiScoreBoard;
	public GUIText uiTime;
	
	public HandleUI uiHandler;
	public Vector3 panelPos=new Vector3(0,-0.52f,-2);
	public float panelOffset=0.25f;
	public float speed;
	
	Player _player;
	Rocks _rocks;
	ControlPanel _panel;
	int index;
	bool pause;
	bool start;
	private float bgY;
	private bool dragging;
	private int prevTouched;
	private int[] curTouched;
	float passedTime;
	float estimatedTime;
	float estimatedScore;
	bool autoScoll;
	string timeLabel;
	// Use this for initialization
	void Start () {
		Screen.autorotateToPortrait=true;
		Init();
		_rocks=new Rocks(texsRock,rockPre);
		_player=new Player(player,_rocks.rockGroup[index].Position);
		_panel=new ControlPanel(texsPanel,panelPre,panelHighlight,panelPos,panelOffset);
		if(autoScoll)
			uiScore.text=_player.Score.ToString();
		else
			uiScore.text=(estimatedScore-_player.Score).ToString();
	}
	void Init(){
		passedTime=0;
		switch(Application.loadedLevelName){
		case "SingleNormal":
			autoScoll=false;
			index=1;
			estimatedTime=90f;
			estimatedScore=2000f;
			timeLabel="Time Left: "+estimatedTime;
			break;
		case "SingleEndless":
			autoScoll=true;
			index=3;
			estimatedTime=0;
			estimatedScore=0;
			timeLabel="Time: "+passedTime;
			break;
		default:
			autoScoll=false;
			index=1;
			estimatedTime=90f;
			estimatedScore=2000f;
			break;
		}
		
		speed=0.005f;
		pause=false;
		start=true;
		bgY=0f;
		dragging= false;
		prevTouched=-1;
		curTouched=new int[2]{-1,-1};
		uiHandler.None();
	}
	void Update () {
		if(start){
			start=uiHandler.CountDown();
		}
		// when play
		else if(!pause){
			if(!_player.fall)
				passedTime+=Time.deltaTime;
			
			if(autoScoll){//endless mode, will auto scoll down
				if(_player.fall)
					speed=-0.1f;
				else
					speed=0.005f*(1+_player.Score/2000f);
				bgY-=speed*0.1f;
				timeLabel="Time: "+Mathf.FloorToInt(passedTime).ToString();
				//check if player is dead
				if(_player.Head<0f){
					_player.Dead();
					uiHandler.Fail();
					uiScoreBoard.text=_player.Score.ToString();
				}
				_rocks.Update(speed);
				
				uiBackground.material.SetTextureOffset("_MainTex",new Vector2(0,bgY));
				
			}else{//normal mode, will not auto scoll down
				if(_player.fall)
					speed=-0.1f;
				else
					speed=0.01f;
				//check if need move player and rock down
				if(_player.Head>2f){
					bgY-=speed*0.1f;
					_rocks.Update(speed);
					uiBackground.material.SetTextureOffset("_MainTex",new Vector2(0,bgY));
				}
				
				//check if time is end
				if(estimatedTime-passedTime<=0){
					//check if score is higher than expected
					if(_player.Score<estimatedScore){//failed
						_player.Dead();
						uiHandler.Fail();
						uiScoreBoard.text=_player.Score.ToString();
					}
					else{//pass
					}
				}else
					timeLabel="Time Left: "+Mathf.FloorToInt(estimatedTime-passedTime).ToString();
				
			}
			
			_player.Update(_rocks.rockGroup[index].Position);
			
			#region Handle Input
			RaycastHit rayHit=new RaycastHit();
			if(Application.platform==RuntimePlatform.Android){
				if(Input.touchCount>0){
					Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
					if(Physics.Raycast(ray,out rayHit)){
						Debug.DrawRay(ray.origin, rayHit.point, Color.yellow);
						if(Input.GetTouch(0).phase==TouchPhase.Began){
							OnPressDown(rayHit.transform);
						}
						if(Input.GetTouch(0).phase==TouchPhase.Moved){
							OnPress(rayHit.transform);
						}
						if(Input.GetTouch(0).phase==TouchPhase.Ended){
							OnPressUp(rayHit.transform);
						}
					}
				}
			}
			else if(Application.platform==RuntimePlatform.WindowsEditor){

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(ray,out rayHit)){
					Debug.DrawRay(ray.origin, rayHit.point, Color.yellow);
					if(Input.GetMouseButtonDown(0)){
						OnPressDown(rayHit.transform);
					}
					if(Input.GetMouseButton(0)){
						OnPress(rayHit.transform);
					}
					if(Input.GetMouseButtonUp(0)){
						OnPressUp(rayHit.transform);
					}
				}
			}
			#endregion
		}
		#region handle input
		if(Application.platform==RuntimePlatform.Android){
			if(Input.touchCount>0){
				if(Input.GetTouch(0).phase==TouchPhase.Ended){
					// press pause
					if(!start&&uiPause.HitTest(Input.touches[0].position)){
						Pause();
					}
					// press restart
					if(!pause&&uiRestart.HitTest(Input.touches[0].position)){
						Reset();
					}
					// continue
					if(pause&&uiContinue.HitTest(Input.touches[0].position)){
						Continue();
					}
					// press quit
					if(uiQuit.HitTest(Input.touches[0].position)){
						Quit();
					}
				}
			}
		}
		else if(Application.platform==RuntimePlatform.WindowsEditor){
			if(Input.GetMouseButtonUp(0)){
				// press pause
				if(!start&&uiPause.HitTest(Input.mousePosition)){
					Pause();
				}
				// press restart
				if(!pause&&uiRestart.HitTest(Input.mousePosition)){
					Reset();
				}
				// press continue
				if(pause&&uiContinue.HitTest(Input.mousePosition)){
					Continue();
				}
				//press quit
				if(uiQuit.HitTest(Input.mousePosition)){
					Quit();
				}
			}
		}
		# endregion
		uiTime.text=timeLabel;
		_panel.SetColor();
		
	}
	void OnPressDown(Transform selectedObj){
		for(int i=0;i<4;i++){
			if(selectedObj==_panel.colors[i].transform){
				curTouched[0]=i;
				_panel.Pressed(i);
			}
		}
		dragging=true;
	}
	void OnPress(Transform selectedObj){
		if(dragging){
			for(int i=0;i<4;i++){
				if(selectedObj==_panel.colors[i].transform){
					if(curTouched[0]!=i){
						if(curTouched[1]!=-1)
							_panel.UnPressed(curTouched[1]);
						curTouched[1]=i;
						_panel.Pressed(i);
					}
				}
			}
		}
	}
	void OnPressUp(Transform selectedObj){
		//bool endAtCube=false;
		if(dragging){
			//drawline
			for(int i=0;i<4;i++){
				if(selectedObj==_panel.colors[i].transform){
					if(curTouched[0]!=i){
						curTouched[1]=i;
						_panel.Pressed(i);
					}
				}
			}
			
		}
		//Debug.Log("curTouched: 0:"+curTouched[0]+", 1:"+curTouched[1]);
		
		if(curTouched[1]!=-1){//only pressed one button
			if(prevTouched!=-1){
				if(prevTouched==curTouched[0])
					CheckMatch(_panel.MixColor());
				else
					CheckMatch(_panel.ChangeColor());
				curTouched=new int[2]{-1,-1};
			}
			else if(curTouched[1]!=-1){	
				CheckMatch(_panel.ChangeColor());
				curTouched=new int[2]{-1,-1};
			}
		}
		else if(curTouched[1]==-1){
			if(prevTouched!=-1){
				if(prevTouched==curTouched[0])
					_panel.ResetPress();
				else
					CheckMatch(_panel.ChangeColor());
				curTouched=new int[2]{-1,-1};
			}
		}
		dragging=false;
		prevTouched=curTouched[0];
		curTouched=new int[2]{-1,-1};
	}
	
	void CheckMatch(int[] order){
		int ma=_rocks.CheckMatch(order,index);
		Debug.Log("maRes="+ma);
		if(ma!=-1){
			index=ma;
			_player.PlayerJump(_rocks.rockGroup[index].Position);
			if(autoScoll)
				uiScore.text=_player.Score.ToString();
			else
				uiScore.text=(estimatedScore-_player.Score).ToString();
		}
		
	}
	void Continue(){
		pause=false;
		_rocks.Play();
		uiHandler.Play();
	}
	
	void Reset(){
		Init();
		_rocks.Reset(speed);
		_player.Init(_rocks.rockGroup[index].pos);
		uiScore.text=_player.Score.ToString();
		uiBackground.material.SetTextureOffset("_MainTex",new Vector2(0,0));
	}
	void Pause(){
		pause=true;
		_rocks.Paused();
		uiHandler.Pause();
		uiScoreBoard.text=_player.Score.ToString();
	}
	void Quit(){
		Save();
		Application.LoadLevel("MainMenu");
	}
	void Save(){
		int s=PlayerPrefs.GetInt("Score");
		if(_player.Score>s)
			PlayerPrefs.SetInt("Score",_player.Score);
		PlayerPrefs.SetInt("Time",Mathf.FloorToInt(passedTime));
		PlayerPrefs.Save();
	}
}
