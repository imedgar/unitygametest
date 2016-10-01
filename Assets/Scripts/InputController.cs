using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	
	public delegate void OnAction(string action);
	public static event OnAction onMove;
	public static event OnAction onJump;
	public static event OnAction onAttack;
	
	// Handle our Ray and Hit
	void Update () {

		onMove("RIGHT");
		
		//if (Input.GetKeyDown(KeyCode.R)) {	// Attack key
        //	onAttack("");
		//}
		
	}	
	
	void FixedUpdate () {
		
		if(Input.GetKey(KeyCode.W)) {	// Jump	
			onJump("");
		}
		
	}
}
