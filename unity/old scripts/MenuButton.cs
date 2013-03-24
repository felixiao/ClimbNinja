using UnityEngine;
using System.Collections;
public class MenuButton : MonoBehaviour {
	string ip="";
	void OnGUI(){
			if (GUI.Button(new Rect(10, 100, 100, 50), "Single Player"))
	        	Application.LoadLevel("SingleNormal");
			if(GUI.Button(new Rect(10, 300, 100, 50), "Multi-Player"))
				Application.LoadLevel("MultiOneScreen");
			if(GUI.Button(new Rect(10, 500, 100, 50), "Info"))
				Debug.Log("Clicked the button with text");
			if(GUI.Button(new Rect(10, 700, 100, 50), "Quit"))
				Application.Quit();
	}
}
