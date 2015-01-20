using UnityEngine;
using System.Collections;

public class MoveButton : UnityBehaviour {

	bool _pressed;

	Vector2 mouseXY;

	public override void OnAwake() {
		_pressed = false;
	}

	// Use this for initialization
	public override void OnStart () {
		
	}

	// Update is called once per frame
	public override void OnUpdate () {
		ExcuteMovement ();
	}

	void OnPress(bool isDown) {
		if (isDown) {
			Debug.Log ("Press");
			_pressed = true;
		} 
		else {
			Debug.Log ("Release");
			_pressed = false;
		}
	}

	void ExcuteMovement() {
		// not pressed -> stop
		if (!_pressed)	return;
		Debug.Log (Input.mousePosition);
		transform.localPosition = new Vector3 (mouseXY.x, mouseXY.y);

	}

	public override void OnUpdateGUI () {
		Event currentEvent = Event.current;
		if (currentEvent.isMouse) {
			mouseXY = new Vector2 (currentEvent.mousePosition.x - Screen.width/2, -currentEvent.mousePosition.y + Screen.height/2);
			// AND DO THE REST
		}
		
	}
}
