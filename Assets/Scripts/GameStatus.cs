using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameStatus : MonoBehaviour {
	
	GameObject deathDetectorRef;
    float timeStampInnerZone;
	[SerializeField]
    float innerZoneCooldown;
	
	// Use this for initialization
	void Start () {
		deathDetectorRef = GameObject.FindGameObjectWithTag ("DeathDetector");
	}
	
	// Update is called once per frame
	void Update () {
		
		ToInnerZone ();
		InnerZoom ();

		// Streets mode
		//if (GameManager.Instance.score * 1.5 > 800){
		//	ToStreets ();
		//}
	}
	
	void ToStreets () {
		GameManager.Instance.currentState = GameManager.GameStates.Street;
		deathDetectorRef.SetActive (false);
	}
	
	void ToInnerZone () {
		if (RandomInnerZone() && GameManager.Instance.CanStartGameLogic()){
			GameManager.Instance.currentState = GameManager.GameStates.InnerZone;
			Debug.Log ("INNER ZONE");
		}
	}
	
	bool RandomInnerZone () {
		if (timeStampInnerZone <= Time.time)
        {
			int randomInt;
			randomInt = Random.Range(1,1000);
			if (randomInt % 501 == 0) {
				timeStampInnerZone = Time.time + innerZoneCooldown;

				return true;
			}
		}
		return false;
	}
	
	void InnerZoom () {
		if (GameManager.Instance.playerEnteredInnerZone){
			if (Camera.main.orthographicSize > 7){
				Camera.main.orthographicSize -= 1f * Time.deltaTime; 
			}
			
		} else {
			if (Camera.main.orthographicSize < 8 && GameManager.Instance.score * 1.5 > 10){
				Camera.main.orthographicSize += 1f * Time.deltaTime; 
			}
		}
	}
	public void StartGame() {
		// Activate game
        GameManager.Instance.ActiveGameLogic();
	}
}