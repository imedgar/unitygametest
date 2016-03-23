using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	string currentScore = "";
	IEnumerator Start ()
	{
		GUI.depth = 2;
		while (true) {
			if (Time.timeScale == 1 && GameManager.Instance.score > 0) {
				yield return new WaitForSeconds (0.1f);
                currentScore = (Mathf.Round (GameManager.Instance.score)) + " m";
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
	
	void OnGUI ()
	{
		if (GameManager.Instance.CanStartGameLogic()){
	        GUI.Label(new Rect(5, 0, 100, 25), PlayerPrefs.GetInt("lastscore").ToString() + " last");
	        GUI.Label(new Rect (5, 20, 100, 25), PlayerPrefs.GetInt("highscore").ToString() + " max" );
	        GUI.Label(new Rect(5, 40, 100, 25), currentScore);
	        //GUI.Label (new Rect (5, 0, 100, 25), GameManager.Instance.playerSpeed + " spd" );
		}
    }
}
