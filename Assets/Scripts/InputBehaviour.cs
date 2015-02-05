using UnityEngine;
using System.Collections;

public class InputBehaviour : UnityBehaviour {
	
	public virtual void IP_Awake() {}
	public virtual void IP_Start() {}
	public virtual void IP_Update() {}
	public virtual void IP_OnGUI() {}
	public virtual void IP_OnDestroy() {}
	public virtual void IP_OnApplicationQuit() {}
	public virtual void IP_OnMouseDown() {}
	public virtual void IP_OnMouseUp() {}
	public virtual void IP_OnTouch( string type, int id, float x, float y, float dx, float dy ) {}

	delegate void listener( string type, int id, float x, float y, float dx, float dy );

	event listener _begin0, _begin1, _begin2, _begin3, _begin4;
	event listener _move0, _move1, _move2, _move3, _move4;
	event listener _end0, _end1, _end2, _end3, _end4;
	
	//포인트별 begin의 좌표를 기억해둘 배열
	private Vector2[] _delta = new Vector2[5];

	protected Vector2 _mousePos;

	public override void CM_Awake() {
		_begin0 += onTouch;
		_end0 += onTouch;
		_move0 += onTouch;
		this.IP_Awake ();
	}
	
	// Use this for initialization
	public override void CM_Start () {
		this.IP_Start ();
	}
	
	// Update is called once per frame
	public override void CM_Update () {
#if UNITY_EDITOR
		if ( Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) ) {
			int id = 0;
			Vector3 pos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) * GameObject.Find("UI Root").GetComponent<UIRoot>().activeHeight/2;
			Vector2 pos = new Vector2( pos3.x, pos3.y );
			if ( Input.GetMouseButtonDown(0) ) _delta[id] = pos;
			
			//좌표계 정리
			float x, y, dx, dy;
			x = pos.x;
			y = pos.y;
			if( Input.GetMouseButtonDown(0) ){
				dx = dy = 0;
			}
			else{
				dx = pos.x - _delta[id].x;
				dy = pos.y - _delta[id].y;
			}
			
			if ( Input.GetMouseButtonDown(0) ) {
				if(_begin0!=null) _begin0( "begin",id,x,y,dx,dy );
			}
			else if ( Input.GetMouseButton(0) ) {
				if(_move0!=null) _move0( "move",id,x,y,dx,dy );
			}
			else if ( Input.GetMouseButtonUp(0) ) {
				if(_end0!=null) _end0( "end",id,x,y,dx,dy );
			}
			
		}
#elif UNITY_IPHONE
		int count = Input.touchCount;
		
		if ( count > 0 ) {
			for( int i = 0; i < count; i++ ){
				Touch touch = Input.GetTouch(i);
				int id = touch.fingerId;
				
				//터치좌표
				Vector2 pos = touch.position;
				
				//begin이라면 무조건 delta에 넣어주자.
				if( touch.phase == TouchPhase.Began ) _delta[id] = touch.position;
				
				//좌표계 정리
				float x, y, dx, dy;
				x = pos.x;
				y = pos.y;
				if( touch.phase == TouchPhase.Began ){
					dx = dy = 0;
				}else{
					dx = pos.x - _delta[id].x;
					dy = pos.y - _delta[id].y;
				}
				
				//상태에 따라 이벤트를 호출하자
				if( touch.phase == TouchPhase.Began ){
					switch( id ){
					case 0: if(_begin0!=null) _begin0( "begin",id,x,y,dx,dy ); break;
					case 1: if(_begin1!=null) _begin1( "begin",id,x,y,dx,dy ); break;
					case 2: if(_begin2!=null) _begin2( "begin",id,x,y,dx,dy ); break;
					case 3: if(_begin3!=null) _begin3( "begin",id,x,y,dx,dy ); break;
					case 4: if(_begin4!=null) _begin4( "begin",id,x,y,dx,dy ); break;
					}
				}else if( touch.phase == TouchPhase.Moved ){
					switch( id ){
					case 0: if(_move0!=null) _move0( "move",id,x,y,dx,dy ); break;
					case 1: if(_move1!=null) _move1( "move",id,x,y,dx,dy ); break;
					case 2: if(_move2!=null) _move2( "move",id,x,y,dx,dy ); break;
					case 3: if(_move3!=null) _move3( "move",id,x,y,dx,dy ); break;
					case 4: if(_move4!=null) _move4( "move",id,x,y,dx,dy ); break;
					}
				}else if( touch.phase == TouchPhase.Ended ){
					switch( id ){
					case 0: if(_end0!=null) _end0( "end",id,x,y,dx,dy ); break;
					case 1: if(_end1!=null) _end1( "end",id,x,y,dx,dy ); break;
					case 2: if(_end2!=null) _end2( "end",id,x,y,dx,dy ); break;
					case 3: if(_end3!=null) _end3( "end",id,x,y,dx,dy ); break;
					case 4: if(_end4!=null) _end4( "end",id,x,y,dx,dy ); break;
					}
				}
			}
			
		}
#endif
		_mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y ) * GameObject.Find("UI Root").GetComponent<UIRoot>().activeHeight/2;
		this.IP_Update ();
	}
	
	public override void CM_OnDestroy() {
		this.IP_OnDestroy();
	}
	
	public override void CM_OnApplicationQuit() {
		this.IP_OnApplicationQuit ();
	}
	
	public override void CM_OnGUI() {
		this.IP_OnGUI ();
	}
	
	public override void CM_OnMouseDown() {
		this.IP_OnMouseDown();
	}
	
	public override void CM_OnMouseUp() {
		this.IP_OnMouseUp(); 
	}

	private void onTouch( string type, int id, float x, float y, float dx, float dy){
		switch( type ){
		case"begin": 
//			Debug.Log( "down:"+x+","+y ); 
			break;
		case"end": 
//			Debug.Log( "end:"+x+","+y+", d:"+dx+","+dy ); 
			break;
		case"move": 
//			Debug.Log( "move:"+x+ ","+y+", d:"+dx+","+dy ); 
			break;
		}

		this.IP_OnTouch( type, id, x, y, dx, dy);
	}
}