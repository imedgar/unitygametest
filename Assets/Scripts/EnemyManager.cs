using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]
	public bool spawnEnemyBerserker;
    [SerializeField]
    public int distanceToPlayerSpawn;
	Vector2 spawnPoint;
	Vector2 powerUpPoint;
	private RaycastHit2D hit;

	private void Start(){
		if (spawnEnemyBerserker) {
			InvokeRepeating("SpawnPrefabBerserker",2, 3.75f);
		}
		InvokeRepeating("PowerUpSpawn",2, 5.0f);
	}
	
	private void SpawnPrefabBerserker(){
		if (GameManager.Instance.CanStartGameLogic ()) {
			if (GameManager.Instance.currentState == GameManager.GameStates.Roofs){
				spawnPoint = ChumpRayBuilding();
			}
            if (spawnPoint.x != 0)
            {
                ObjectPool.instance.GetObjectForType("Berserker", true, 
					new Vector3(spawnPoint.x, spawnPoint.y, -2), Quaternion.Euler(0, 0, 0));
				spawnPoint = new Vector2 (0,0);
            }
		}
	}
	
	private Vector2 ChumpRayBuilding (){
		
		for (int i = 25; i > 0; i-- ){
			Vector2 chumpRayPosition = new Vector2(i, 10);
<<<<<<< HEAD
			hit = Physics2D.Raycast(chumpRayPosition, Vector2.down, 200, 1 << LayerMask.NameToLayer("Ground"));
			if (hit) {
				if(hit.transform.tag == "Ground"){
					return hit.point;
=======
			hit = Physics2D.Raycast(chumpRayPosition, Vector2.down, 20, 1 << LayerMask.NameToLayer("Ground"));
			if (hit && i > 10) {
				if (hit.transform.tag == "Ground"){
					//return new Vector2 (hit.transform.position.x, hit.point.y);
					return new Vector2 (GetRandomSpawnPointOnBuilding(hit.collider.gameObject), hit.point.y);
>>>>>>> recuperar_error
				}
			} 
		}
		return new Vector2 (0,0);
	}
	
	void PowerUpSpawn () {
		if (GameManager.Instance.CanStartGameLogic()){
			int randomInt;
			randomInt = Random.Range(1,6);
			if (randomInt.Equals(3)) {
				powerUpPoint = ChumpRayBuilding();
				if (powerUpPoint.x != 0)
	            {
					ObjectPool.instance.GetObjectForType("PowerUp", true, 
						new Vector3(powerUpPoint.x, powerUpPoint.y + 1, -2), 
						Quaternion.Euler(0, 0, 0));
					powerUpPoint = new Vector2 (0,0);
	            }
			}
		}
	}
	
	float GetRandomSpawnPointOnBuilding (GameObject building) {
		
		float size = building.GetComponent<BoxCollider2D>().size.x;
		float position = 0f;
		int randomInt;
		randomInt = Random.Range(1,6);
		switch (randomInt) {
			case 1:
				position = building.transform.position.x + (size / 6);
				break;
			case 2:
				position = building.transform.position.x - (size / 6);
				break;
			case 3:
				position = building.transform.position.x + (size / 4);
				break;
			case 4:
				position = building.transform.position.x - (size / 4);
				break;
			case 5:
				position = building.transform.position.x;
				break;
		}
		return position;
	}

}