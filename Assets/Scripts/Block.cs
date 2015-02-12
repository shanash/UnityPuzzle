using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public int Floor;
	public int Row;

//	public bool crashable;
	public int Type;

	private bool _chkPress = false;

	// Use this for initialization
	public void Start () {
	}

	public void OnDestory() {
	}
	
	// Update is called once per frame
	public void Update () {
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
		Debug.Log ("OnDragOut");
		//if (!_chkPress) return;

		PuzzleMap map = gameObject.transform.parent.GetComponent<PuzzleMap> ();
		Vector2 mouse = InputManager.Instance.Mouse;

		Debug.Log ("OnDragOut : " + Floor + ", " + Row);
		Debug.Log( "CustomInput.Instance.MousePos.y : " + (mouse.y ) );
		Debug.Log( "transform.localPosition.y : " + transform.localPosition.y );

		if (mouse.y  > transform.localPosition.y + 20 || 
		    mouse.y  < transform.localPosition.y - 20) {
			Debug.Log("Fail");
			return;
		}

		Debug.Log("Through");

		if (mouse.x > transform.localPosition.x && Row < 7) {
			Debug.Log ("Right");
			map.MoveRight( Floor, Row );
		} 
		else if (mouse.x < transform.localPosition.x && Row > 0 )  {
			Debug.Log("Left");
			map.MoveLeft( Floor, Row );
		}

		_chkPress = false;
	}
}
