using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {
	
	[SerializeField]
	GameObject _player;
	private bool touched;
	[SerializeField]
	SVGImporter.SVGRenderer render;
	Color markedColor;
	Color defaultColor;

	void Start () {
		touched = false;
		defaultColor = render.color;
		markedColor = new Color (255 / 255, 96 / 255, 96 / 255, 255 / 255);
	}
	
	void Update () {
		
		if (touched) {
			Destroy ();		
		}
	}
	
    void OnMouseDown()
    {   
		Marked ();
    }
	
	void Marked (){
		if(!touched && GameManager.Instance.playerCanBreak){ // If can
			touched = true;
			render.color = markedColor;
			GameManager.Instance.playerCanBreak = false;
 		}
	}
	
	void Destroy () {
		if((_player.transform.position.x + 1) >= gameObject.transform.position.x){
			touched = false;
			render.color = defaultColor;
			ObjectPool.instance.PoolObject(gameObject);
		}
	}

}
