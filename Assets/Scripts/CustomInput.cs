using UnityEngine;
using System.Collections;

public class CustomInput : UnitySingleton<CustomInput> {

	private int _count = 0;

	public override void CM_Awake ()
	{
		begin0 += onTouch;
		end0 += onTouch;
		move0 += onTouch;

	}
	
	public void Foo ()
	{
		UnityEngine.Debug.Log ("Foo " + _count + " times");
		_count++;
	}
		
	public Vector3 MousePos;

	// Use this for initialization
	public override void CM_Start () {
	
	}

	delegate void listener( string type, int id, float x, float y, float dx, float dy );

	
	event listener begin0, begin1, begin2, begin3, begin4;
	event listener move0, move1, move2, move3, move4;
	event listener end0, end1, end2, end3, end4;

	//포인트별 begin의 좌표를 기억해둘 배열
	private Vector2[] delta = new Vector2[5];


	// Update is called once per frame
	public override void CM_Update () {

#if UNITY_EDITOR
		if ( Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) ) {
			int id = 0;
			Vector3 pos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) * GameObject.Find("UI Root").GetComponent<UIRoot>().activeHeight/2;
			Vector2 pos = new Vector2( pos3.x, pos3.y );
			if ( Input.GetMouseButtonDown(0) ) delta[id] = pos;

			//좌표계 정리
			float x, y, dx, dy;
			x = pos.x;
			y = pos.y;
			if( Input.GetMouseButtonDown(0) ){
				dx = dy = 0;
			}
			else{
				dx = pos.x - delta[id].x;
				dy = pos.y - delta[id].y;
			}

			if ( Input.GetMouseButtonDown(0) ) {
				if(begin0!=null) begin0( "begin",id,x,y,dx,dy );
			}
			else if ( Input.GetMouseButton(0) ) {
				if(move0!=null) move0( "move",id,x,y,dx,dy );
			}
			else if ( Input.GetMouseButtonUp(0) ) {
				if(end0!=null) end0( "end",id,x,y,dx,dy );
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
				if( touch.phase == TouchPhase.Began ) delta[id] = touch.position;
				
				//좌표계 정리
				float x, y, dx, dy;
				x = pos.x;
				y = pos.y;
				if( touch.phase == TouchPhase.Began ){
					dx = dy = 0;
				}else{
					dx = pos.x - delta[id].x;
					dy = pos.y - delta[id].y;
				}
				
				//상태에 따라 이벤트를 호출하자
				if( touch.phase == TouchPhase.Began ){
					switch( id ){
					case 0: if(begin0!=null) begin0( "begin",id,x,y,dx,dy ); break;
					case 1: if(begin1!=null) begin1( "begin",id,x,y,dx,dy ); break;
					case 2: if(begin2!=null) begin2( "begin",id,x,y,dx,dy ); break;
					case 3: if(begin3!=null) begin3( "begin",id,x,y,dx,dy ); break;
					case 4: if(begin4!=null) begin4( "begin",id,x,y,dx,dy ); break;
					}
				}else if( touch.phase == TouchPhase.Moved ){
					switch( id ){
					case 0: if(move0!=null) move0( "move",id,x,y,dx,dy ); break;
					case 1: if(move1!=null) move1( "move",id,x,y,dx,dy ); break;
					case 2: if(move2!=null) move2( "move",id,x,y,dx,dy ); break;
					case 3: if(move3!=null) move3( "move",id,x,y,dx,dy ); break;
					case 4: if(move4!=null) move4( "move",id,x,y,dx,dy ); break;
					}
				}else if( touch.phase == TouchPhase.Ended ){
					switch( id ){
					case 0: if(end0!=null) end0( "end",id,x,y,dx,dy ); break;
					case 1: if(end1!=null) end1( "end",id,x,y,dx,dy ); break;
					case 2: if(end2!=null) end2( "end",id,x,y,dx,dy ); break;
					case 3: if(end3!=null) end3( "end",id,x,y,dx,dy ); break;
					case 4: if(end4!=null) end4( "end",id,x,y,dx,dy ); break;
					}
				}
			}

		}
#endif
	}


	public override void CM_OnMouseDown() {
		Debug.Log( "Down" );
	}

	public override void CM_OnMouseUp() {
		Debug.Log( "Up" );
	}

	void onTouch( string type, int id, float x, float y, float dx, float dy){
		switch( type ){
		case"begin": 
			Debug.Log( "down:"+x+","+y ); 
			MousePos = new Vector3(x,y);
			break;
		case"end": 
			Debug.Log( "end:"+x+","+y+", d:"+dx+","+dy ); 
			MousePos = new Vector3(x,y);
			break;
		case"move": 
			Debug.Log( "move:"+x+ ","+y+", d:"+dx+","+dy ); 
			MousePos = new Vector3(x,y);
			break;
		}
	}
}
