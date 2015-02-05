using UnityEngine;
using System.Collections;

public class Block : UnityBehaviour {

	public int Floor;
	public int Row;

//	public bool crashable;
	public int Type;

	bool _chkPress;

	// Use this for initialization
	public override void CM_Start () {
	}
	
	// Update is called once per frame
	public override void CM_Update () {
	}

	void OnClick() {
//		if (!crashable)	return;
		PuzzleMap map = gameObject.transform.parent.GetComponent<PuzzleMap> ();
		map.Delete(Floor, Row);
	}

	void OnPress( bool isPressed ) {
		if (isPressed) {
			Debug.Log ("Press");
			_chkPress = true;
		} 
		else {
			_chkPress = false;
		}
	}

	void OnDragOut() {
		// this time move blog left or right
		if (!_chkPress) return;

		PuzzleMap map = gameObject.transform.parent.GetComponent<PuzzleMap> ();

		Debug.Log ("OnDragOut : " + Floor + ", " + Row);
		Debug.Log( "CustomInput.Instance.MousePos.y : " + (CustomInput.Instance.MousePos.y ) );
		Debug.Log( "transform.localPosition.y : " + transform.localPosition.y );


		if (CustomInput.Instance.MousePos.y  > transform.localPosition.y + 20 || 
		    CustomInput.Instance.MousePos.y  < transform.localPosition.y - 20) {
			Debug.Log("Fail");
			return;
		}
		Debug.Log("Through");



		if (CustomInput.Instance.MousePos.x > transform.localPosition.x && Row < 7) {
			Debug.Log ("Right");
			map.MoveRight( Floor, Row );
		} 
		else if (CustomInput.Instance.MousePos.x < transform.localPosition.x && Row > 0 )  {
			Debug.Log("Left");
			map.MoveLeft( Floor, Row );
		}

		_chkPress = false;

	}

}
