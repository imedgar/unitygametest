using UnityEngine;
using System.Collections;

public class ParallaxEffect : MonoBehaviour {
	
	[SerializeField]
	private float speed;
	private Transform camRefCached;
	private Transform transformCached;
	private Transform playerTransformCached;

	// Use this for initialization
	void Start () {
		this.camRefCached = GameObject.Find("Camera").GetComponent<Transform> ();
		this.transformCached = GetComponent<Transform> ();
		this.playerTransformCached =  GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(playerTransformCached.position.x - transformCached.position.x > 5){
			transformCached.position = new Vector3(playerTransformCached.position.x + 20, transformCached.position.y, transformCached.position.z);
		}
		Scroll();
	
	}
	
	void Scroll () {
		transformCached.Translate(Vector3.left * speed * Time.deltaTime);
	}
}
