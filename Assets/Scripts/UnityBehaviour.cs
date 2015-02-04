using UnityEngine;
using System.Collections;

public class UnityBehaviour : MonoBehaviour {

	public virtual void CM_Awake() {}
	public virtual void CM_Start() {}
	public virtual void CM_Update() {}
	public virtual void CM_OnGUI() {}
	public virtual void CM_OnDestroy() {}
	public virtual void CM_OnApplicationQuit() {}
	public virtual void CM_OnMouseDown() {}
	public virtual void CM_OnMouseUp() {}

	protected void Awake() {
		this.CM_Awake ();
	}

	// Use this for initialization
	void Start () {
		this.CM_Start ();
	}
	
	// Update is called once per frame
	void Update () {
		this.CM_Update ();
	}
	
	void OnDestroy() {
		this.CM_OnDestroy();
	}

	void OnApplicationQuit() {
		this.CM_OnApplicationQuit ();
	}

	void OnGUI() {
		this.CM_OnGUI ();
	}

	protected void OnMouseDown() {
		this.CM_OnMouseDown();
	}

	protected void OnMouseUp() {
		this.CM_OnMouseUp(); 
	}
}

public class UnitySingleton<T> : UnityBehaviour where T : UnitySingleton<T>
{
	private static T _instance = null;
	public static T Instance
	{
		get
		{
			if (_instance == null) {
				_instance = FindObjectOfType (typeof(T)) as T;
				if (null == _instance) {
					//UnityEngine.Debug.Log ("Fail to get Instance");
					_instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
				}
			}

			return _instance;
		}
	}

	public new void Awake() {
		DontDestroyOnLoad (this);
		base.Awake ();
	}
}
