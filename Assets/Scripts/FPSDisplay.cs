using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	//float deltaTime = 0.0f;
	//
	//void Update()
	//{
	//	deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	//}
	//
	//void OnGUI()
	//{
	//	int w = Screen.width, h = Screen.height;
	//	
	//	GUIStyle style = new GUIStyle();
	//	
	//	Rect rect = new Rect(0, 0, w, h * 2 / 100);
	//	style.alignment = TextAnchor.UpperLeft;
	//	style.fontSize = h * 2 / 30;
	//	style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
	//	float msec = deltaTime * 1000.0f;
	//	float fps = 1.0f / deltaTime;
	//	string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
	//	GUI.Label(rect, text, style);
	//}

	string label = "";
	float count;
	
	IEnumerator Start ()
	{
		GUI.depth = 2;
		while (true) {
			if (Time.timeScale == 1) {
				yield return new WaitForSeconds (0.1f);
				count = (1 / Time.deltaTime);
				label = (Mathf.Round (count)) + " fps";
			} else {
				label = "Pause";
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (450, 20, 100, 25), label);
	}
}
