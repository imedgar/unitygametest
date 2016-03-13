using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameStatus : MonoBehaviour {
	
	GameObject deathDetectorRef;

	// Use this for initialization
	void Start () {
		deathDetectorRef = GameObject.FindGameObjectWithTag ("DeathDetector");
		// Set the texture so that it is the the size of the screen and covers it.
	}
	
	// Update is called once per frame
	void Update () {		
		// Streets mode
		if (GameManager.Instance.score * 1.5 > 800){
			transition ();
		}
	}
	
	void transition () {
		GameManager.Instance.currentState = GameManager.GameStates.Street;
		deathDetectorRef.SetActive (false);
	}
}