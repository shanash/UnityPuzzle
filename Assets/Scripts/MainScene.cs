using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : UnityBehaviour {

	private Stack<string> _histories = new Stack<string> ();
	private string _currentScene = string.Empty;

	private static SceneManager _instance = null;
	public static SceneManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType (typeof(SceneManager)) as SceneManager;
				if (null == _instance) {
					_instance = new GameObject("SceneManager").AddComponent<SceneManager>();
				}
			}

			return _instance;
		}
	}

	public void Next(string sceneName) {
		_histories.Push (_currentScene);

		Application.LoadLevel (sceneName);
		_currentScene = sceneName;
	}

	public void Prev() {
		string scene = _histories.Pop ();
		Application.LoadLevel (scene);
	}
}

public class MainScene : UnityBehaviour {

	public override void OnAwake() {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	public override void OnStart () {
	}
	
	// Update is called once per frame
	public override void OnUpdate () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			this.OnPressBack ();
		}
	}

	public void OnPressBack() {
		SceneManager.Instance.Prev ();
	}
}
	