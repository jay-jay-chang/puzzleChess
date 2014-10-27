using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour 
{
	public GameObject ChessPiecePrefab;
	public GameObject CountDownBarPrefab;
	public GameObject countDownBar;
	public static int weight = 4;
	public static int height = 4;
	public static Vector2 ChessPieceScale = new Vector2(150.0f, 150.0f);
	public int resolution_width;
	public int resolution_height;
	public float SqrSwapDistance = 3600.0f;

	GameObject[,] chess;

	coordinate currentMoving;

	List<coordinate> playHistory;

	public System.Action < GameObject[,], List<coordinate> > OnMoveFinish;
	public System.Action < GameObject[,] > OnBoardInitialFinish;
	public System.Action < coordinate, GameObject[,] > OnPress;

	public class coordinate
	{
		public int x;
		public int y;

		public coordinate(int X, int Y)
		{
			x = X;
			y = Y;
		}

		public void Log()
		{
			MonoBehaviour.print("x: " + x.ToString() + ", y: " + y.ToString());
		}

		public bool equal( coordinate a )
		{
			return this.x == a.x && this.y == a.y;
		}
	}

	// Use this for initialization
	void Start () 
	{
		chess = new GameObject[weight, height];
		for(int j=0; j<height; ++j)
		{
			for(int i=0; i<weight; ++i)
			{
				chess[i,j] = GameObject.Instantiate( ChessPiecePrefab ) as GameObject;
				chess[i,j].transform.parent = this.gameObject.transform;
				chess[i,j].transform.localPosition = new Vector3( i*ChessPieceScale.x - weight*ChessPieceScale.x*0.5f + ChessPieceScale.x*0.5f, 
				                                                 height*ChessPieceScale.y*0.5f - j*ChessPieceScale.y - ChessPieceScale.y*0.5f,
				                                                  0 );
				chess[i,j].transform.localScale = new Vector3(1,1,1);
				setChessInfo( new coordinate( i, j ) );
			}
		}

		if( OnBoardInitialFinish != null )
		{
			OnBoardInitialFinish( chess );
		}

		countDownBar = GameObject.Instantiate( CountDownBarPrefab ) as GameObject;
		countDownBar.SetActive( false );
	}

	public GameObject GetChess(coordinate co){
		if(co.x < 0 || co.x >= weight )
			return null;

		if(co.y < 0 || co.y >= height)
			return null;

		return chess[co.x, co.y];
	}

	public coordinate GetChessCo(GameObject chess)
	{
		ButtonCallBack bcb = chess.GetComponent<ButtonCallBack>();
		return new coordinate( bcb.id1, bcb.id2 );
	}

	public GameObject[] GetNeighbors(GameObject chess)
	{
		return GetNeighbors(GetChessCo(chess));
	}

	public GameObject[] GetNeighbors(coordinate co)
	{
		GameObject[] ns = new GameObject[4];
		//0:up, 1:right, 2:down, 3:left
		ns[0] = GetChess(new coordinate(co.x, co.y+1));
		ns[1] = GetChess(new coordinate(co.x+1, co.y));
		ns[2] = GetChess(new coordinate(co.x, co.y-1));
		ns[3] = GetChess(new coordinate(co.x-1, co.y));

		return ns;
	}

	public coordinate CurrentMovingCO
	{
		get{ return currentMoving; }
	}

	public GameObject CurrentMovingObject()
	{
		return (currentMoving != null)? chess[ currentMoving.x, currentMoving.y ] : null;
	}

	void setChessInfo(coordinate co)
	{
		foreach(ButtonCallBack cb in chess[co.x, co.y].GetComponents<ButtonCallBack>())
		{
			cb.id1 = co.x;
			cb.id2 = co.y;
			cb.CallBack = OnchessTouched;
		}
	}

	void setPositionByID(coordinate co)
	{
		chess[co.x, co.y].transform.localPosition = GetPositionByID( co );
	}

	Vector3 GetPositionByID(coordinate co)
	{
		return new Vector3( co.x*ChessPieceScale.x - weight*ChessPieceScale.x*0.5f + ChessPieceScale.x*0.5f, 
		                   height*ChessPieceScale.y*0.5f - co.y*ChessPieceScale.y - ChessPieceScale.y*0.5f,
		                   0 );
	}

	IEnumerator PressCountDown(float secs)
	{
		countDownBar.transform.parent = CurrentMovingObject().transform;
		countDownBar.transform.localPosition = new Vector3(0, 80, 0);
		countDownBar.transform.localScale = new Vector3(2,2,1);
		countDownBar.SetActive(true);
		countDownBar.GetComponent<countDownBar>().Run(secs);

		yield return new WaitForSeconds(secs);
		GameObject go = CurrentMovingObject();
		if(go != null)
		{
			countDownBar.SetActive(false);
			go.SendMessage("OnPress", false);
		}
	}
	
	void OnchessTouched( ButtonCallBack bcb, ButtonCallBack.Trigger trigger)
	{
		//print(bcb.id1 + ", " + bcb.id2 + " : " + trigger.ToString());

		coordinate co = new coordinate(bcb.id1, bcb.id2);

		if(trigger == ButtonCallBack.Trigger.OnPress)
		{
			currentMoving = co;
			if(playHistory != null)
			{
				playHistory.Clear();
				playHistory = null;
			}

			playHistory = new List<coordinate>();
			playHistory.Add(co);

			if( OnPress != null )
			{
				OnPress( co, chess );
			}
			StartCoroutine(PressCountDown(2.0f));
		}
		else if(trigger == ButtonCallBack.Trigger.OnRelease)
		{
			if(currentMoving != null)
			{
				setPositionByID( currentMoving );

				if(OnMoveFinish != null)
				{
					OnMoveFinish( chess, playHistory );
				}

				currentMoving = null;
			}
		}
	}

	coordinate getNearestchess( GameObject currentMoving )
	{
		float x = currentMoving.transform.localPosition.x + weight*ChessPieceScale.x*0.5f;
		float y = currentMoving.transform.localPosition.y -  height*ChessPieceScale.y*0.5f;

		if(x < 0.0f) x = 0.0f;
		if(y > 0.0f) y = 0.0f;

		return new coordinate( (int)( x / ChessPieceScale.x ), (int)( y / -ChessPieceScale.y ) );
	}

	void swapID(coordinate c1, coordinate c2)
	{
		GameObject temp = chess[ c1.x, c1.y ];
		chess[ c1.x, c1.y ] = chess[ c2.x, c2.y ];
		chess[ c2.x, c2.y ] = temp;
		setChessInfo(c1);
		setChessInfo(c2);
	}

	void AdjustPosAfterSwap(coordinate c1, coordinate c2)
	{
		if( currentMoving.equal(c2) )
			setPositionByID(c2);
		else
			setPositionByID(c1);
	}

	// Update is called once per frame
	void Update () 
	{
		if(currentMoving != null)
		{
			GameObject currentmovingOb = CurrentMovingObject();
			coordinate co = getNearestchess( currentmovingOb );
			if( !co.equal(currentMoving) )
			{
				float dis = ( GetPositionByID(co) - CurrentMovingObject().transform.localPosition ).sqrMagnitude;
				if( dis < SqrSwapDistance )
				{
					playHistory.Add(co);
					swapID( currentMoving, co );
					AdjustPosAfterSwap( currentMoving, co );
					currentMoving = co;
				}
			}
		}
	}
}
