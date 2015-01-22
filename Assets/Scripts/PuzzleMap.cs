using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct BlockPos {
	public BlockPos( int floor, int row ) {
		Floor = floor;
		Row = row;
	}
	public int Floor;
	public int Row;
}

public class PuzzleMap : UnityBehaviour {

	public UILabel ComboLabel;

	private const int MAXFLOOR_NUM = 12;
	private const int FLOOR_NUM = 5;
	private const int ROW_NUM = 8;

	private GameObject[,] _arrBlock;

	private const float dropTime = 2.0f;
	private float _curDropTime = 2.0f;

	private int _comboCount = 0;

	public override void OnAwake () {
		_arrBlock = new GameObject[MAXFLOOR_NUM, ROW_NUM];

		for (int i = 0; i < MAXFLOOR_NUM; i++) {
			for ( int j = 0; j < ROW_NUM; j++ ) {
				CreateBlock(i,j);
			}
		}

	}

	private void CreateBlock(int floor, int row) {
		GameObject prefabBlock = Resources.Load ("Prefabs/Block") as GameObject;

		_arrBlock[floor,row] = Instantiate(prefabBlock) as GameObject;
		_arrBlock[floor,row].transform.parent = transform;
		_arrBlock[floor,row].transform.localScale = new Vector3(1,1);
		_arrBlock[floor,row].transform.localPosition = new Vector3( 40*row -80, 40*floor -110 );
		_arrBlock[floor,row].GetComponent<Block>().Floor = floor;
		_arrBlock[floor,row].GetComponent<Block>().Row = row;
		int type = Random.Range(0,3);

		_arrBlock[floor,row].GetComponent<Block>().Type = type;
		
		if ( type == 1 ) _arrBlock[floor,row].GetComponent<UIButton>().defaultColor = new Color( 0, 1, 0 );
		else if ( type == 2 ) _arrBlock[floor,row].GetComponent<UIButton>().defaultColor = new Color( 0, 0, 1 );
	}

	// Use this for initialization
	public override void OnStart () {
		Debug.Log (_arrBlock [0, 0]);
	}
	
	// Update is called once per frame
	public override void OnUpdate () {
//		Debug.Log( _curDropTime );
		if (_curDropTime < dropTime) {
			_curDropTime += Time.deltaTime;
			if ( _curDropTime > dropTime ) {
				DropLine ();
			}
		}
		else {
			BrakeBlock();
			if ( _curDropTime > dropTime ) {
				_comboCount = 0;
				ComboLabel.text = "";
			}
			else {
				_comboCount++;
				Debug.Log( "COMBO : " + _comboCount );
				ComboLabel.text = _comboCount + " COMBO";
			}
		}
//		Debug.Log( "X : " + _arrBlock[0,0].transform.localPosition.x +", Y : " +  _arrBlock[0,0].transform.localPosition.y );
	}

	public void Delete( int floor, int row ) {
		if (_curDropTime < dropTime) {
			_curDropTime = dropTime;
			DropLine();
		}

		Destroy (_arrBlock [floor, row]);
		_arrBlock [floor, row] = null;
		WaitForDrop (floor, row);

	}

	public void Delete( List<BlockPos> listPos ) {

		if (_curDropTime < dropTime) {
			_curDropTime = dropTime;
			DropLine();
		}

		foreach ( BlockPos pos in listPos ) {
			if ( _arrBlock[pos.Floor, pos.Row] == null ) continue;
			Destroy (_arrBlock [pos.Floor, pos.Row]);
			_arrBlock [pos.Floor, pos.Row] = null;
		}
		WaitForDrop (listPos);
	}

	public void WaitForDrop( int floor, int row ) {
		_curDropTime = 0.0f;
	}

	public void WaitForDrop( List<BlockPos> listPos ) {
		_curDropTime = 0.0f;
	}

	public void DropLine() {
		for ( int i = 0; i < ROW_NUM; i++ ) {
			int blankNum = 0;
			for ( int j = 0; j < MAXFLOOR_NUM; j++ ) {
				if ( _arrBlock[j,i] == null ) {
					blankNum++;
				}
				else {
					Move ( new BlockPos(j,i), new BlockPos(-blankNum,0) );
				}
			}
			
			for ( int j = MAXFLOOR_NUM-1; j >= 0; j-- ) {
				if ( _arrBlock[j,i] != null ) break;
				CreateBlock(j,i);
			}
		}
	}

	private void BrakeBlock() {
		List<BlockPos> listPos = new List<BlockPos>();

		//floor chk
		for (int i = 0; i < FLOOR_NUM; i++) {
			int sequenceType = 0;
			int sequenceCount = 0;
			for ( int j = 0; j < ROW_NUM; j++ ) {
				int type = 0;
				if ( _arrBlock[i, j] != null ) {
					type = _arrBlock[i, j].GetComponent<Block>().Type;
				}


				if ( type == sequenceType ) {
					sequenceCount++;
					if (type == 0 ) sequenceCount = 0;

					if ( sequenceCount == 3 ) {

						listPos.Add ( new BlockPos(i, j-2) );
						listPos.Add ( new BlockPos(i, j-1) );
						listPos.Add ( new BlockPos(i, j) );
						Debug.Log( "Floor : " + i + ", Row : " + (j-2).ToString() );
						Debug.Log( "Floor : " + i + ", Row : " + (j-1).ToString() );
						Debug.Log( "Floor : " + i + ", Row : " + (j).ToString() );
					}
					else if ( sequenceCount > 3 ) {
						listPos.Add ( new BlockPos(i, j) );
						Debug.Log( "Floor : " + i + ", Row : " + (j).ToString() );
					}
				}
				else {
					sequenceType = type;
					sequenceCount = 1;
				}

			}
		}

		//row check
		for ( int j = 0; j < ROW_NUM; j++ ) {
			int sequenceType = 0;
			int sequenceCount = 0;
			for ( int i = 0; i < FLOOR_NUM; i++ ) {
				int type = 0;
				if ( _arrBlock[i, j] != null ) {
					type = _arrBlock[i, j].GetComponent<Block>().Type;
				}
				
				
				if ( type == sequenceType ) {
					sequenceCount++;
					if (type == 0 ) sequenceCount = 0;
					
					if ( sequenceCount == 3 ) {
						
						listPos.Add ( new BlockPos(i-2, j) );
						listPos.Add ( new BlockPos(i-1, j) );
						listPos.Add ( new BlockPos(i, j) );
						Debug.Log( "Floor : " + i + ", Row : " + (j-2).ToString() );
						Debug.Log( "Floor : " + i + ", Row : " + (j-1).ToString() );
						Debug.Log( "Floor : " + i + ", Row : " + (j).ToString() );
					}
					else if ( sequenceCount > 3 ) {
						listPos.Add ( new BlockPos(i, j) );
						Debug.Log( "Floor : " + i + ", Row : " + (j).ToString() );
					}
				}
				else {
					sequenceType = type;
					sequenceCount = 1;
				}
				
			}
		}

		if ( listPos.Count > 0 ) Delete ( listPos );
	}

	public void Drop( int floor, int row ) {
		Move(new BlockPos(floor,row), new BlockPos(-1,0) );
	}

	public void MoveLeft( int floor, int row ) {
		Move(new BlockPos(floor,row), new BlockPos(0,-1) );
		DropLine();
		BrakeBlock();
	}

	public void MoveRight( int floor, int row ) {
		Move(new BlockPos(floor,row), new BlockPos(0,1) );
		DropLine();
		BrakeBlock();
	}

	private void Move( BlockPos pos, BlockPos dir ) {
		if ( pos.Floor + dir.Floor < 0 || pos.Floor + dir.Floor >= MAXFLOOR_NUM ) return;
		if ( pos.Row + dir.Row < 0 || pos.Row + dir.Row >= ROW_NUM  ) return;
		if ( _arrBlock[pos.Floor+dir.Floor, pos.Row+dir.Row] != null ) return;

		_arrBlock [pos.Floor+dir.Floor,pos.Row+dir.Row] = _arrBlock[pos.Floor, pos.Row];
		_arrBlock [pos.Floor, pos.Row] = null;
		_arrBlock [pos.Floor+dir.Floor, pos.Row+dir.Row].GetComponent<Block> ().Floor = pos.Floor+dir.Floor;
		_arrBlock [pos.Floor+dir.Floor, pos.Row+dir.Row].GetComponent<Block> ().Row = pos.Row+dir.Row;
		_arrBlock [pos.Floor+dir.Floor, pos.Row+dir.Row].transform.localPosition = new Vector3 (40 * (pos.Row+dir.Row) - 80, 40 * (pos.Floor+dir.Floor) - 110);

	}
}
