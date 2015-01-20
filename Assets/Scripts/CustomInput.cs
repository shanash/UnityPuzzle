using UnityEngine;
using System.Collections;

public class CustomInput : UnitySingleton<CustomInput> {

	private int _count = 0;

	public override void OnAwake ()
	{
	}
	
	public void Foo ()
	{
		UnityEngine.Debug.Log ("Foo " + _count + " times");
		_count++;
	}
		
	public Vector3 MousePos;

	// Use this for initialization
	public override void OnStart () {
	
	}
	
	// Update is called once per frame
	public override void OnUpdate () {
	
	}

	public override void OnUpdateGUI () {
		Event currentEvent = Event.current;
		if (currentEvent.isMouse) {
			MousePos = new Vector3 (currentEvent.mousePosition.x - Screen.width/2, -currentEvent.mousePosition.y + Screen.height/2);
			// AND DO THE REST
		}
		
	}
}
