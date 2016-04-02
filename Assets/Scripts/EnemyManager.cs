using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	[SerializeField]
	public bool spawnEnemyBerserker;
    [SerializeField]
    public int distanceToPlayerSpawn;

    public GameObject playerRef;

    int spawnPositionX;
	private RaycastHit2D hit;

	private void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player");
		if (spawnEnemyBerserker) {
			InvokeRepeating("SpawnPrefabBerserker",2, 3.75f);
		}
	}
	
	private void SpawnPrefabBerserker(){
		if (GameManager.Instance.CanStartGameLogic ()) {
			if (GameManager.Instance.currentState == GameManager.GameStates.Roofs){
				spawnPositionX = ChumpRayBuilding();
			}
            if (spawnPositionX != 0)
            {
                ObjectPool.instance.GetObjectForType("Berserker", true, new Vector3(( spawnPositionX - 2 ), 5.5f, -2), Quaternion.Euler(0, 0, 0));
				spawnPositionX = 0;
            }
		}
	}
	
	private int ChumpRayBuilding (){
        
		int positionX;

		for (int i = 25; i > 0; i-- ){
			Vector2 chumpRayPosition = new Vector2(i, 10);
			hit = Physics2D.Raycast(chumpRayPosition, Vector2.down, 200, 1 << LayerMask.NameToLayer("Ground"));
			
			if (hit) {
				if(hit.transform.tag == "Ground"){
	                positionX = i;
					return positionX;
				}
			} 
		}
		return 0;
	}

}