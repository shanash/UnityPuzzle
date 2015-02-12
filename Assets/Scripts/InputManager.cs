using UnityEngine;
using System.Collections.Generic;


public enum eTouchPhase
{
	kTouchBegan = 0,
	kTouchMoved,
	kTouchEnded
}

public interface IInputEventListener
{
	void OnTouchEvent(eTouchPhase phase, int fingerId, Vector2 pos, Vector2 delta);
}

public class InputManager : MonoBehaviour
{
	#region Singleton.
	private static InputManager _instance = null;
	public static InputManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(InputManager)) as InputManager;
				if (_instance == null)
				{
					_instance = new GameObject(typeof(InputManager).ToString()).AddComponent<InputManager>();
					DontDestroyOnLoad(_instance.gameObject);
				}
			}

			return _instance;
		}
	}
	#endregion

	private const int kMaxFinger = 5;

	private List<IInputEventListener> _eventListeners = new List<IInputEventListener>();
	private Vector2[] _beganPos = new Vector2[kMaxFinger];
	private Vector2 _mouse = Vector2.zero;

	public Vector2 Mouse
	{
		get { return _mouse; }
	}

	public void Init()
	{
		this.ClearEventListeners();

		_beganPos = new Vector2[kMaxFinger];
		_mouse = Vector2.zero;
	}

	public void ClearEventListeners()
	{
		_eventListeners.Clear();
	}

	public void AddEventListener(IInputEventListener listener)
	{
		if (!this.IsExistEventListener(listener)) 
		{
			_eventListeners.Add(listener);
		}
	}

	public void RemoveEventListener(IInputEventListener listener)
	{
		if (this.IsExistEventListener(listener)) 
		{
			_eventListeners.Remove(listener);
		}
	}

	private bool IsExistEventListener(IInputEventListener listener)
	{
		return _eventListeners.Exists(delegate(IInputEventListener key) {
				return (key == listener);
			});
	}

	void Update()
	{
		UIRoot root = GameObject.FindObjectOfType(typeof(UIRoot)) as UIRoot;
		if (root == null)
		{
			Debug.LogWarning("UIRoot Not Found.");
			return;
		}

#if UNITY_EDITOR
		Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) * root.activeHeight / 2;
		_mouse.x = v.x;
		_mouse.y = v.y;

		int touchCount = 1; //Only Mouse LeftButton
#else
		int touchCount = Math.Min(Input.touchCount, kMaxFinger);
#endif

		for (int i = 0; i < touchCount; ++i)
		{
#if UNITY_EDITOR
			int fingerId = 0;

			bool began = Input.GetMouseButtonDown(i);
			bool moved = Input.GetMouseButton(i);
			bool ended = Input.GetMouseButtonUp(i);
#else
			Touch touch = Input.GetTouch(i);
			int fingerId = touch.fingerId;

			bool began = (touch.phase == TouchPhase.Began);
			bool moved = (touch.phase == TouchPhase.Moved);
			bool ended = (touch.phase == TouchPhase.Ended);
#endif

			if (!began && !moved && !ended)
			{
				continue;
			}

#if UNITY_EDITOR
			Vector2 p = new Vector2(Mouse.x, Mouse.y);
#else
			Vector2 p = touch.position;
#endif
			Vector2 d = Vector2.zero;

			if (began)
			{
				_beganPos[fingerId] = p;
			}
			else
			{
				d.x = p.x - _beganPos[fingerId].x;
				d.y = p.y - _beganPos[fingerId].y;
			}

			_eventListeners.ForEach(delegate(IInputEventListener listener) {
				if (began) listener.OnTouchEvent(eTouchPhase.kTouchBegan, fingerId, p, d);
				if (moved) listener.OnTouchEvent(eTouchPhase.kTouchMoved, fingerId, p, d);
				if (ended) listener.OnTouchEvent(eTouchPhase.kTouchEnded, fingerId, p, d);
			});
		}
	}
}
