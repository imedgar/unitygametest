using UnityEngine;
using System.Collections;

public class AddObstacle : MonoBehaviour {
	
	[SerializeField]
	GameObject prefabToAdd;
	
	// Use this for initialization
	void Start () {
		AddChimney ();	
	}
	
	void AddChimney (){
		float sizeY = gameObject.GetComponent<BoxCollider2D>().size.y;
		float yPrefabPosition = transform.position.y + 
			( gameObject.GetComponent<BoxCollider2D>().size.y / 2 ) +
			( prefabToAdd.GetComponent<BoxCollider2D>().size.y / 2 );
		(Instantiate (prefabToAdd, 
			new Vector3(gameObject.transform.position.x, yPrefabPosition, gameObject.transform.position.z), 
			Quaternion.Euler(0, 0, 0)) 
			as GameObject).transform.parent = gameObject.transform;
	}
}
