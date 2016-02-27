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
}