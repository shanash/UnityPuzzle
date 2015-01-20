using UnityEngine;
using System.Collections;

public class CrashBlock : UnityBehaviour {

	// Use this for initialization
	public override void OnStart () {
	
	}
	
	// Update is called once per frame
	public override void OnUpdate () {
	
	}

	void OnClick() {
		Debug.Log ("Click");
		PuzzleMap map = gameObject.transform.parent.GetComponent<PuzzleMap> ();
		map.Delete (gameObject.GetComponent<BlockInfo> ().Floor, gameObject.GetComponent<BlockInfo> ().Raw);
	}


	void OnDragOut() {
		// this time move blog left or right
		Debug.Log ("OnDragOut");
	}
}
