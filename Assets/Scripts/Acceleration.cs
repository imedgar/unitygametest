using UnityEngine;
using System.Collections;

public class Acceleration : MonoBehaviour {
	
	[SerializeField]
	private bool doAccelerate;
	[SerializeField]
	private float accelerationCap;
	
	
	private float accelerationRate = 0.02f;

	private void Start(){
		if (doAccelerate) {
			InvokeRepeating("Accelerate", 0, 3.0f);
		}
	}
	
	private void Accelerate () {
		if (Time.timeScale <= accelerationCap){
			Time.timeScale += accelerationRate;
		}
	}
		
}
