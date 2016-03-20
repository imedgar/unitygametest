using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	string label = "";
	IEnumerator Start ()
	{
		GUI.depth = 2;
		while (true) {
			if (Time.timeScale == 1 && GameManager.Instance.score > 0) {
				yield return new WaitForSeconds (0.1f);
				label = (Mathf.Round (GameManager.Instance.score * 1.5f)) + " m";
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (5, 20, 100, 25), label);
		GUI.Label (new Rect (5, 40, 100, 25), PlayerPrefs.GetInt("highscore").ToString() + " max" );
	}
}
