using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameStatus : MonoBehaviour {
	
	GameObject deathDetectorRef;
    float timeStampInnerZone;
	[SerializeField]
    float innerZoneCooldown;
    int score = 0;
	Camera mainCamRef;
	// Use this for initialization
	void Start () {
		deathDetectorRef = GameObject.FindGameObjectWithTag ("DeathDetector");
		mainCamRef = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.CanStartGameLogic())
        {
			if (Time.timeScale != 0f){
	            score++;
	            GameManager.Instance.score = (int)(score * 0.2);
			}
            if (GameManager.Instance.naturalWorldSpeed < GameManager.Instance.naturalWorldSpeedCap)
            {
                GameManager.Instance.naturalWorldSpeed += GameManager.Instance.naturalWorldAcceleration;
            }
        }

        ToInnerZone ();
		InnerZoom ();
	}
	
	void ToStreets () {
		GameManager.Instance.currentState = GameManager.GameStates.Street;
		deathDetectorRef.SetActive (false);
	}
	
	void ToInnerZone () {
		if (RandomInnerZone() && GameManager.Instance.CanStartGameLogic()){
			GameManager.Instance.currentState = GameManager.GameStates.InnerZone;
		}
	}
	
	bool RandomInnerZone () {
		if (timeStampInnerZone <= Time.time)
        {
			int randomInt;
			randomInt = Random.Range(1,1001);
			if (randomInt % 500 == 0) {
				timeStampInnerZone = Time.time + innerZoneCooldown;

				return true;
			}
		}
		return false;
	}
	
	void InnerZoom () {
		if (GameManager.Instance.playerEnteredInnerZone){
            
            if (mainCamRef.orthographicSize > 7){
                mainCamRef.orthographicSize -= 1f * Time.deltaTime; 
			}
			
		} else {
            if (mainCamRef.orthographicSize < 8 && GameManager.Instance.score * 1.5 > 10){
                mainCamRef.orthographicSize += 1f * Time.deltaTime; 
			}
		}
	}
	public void StartGame() {
		// Activate game
        GameManager.Instance.ActiveGameLogic();
	}
}