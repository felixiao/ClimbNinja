using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HelpInfo : MonoBehaviour {
	ControlPanel _panel;
	public List<Texture> texsPanel;
	public GameObject panelPre;
	public GameObject[] panelHighlight;
	public GUITexture quit;
	// Use this for initialization
	void Start () {
		Debug.Log(Screen.GetResolution[0].height+","+Screen.GetResolution[0].width);
		//_panel=new ControlPanel(texsPanel,panelPre,panelHighlight,new Vector3(0,-0.52f,-2),0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		#region handle input
		if(Application.platform==RuntimePlatform.Android){
			if(Input.touchCount>0){
				if(Input.GetTouch(0).phase==TouchPhase.Ended){
					// press quit
					if(quit.HitTest(Input.touches[0].position)){
						Application.LoadLevel("MainMenu");
					}
				}
			}
		}
		else if(Application.platform==RuntimePlatform.WindowsEditor){
			if(Input.GetMouseButtonUp(0)){
				// press quit
				if(quit.HitTest(Input.mousePosition)){
					Application.LoadLevel("MainMenu");
				}
			}
		}
		# endregion
	}
}
