using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	string label = "";
	
	IEnumerator Start ()
	{
		GUI.depth = 2;
		while (true) {
			if (Time.timeScale == 1) {
				yield return new WaitForSeconds (0.1f);
				label = "Meters :" + (Mathf.Round (GameManager.Instance.score * 1.5f));
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (5, 40, 100, 25), label);
	}
}
