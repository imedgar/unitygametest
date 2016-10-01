using UnityEngine;
using System.Collections;

public class FarToPool : MonoBehaviour {

	private GameObject camRef;
	private Transform camTransformCached;
	private Transform objectTransformCached;
	[SerializeField]
	bool parentMoving;
	[SerializeField]
	int distanceToPool = 30;

	// Use this for initialization
	void Start () {
		// pair camera to camera reference
		this.camRef = GameObject.Find("Camera");
		this.camTransformCached = camRef.GetComponent<Transform> ();
		this.objectTransformCached = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((camTransformCached.position.x) - (objectTransformCached.position.x + 0.1) >= distanceToPool && GameManager.Instance.CanStartGameLogic())
		{
			BacktoPool();
		}
        if ((camTransformCached.position.y) - (objectTransformCached.position.y + 0.1) >= distanceToPool && GameManager.Instance.CanStartGameLogic())
        {
            BacktoPool();
        }
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
