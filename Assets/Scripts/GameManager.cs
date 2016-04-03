using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameStates {
		Intro,
		Mainmenu,
		LevelSelection,
		Ingame,
		Roofs,
		Street,
		InnerZone
	}

	public enum PlayerStates {
		Idle,
		Running,
		Shielded
	}

	private static GameManager _instance;

	public GameStates currentState;
	public PlayerStates currentPlayerState;
    public float naturalWorldSpeed;
    public float naturalWorldSpeedCap;
    public float naturalWorldAcceleration;

	// Player stuff
	public float score;
	public int streetsPrepared;
	public float playerSpeed;
	public int playerTransition;
	public bool playerEnteredInnerZone;
	
	// Controller Android
	public RuntimePlatform platform;
	public bool leftTapConfiguration;
	
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
        naturalWorldSpeed = 9.8f;
        naturalWorldAcceleration = 0.002f;
        naturalWorldSpeedCap = 13f;
        playerEnteredInnerZone = true;
		platform = Application.platform;
		leftTapConfiguration = true;
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

	public bool CanStartGameLogic (){
		if (currentState == GameStates.Mainmenu) {
			return false;
		} else {
			return true;
		}
	}

	public void ActiveGameLogic (){
		switch (platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
				if (Input.touchCount > 0) {
					if (Input.GetTouch (0).phase == TouchPhase.Began) {
						GameLogicBegin();
					}
				}
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsWebPlayer:
				if (Input.GetKey (KeyCode.Space)) {
					GameLogicBegin();
				}
                break;
            default:
                break;
        }
		
	}
	
	public void GameRestart (){
        PlayerPrefs.SetInt("lastscore", (int)score);
        if (score > PlayerPrefs.GetInt("highscore"))
	     {
	           PlayerPrefs.SetInt("highscore", (int) score);
	     }
		GameManager.Instance.currentState = GameManager.GameStates.Mainmenu;
		Application.LoadLevel (Application.loadedLevel);
	}
	
	private void GameLogicBegin (){
		GameManager.Instance.currentState = GameManager.GameStates.Roofs;
		GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
		canvas.SetActive(false);		
	}

}