using UnityEngine;
using System.Collections;

public class FarToPool : MonoBehaviour {

	private GameObject camRef;

	// Use this for initialization
	void Start () {
		// pair camera to camera reference
		camRef = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		if ((camRef.transform.position.x) - (gameObject.transform.position.x + 0.1) >= 30 && GameManager.Instance.CanStartGameLogic())
		{
			BacktoPool();
		}
        if (GameManager.Instance.CanStartGameLogic())
        {
            objectNaturalMoving();
        }
        //if (GameManager.Instance.currentState == GameManager.GameStates.Street && gameObject.tag == "Ground"){
        //	boxCol.enabled = false;
        //} else if (GameManager.Instance.currentState != GameManager.GameStates.Street && gameObject.tag == "Ground"){
        //	boxCol.enabled = true;
        //}
    }

    void objectNaturalMoving() {
        transform.Translate(Vector3.left * GameManager.Instance.naturalWorldSpeed * Time.deltaTime);
    }

	void BacktoPool () {
		ObjectPool.instance.PoolObject(gameObject);
	}
}
