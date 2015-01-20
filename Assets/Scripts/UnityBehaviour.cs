using UnityEngine;
using System.Collections;

public class UnityBehaviour : MonoBehaviour {

	public virtual void OnAwake() {}
	public virtual void OnStart() {}
	public virtual void OnUpdate() {}
	public virtual void OnUpdateGUI() {}
	public virtual void OnQuit() {}
	public virtual void OnAppQuit() {}

	protected void Awake() {
		this.OnAwake ();
	}

	// Use this for initialization
	void Start () {
		this.OnStart ();
	}
	
	// Update is called once per frame
	void Update () {
		this.OnUpdate ();
	}
	
	void OnDestroy() {
		this.OnQuit();
	}

	void OnApplicationQuit() {
		this.OnAppQuit ();
	}

	void OnGUI() {
		this.OnUpdateGUI ();

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
