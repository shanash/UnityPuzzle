using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMap : UnityBehaviour {

	GameObject[,] _arrBlock;

	const float dropTime = 2.0f;
	float _curDropTime = 2.0f;

	public struct BlockPos {
		public BlockPos( int floor, int row ) {
			Floor = floor;
			Row = row;
		}
		public int Floor;
		public int Row;
	}

	List<BlockPos> _listBlankPos = new List<BlockPos>();

	const float brakeTime = 1.0f;
	float _curBrakeTime = 1.0f;

	public override void OnAwake () {
		_arrBlock = new GameObject[12, 8];
		GameObject prefabBlock = Resources.Load("Prefabs/Block") as GameObject;

		for (int i = 0; i < 12; i++) {
			for ( int j = 0; j < 8; j++ ) {
				CreateBlock(i,j);
			}
		}
	}

	void CreateBlock(int floor, int row) {
		GameObject prefabBlock = Resources.Load ("Prefabs/Block") as GameObject;

		_arrBlock[floor,row] = Instantiate(prefabBlock) as GameObject;
		_arrBlock[floor,row].transform.parent = transform;
		_arrBlock[floor,row].transform.localScale = new Vector3(1,1);
		_arrBlock[floor,row].transform.localPosition = new Vector3( 40*row - 40*4, 40*floor - 40*6);
		_arrBlock[floor,row].GetComponent<Block>().Floor = floor;
		_arrBlock[floor,row].GetComponent<Block>().Row = row;
		int type = Random.Range(0,3);
		int dontThisTypeRow = -1;
		int dontThisTypeFloor = -1;

		if ( row > 1 ) {
			if ( _arrBlock[floor,row-1] != null && _arrBlock[floor,row-1].GetComponent<Block>().Type == type ) {
				if ( _arrBlock[floor,row-2] != null && _arrBlock[floor,row-2].GetComponent<Block>().Type == type ) {
					dontThisTypeRow = type;
					while ( dontThisTypeRow == type ) {
						type = Random.Range(0,3);
					}
				}
			}
		}

		if ( floor > 1 ) {
			if ( _arrBlock[floor-1,row] != null && _arrBlock[floor-1,row].GetComponent<Block>().Type == type ) {
				if ( _arrBlock[floor-2,row] != null && _arrBlock[floor-2,row].GetComponent<Block>().Type == type ) {
					dontThisTypeFloor = type;
					while ( dontThisTypeFloor == type || dontThisTypeRow == type ) {
						type = Random.Range(0,3);
					}
				}
			}
		}
		
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
		if (_curDropTime < dropTime) {
			_curDropTime += Time.deltaTime;
			if ( _curDropTime > dropTime ) {
				foreach ( BlockPos pos in _listBlankPos ) {
					DropLine(pos.Floor, pos.Row);
				}
				_listBlankPos.Clear();

			}
		}

		BrakeBlock();
	}

	public void Delete( int floor, int row ) {
		if (_curDropTime < dropTime) {
			_curDropTime = dropTime;
			foreach ( BlockPos pos in _listBlankPos ) {
				DropLine(pos.Floor, pos.Row);
			}
			_listBlankPos.Clear();
		}
		Destroy (_arrBlock [floor, row]);
		_arrBlock [floor, row] = null;
		WaitForDrop (floor, row);

	}

	public void Delete( List<BlockPos> listPos ) {
		if (_curDropTime < dropTime) {
			_curDropTime = dropTime;
			foreach ( BlockPos pos in _listBlankPos ) {
				DropLine(pos.Floor, pos.Row);
			}
			_listBlankPos.Clear();
			//BrakeBlock();
		}

		foreach ( BlockPos pos in listPos ) {
			Destroy (_arrBlock [pos.Floor, pos.Row]);
			_arrBlock [pos.Floor, pos.Row] = null;
		}
		WaitForDrop (listPos);
	}

	public void WaitForDrop( int floor, int row ) {
		_curDropTime = 0.0f;
		_listBlankPos.Clear();
		_listBlankPos.Add ( new BlockPos(floor,row) );
	}

	public void WaitForDrop( List<BlockPos> listPos ) {
		_curDropTime = 0.0f;
		_listBlankPos.Clear();
		foreach ( BlockPos pos in listPos ) {
			_listBlankPos.Add ( pos );
		}
	}

	public void DropLine( int floor, int row ) {
		for( int i = floor+1; i < 12; i++ ) {
			Drop( i, row );
		}

		CreateBlock(11,row);


	}

	private void BrakeBlock() {
		ArrayList list = new ArrayList();

		//floor chk
		for (int i = 0; i < 12; i++) {
			int sequenceType = 0;
			int sequenceCount = 0;
			for ( int j = 0; j < 8; j++ ) {
				int type = 0;
				if ( _arrBlock[i, j] != null ) {
					type = _arrBlock[i, j].GetComponent<Block>().Type;
				}

				if ( type != sequenceType ) {
					if ( sequenceCount >= 3 ) {
						List<BlockPos> listPos = new List<BlockPos>();
						for ( int k = 1; k <= sequenceCount; k++ ) {
							Debug.Log( "Floor : " + i + ", Row : " + (j-k).ToString() );
							listPos.Add ( new BlockPos(i, j-k) );
						}

						Delete ( listPos );
					}
					sequenceType = type;
					sequenceCount = 0;
				}

				if ( type == 0 ) {
					sequenceCount = 0;
				}

				sequenceCount++;
			}
		}
	}

	public void Drop( int floor, int row ) {
		if (floor == 0)	return;
		if (_arrBlock [floor-1, row] != null)	return;
		Move (MOVE_DIR.MOVE_DOWN, floor, row);
	}

	public void MoveLeft( int floor, int row ) {
		if (row == 0) return;
		if (_arrBlock [floor, row - 1] != null)	return;
		Move (MOVE_DIR.MOVE_LEFT, floor, row);
	}

	public void MoveRight( int floor, int row ) {
		if (row == 7) return;
		if (_arrBlock [floor, row + 1] != null)	return;
		Move (MOVE_DIR.MOVE_RIGHT, floor, row);
	}

	public enum MOVE_DIR{
		MOVE_DOWN,
		MOVE_RIGHT,
		MOVE_LEFT,
	};

	private void Move( MOVE_DIR dir, int floor, int row ) {
		int dirFloor = 0;
		int dirRow = 0;
		switch (dir) {
		case MOVE_DIR.MOVE_DOWN:
			dirFloor = -1;
			break;
		case MOVE_DIR.MOVE_LEFT:
			dirRow = -1;
			break;
		case MOVE_DIR.MOVE_RIGHT:
			dirRow = 1;
			break;
		}

		_arrBlock [floor+dirFloor,row+dirRow] = _arrBlock[floor,row];
		_arrBlock [floor, row] = null;
		_arrBlock [floor+dirFloor, row+dirRow].GetComponent<Block> ().Floor = floor+dirFloor;
		_arrBlock [floor+dirFloor, row+dirRow].GetComponent<Block> ().Row = row+dirRow;
		_arrBlock [floor+dirFloor, row+dirRow].transform.localPosition = new Vector3 (40 * (row+dirRow) - 40*4, 40 * (floor +dirFloor) - 40*6);

		if (dir == MOVE_DIR.MOVE_LEFT) {
			DropLine( floor, row );
		}
		else if ( dir == MOVE_DIR.MOVE_RIGHT ) {
			DropLine( floor, row );
		}
	}
}
