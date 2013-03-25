using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class TrainingEntry : MonoBehaviour {
	public List<Rock> rockGroup=new List<Rock>();
	public List<Texture> texsRock;
	public List<Texture> texsPanel;
	
	public GameObject[] panelHighlight;
	public GameObject panelPre;
	public GameObject rockPre;
	public GameObject player;
	public MeshRenderer uiBackground;
	
	public GUITexture uiPause;
	public GUITexture uiContinue;
	public GUITexture uiQuit;
	public GUITexture uiBoard;
	
	public GUIText uiTime;
	public GUIText uiJumpCount;
	public GUIText uiColorChanged;
	public GUIText uiAJPM;
	public GUIText uiACPM;
	public GUIText uiACPJ;
	
	public Vector3 panelPos=new Vector3(0,-0.52f,-2);
	public float panelOffset=0.25f;

	Rock _rock;
	Player _player;
	ControlPanel _panel;
	int index;
	float passedTime;
	int jumpCount;
	int changeCount;
	bool pause;
	private bool dragging;
	private int prevTouched;
	private int[] curTouched;
	// Use this for initialization
	void Start () {
		Screen.autorotateToPortrait=true;
		Init();
		_rock=new Rock(texsRock,rockPre);
		_rock.Instant(new Vector3(0,2f,-1f));
		
		_player=new Player(player,player.transform.position);
		_panel=new ControlPanel(texsPanel,panelPre,panelHighlight,panelPos,panelOffset);

	}
	void Init(){
		jumpCount=0;
		changeCount=0;
		passedTime=0;
		pause=false;
		dragging= false;
		prevTouched=-1;
		curTouched=new int[2]{-1,-1};
		Play();
	}
	void Update () {
		// when play
		if(!pause){
			_player.Update(player.transform.position+new Vector3(0,0,+0.1f));
			passedTime+=Time.deltaTime;
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
					if(uiPause.HitTest(Input.touches[0].position)){
						Pause();
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
				if(uiPause.HitTest(Input.mousePosition)){
					Pause();
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
		changeCount++;
		if(_rock.CheckMatch(order)){
			_rock.Init();
			_player.PlayerJump(player.transform.position);
			jumpCount++;
		}
	}
	void Continue(){
		pause=false;
		Play();
	}
	
	void Pause(){
		pause=true;
		uiContinue.enabled=true;
		uiQuit.enabled=true;
		uiBoard.enabled=true;
		uiTime.enabled=true;
		uiJumpCount.enabled=true;
		uiColorChanged.enabled=true;
		uiAJPM.enabled=true;
		uiACPM.enabled=true;
		uiACPJ.enabled=true;
		uiTime.text="Time: "+Mathf.FloorToInt(passedTime).ToString();
		uiJumpCount.text="Jump Count: "+jumpCount.ToString();
		uiColorChanged.text="Color Changed: "+changeCount.ToString();
		uiAJPM.text="AJPM: "+(jumpCount/passedTime*60f).ToString();
		uiACPM.text="ACPM: "+(changeCount/passedTime*60f).ToString();
		uiACPJ.text="ACPJ: "+(changeCount/(jumpCount*1f)).ToString();
	}
	void Quit(){
		Save();
		Application.LoadLevel("MainMenu");
	}
	void Save(){
	}
	public void Play(){
		uiContinue.enabled=false;
		uiQuit.enabled=false;
		uiBoard.enabled=false;
		uiTime.enabled=false;
		uiJumpCount.enabled=false;
		uiColorChanged.enabled=false;
		uiAJPM.enabled=false;
		uiACPM.enabled=false;
		uiACPJ.enabled=false;
	}
}
