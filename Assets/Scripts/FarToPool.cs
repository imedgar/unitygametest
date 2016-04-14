using UnityEngine;
using System.Collections;

public class FarToPool : MonoBehaviour {

	private GameObject camRef;
	[SerializeField]
	bool parentMoving;
	[SerializeField]
	int distanceToPool = 30;

	// Use this for initialization
	void Start () {
		// pair camera to camera reference
		camRef = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		if ((camRef.transform.position.x) - (gameObject.transform.position.x + 0.1) >= distanceToPool && GameManager.Instance.CanStartGameLogic())
		{
			BacktoPool();
		}
        if ((camRef.transform.position.y) - (gameObject.transform.position.y + 0.1) >= distanceToPool && GameManager.Instance.CanStartGameLogic())
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
		if (!parentMoving){
        	transform.Translate(Vector3.left * GameManager.Instance.naturalWorldSpeed * Time.deltaTime);
		}
    }

	void BacktoPool () {
		ObjectPool.instance.PoolObject(gameObject);
	}
}
