using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MainScene : MonoBehaviour, IInputEventListener
{
	public void Awake()
	{
		InputManager.Instance.Init();
	}

	// Use this for initialization
	public void Start()
	{
		InputManager.Instance.AddEventListener(this);
	}

	public void OnDestory()
	{
		InputManager.Instance.RemoveEventListener(this);
	}

	public void OnTouchEvent(eTouchPhase phase, int fingerId, Vector2 pos, Vector2 delta)
	{
		Debug.Log("OnTouchEvent: " + phase.ToString());
	}
}
