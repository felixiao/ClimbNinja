using UnityEngine;
using System.Collections;

public class Player{
	private int score;
	private GameObject _player;
	public float Head{get{return _player.transform.position.y;}}
	public int Score{get{return score;}}
	object[] ani_jump;
	object[] ani_stand;
	object[] ani_fall;
	private float fps = 24;
	private float time;
	private int nowFram;
	private bool ani_Start;
	private bool jump;
	private float timeLeft;
	public bool fall;
	public enum Anima{
		stand,jump,fall
	}
	Anima aniType=Anima.stand;
	public Player(GameObject player,Vector3 pos){
		_player=player;
		ani_jump = Resources.LoadAll("jump");
		ani_stand=Resources.LoadAll("stand");
		ani_fall=Resources.LoadAll("fall");
		Init(pos);
	}
	public void Init(Vector3 pos){
		_player.transform.position=new Vector3(pos.x,pos.y,pos.z-0.1f);
		score=0;
		nowFram=0;
		ani_Start=true;
		jump=false;
		timeLeft=1.0f;
		fall=false;
		aniType=Anima.stand;
		_player.renderer.material.mainTexture = (Texture)ani_stand[nowFram];
	}
	
	// Update is called once per frame
	public void Update (Vector3 pos) {
		//_player.transform.position=pos+new Vector3(0,0,-0.1f);
		DoAnima(aniType);
		if(!fall){
			DoMove(pos);
		}
	}
	
	
	public void Dead(){
		Debug.Log("player fall!");
		aniType=Anima.fall;
		ani_Start=true;
		fall=true;
		_player.transform.position.Set(_player.transform.position.x,3f,_player.transform.position.z);
	}
	public void PlayerJump(Vector3 pos){
		score+=100;
		Debug.Log("player jump!");
		aniType=Anima.jump;
		ani_Start=true;
		jump=true;
		timeLeft=1.0f;
		//_player.transform.position=pos+new Vector3(0,0,-0.1f);
	}
	public void DoMove(Vector3 pos){
		pos=pos+new Vector3(0,0,-0.1f);
		if(jump){
			float deltaMove = Time.deltaTime/timeLeft*Vector3.Distance(_player.transform.position,pos);
        	_player.transform.position = Vector3.MoveTowards(_player.transform.position,pos,deltaMove);
			timeLeft-=Time.deltaTime;
			if(timeLeft<=0||_player.transform.position==pos)
				jump=false;
		}
		else if(!fall)
			_player.transform.position=pos;
	}
	public void DoAnima(Player.Anima aniT){
		object[] texs;
		switch(aniT){
		case Anima.stand:
			texs=ani_stand;
			break;
		case Anima.jump:
			texs=ani_jump;
			break;
		case Anima.fall:
			texs=ani_fall;
			break;
		default:
			texs=ani_stand;
			break;
		}
		if(ani_Start){
			nowFram = 0;
			ani_Start=false;
		}
		//计算限制帧的时间
        time += Time.deltaTime;
        if(time >= 1.0 / fps){
             nowFram++;
             time = 0;
             if(nowFram >= texs.Length)
             {
                nowFram = 0;
				ani_Start=true;
				if(aniT==Anima.fall)
					aniType=aniT;
				else
					aniType=Anima.stand;
             }
		}
        _player.renderer.material.mainTexture = (Texture)texs[nowFram];
		
	}
}
