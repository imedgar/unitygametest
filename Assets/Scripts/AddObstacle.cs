using UnityEngine;
using System.Collections;

public class AddObstacle : MonoBehaviour {
	
	[SerializeField]
	GameObject prefabToAdd;
	[SerializeField]
	bool addChimney;
	
	// Use this for initialization
	void Start () {
		if (addChimney){
			AddChimney ();
		}
		
	}
	
	void AddChimney (){
		float sizeY = gameObject.GetComponent<BoxCollider2D>().size.y;
		int randomXPosition = Random.Range (1,3);
		float xPosition;
		if (randomXPosition == 1){
			xPosition = gameObject.transform.position.x - ( gameObject.GetComponent<BoxCollider2D>().size.x / 4 );
		} else {
			xPosition = gameObject.transform.position.x + ( gameObject.GetComponent<BoxCollider2D>().size.x / 4 );
		}
		float yPrefabPosition = transform.position.y + 
			( gameObject.GetComponent<BoxCollider2D>().size.y / 2 ) +
			( prefabToAdd.GetComponent<BoxCollider2D>().size.y / 2 );
		(Instantiate (prefabToAdd, 
			new Vector3(xPosition, yPrefabPosition, gameObject.transform.position.z), 
			Quaternion.Euler(0, 0, 0)) 
			as GameObject).transform.parent = gameObject.transform;
	}
}
