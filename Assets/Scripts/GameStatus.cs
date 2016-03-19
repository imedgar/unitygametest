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
		// Set the texture so that it is the the size of the screen and covers it.
	}
	
	// Update is called once per frame
	void Update () {	
		if (randomInnerZone() && GameManager.Instance.CanStartGameLogic()){
			toInnerZone();
			Debug.Log ("INNER ZONE");
		}
		
		// Streets mode
		//if (GameManager.Instance.score * 1.5 > 800){
		//	transition ();
		//}
		
		
	}
	
	void transition () {
		GameManager.Instance.currentState = GameManager.GameStates.Street;
		deathDetectorRef.SetActive (false);
	}
	
	void toInnerZone () {
		GameManager.Instance.currentState = GameManager.GameStates.InnerZone;
	}
	
	bool randomInnerZone () {
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
}