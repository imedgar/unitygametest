using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public float velocity = 0f;
	public GameObject target;
	public float offsetY;
	private Renderer rendererRef;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		rendererRef = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Behaviour (GameManager.Instance.currentState);
	}
	
	void Behaviour (GameManager.GameStates currentGameState){
		
		if (target.transform.position.y < -4){
			gameObject.transform.position = new Vector3 (target.transform.position.x + 9, 
			-3 + 2 + offsetY, 
			transform.position.z);
		}
		else{
			gameObject.transform.position = new Vector3 (target.transform.position.x + 9, 
			4 + 2 + offsetY, 
			transform.position.z);
		}
		
		if (GameManager.Instance.CanStartGameLogic())
        {
            switch (currentGameState)
            {
                case GameManager.GameStates.Mainmenu:
                    break;
                case GameManager.GameStates.Roofs:
                case GameManager.GameStates.InnerZone:
					rendererRef.material.mainTextureOffset = new Vector2 ((Time.time * velocity) % 1, 0);
                    break;
                default:
                    break;
            }
		}
	}
}
