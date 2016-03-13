using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameStates {
		Intro,
		Mainmenu,
		LevelSelection,
		Ingame,
		Roofs,
		Street
	}

	public enum PlayerStates {
		Idle,
		Running,
		Shielded
	}

	private static GameManager _instance;

	public GameStates currentState;
	public PlayerStates currentPlayerState;
	
	public float score;
	public int streetsPrepared;
	public int playerTransition;
	
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

	void Awake() {
		score = 0;
		currentState = GameStates.Mainmenu;
		currentPlayerState = PlayerStates.Idle;
		streetsPrepared = 0;
		playerTransition = 0;
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
					GameManager.Instance.currentState = GameManager.GameStates.Roofs;
					GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
					canvas.SetActive(false);
				}
			}
		} 
		if (Input.GetKey (KeyCode.Space)) {
			GameManager.Instance.currentState = GameManager.GameStates.Roofs;
			GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
			canvas.SetActive(false);
		}
	}
}