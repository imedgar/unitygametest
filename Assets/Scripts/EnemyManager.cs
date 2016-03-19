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
			InvokeRepeating("SpawnPrefabBerserker",2, 4.0f);
		}
	}
	
	private void SpawnPrefabBerserker(){
		if (GameManager.Instance.CanStartGameLogic ()) {
			if (GameManager.Instance.currentState == GameManager.GameStates.Roofs){
				spawnPositionX = ChumpRayBuilding();
			}
			else if (GameManager.Instance.currentState == GameManager.GameStates.Street){
				spawnPositionX = (int) playerRef.transform.position.x + 50;
			}
            if (spawnPositionX != 0)
            {
                ObjectPool.instance.GetObjectForType("Berserker", true, new Vector3(spawnPositionX, 6, -2), Quaternion.Euler(0, 0, 0));
            }
		}
	}
	
	
    void Behaviour(string whichUpdate, GameManager.GameStates currentGameState)
    {
        if (GameManager.Instance.CanStartGameLogic())
        {
            switch (currentGameState)
            {
                case GameManager.GameStates.Roofs:
                    break;
                case GameManager.GameStates.Street:
                    break;
		        case GameManager.GameStates.InnerZone:
                    break;
                default:
                    break;
            }
        }
    }
	
	private int ChumpRayBuilding (){
        
		int positionX = 0;
		int chumpRayX = (int) playerRef.transform.position.x + distanceToPlayerSpawn;

		for (int i = chumpRayX; i < (chumpRayX + 50); i++ ){
			Vector2 chumpRayPosition = new Vector2(i, 10);
			hit = Physics2D.Raycast(chumpRayPosition, Vector2.down, 200, 1 << LayerMask.NameToLayer("Ground"));
			if (hit) {
	            if (hit.collider.tag == "Ground"){
	                positionX = i;
					break;
				}
			} 
		}
		return positionX;
	}

}