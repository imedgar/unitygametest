using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameStates {
		Intro,
		Mainmenu,
		LevelSelection,
		Ingame
	}

	private static GameManager _instance;

	public GameStates currentState;
	
	// Controller Android
	public RuntimePlatform platform;
	
	// Canvas Ref
	GameObject canvas;

	public static GameManager Instance { 
		get{
			// create logic to create the instance
			if ( _instance == null ){
				GameObject go = new GameObject("GameManager");
				go.AddComponent<GameManager>();
			}
			return _instance;
		} 
	}
	private int score = 0;

	void Awake() {
		currentState = GameStates.Mainmenu;
		platform = Application.platform;
		canvas = GameObject.FindGameObjectWithTag ("UIPanel");
		_instance = this;
	}

	public void Pause(bool paused) {
		if(paused) {
			// pause the game/physic
			//Time.time = 0.0f;
		} else {
			// resume
			//Time.time = 1.0f;
		}
	}

	public bool canStartGameLogic (){
		if (currentState == GameStates.Mainmenu) {
			return false;
		} else {
			return true;
		}
	}

	public void activeGameLogic (){
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					GameManager.Instance.currentState = GameManager.GameStates.Ingame;
					canvas.SetActive(false);
				}
			}
		} 
		if (Input.GetKey (KeyCode.Space)) {
			GameManager.Instance.currentState = GameManager.GameStates.Ingame;
			canvas.SetActive(false);
		}
	}
}