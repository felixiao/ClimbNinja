using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMouse : MonoBehaviour {
	private Transform selectedObj;
	private List<int> clickList=new List<int>();
	private List<int> pressed=new List<int>();
	private bool dragging= false;
	private List<GameObject> cube=new List<GameObject>();
	// Use this for initialization
	void Start () {
		cube=((ColorPanel)GetComponent("ColorPanel")).cube;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit=new RaycastHit();
		if(Application.loadedLevelName=="SingleNormal"){
			if(Physics.Raycast(ray,out rayHit)){
				selectedObj=rayHit.transform;
				if(Input.GetMouseButtonDown(0)){
					//鼠标左键按下事件
					for(int i=0;i<cube.Count;i++){
						if(selectedObj==cube[i].transform){
							pressed.Add(i);
						}
					}
					dragging=true;
				}
				if(Input.GetMouseButton(0)){
					if(dragging){
						for(int i=0;i<cube.Count;i++){
							if(selectedObj==cube[i].transform&&!pressed.Contains(i)){
								pressed.Add(i);
							}
						}
					}
				}
				if(Input.GetMouseButtonUp(0)){
					bool endAtCube=false;
					if(dragging){
						for(int i=0;i<cube.Count;i++){
							if(selectedObj==cube[i].transform&&!pressed.Contains(i)){
								pressed.Add(i);
								endAtCube=true;
							}
						}
					}
					dragging=false;
					HandleInputResult();
					pressed.Clear();
				}
			}
		}
	}
	
	void HandleInputResult(){
		if(pressed.Count>1){
			((ColorPanel)GetComponent("ColorPanel")).MeltColor(pressed);
			((ColorPanel)GetComponent("ColorPanel")).CheckMatch();
			clickList.Clear();
		}
		else if(pressed.Count==1){
			clickList.Add(pressed[0]);
			if(clickList.Count==2){
				((ColorPanel)GetComponent("ColorPanel")).ExchangeColor(clickList[0],clickList[1]);
				clickList.Clear();
				((ColorPanel)GetComponent("ColorPanel")).CheckMatch();
			}
		}
	}
}
