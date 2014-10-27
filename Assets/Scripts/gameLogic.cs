using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameLogic : MonoBehaviour 
{
	public GameObject ChessPiecePrefab;
	public Board chessBoard;

	public GameObject[] charList1;
	public GameObject[] charList2;
	public GameObject charList1Anchor;
	public GameObject charList2Anchor;

	int playerTurn;
	public Player[] players;

	void Awake()
	{
		chessBoard.OnMoveFinish = OnChessMoveFinished;
		chessBoard.OnBoardInitialFinish = OnchessboardinitialFinish;
		chessBoard.OnPress = OnPress;
	}

	void OnPress(Board.coordinate co, GameObject[,] chessboard){

		for(int i=0; i<chessboard.GetLength(0); ++i)
		{
			for(int j=0; j<chessboard.GetLength(1); ++j)
			{
				chessboard[i,j].GetComponent<chessUI>().blocker.SetActive( false );
			}
		}

	}

	void Start()
	{
		players = new Player[2];
		players[0] = this.gameObject.AddComponent<Player>();
		players[1] = this.gameObject.AddComponent<Player>();

		players[0].playertype = playerType.PLAYER1;
		players[1].playertype = playerType.PLAYER2;
		
		players[0].initial1();
		players[1].initial2();

		charList1 = new GameObject[players[0].chars.Count];
		for( int i=0; i<players[0].chars.Count; ++i )
		{
			charList1[i] = GameObject.Instantiate( ChessPiecePrefab ) as GameObject;
			charList1[i].transform.parent = charList1Anchor.transform;
			charList1[i].transform.localScale = new Vector3( 0.5f, 0.5f, 0.5f );
			charList1[i].transform.localPosition = new Vector3( 80.0f*(i%6), -80.0f*(i/6), 0 );
			charList1[i].GetComponent<NPC>().initial( players[0].chars[i] );
			foreach( UISprite us in charList1[i].GetComponent<chessUI>().arrows )
			{
				us.gameObject.SetActive( false );
			}
			charList1[i].GetComponent<chessUI>().hpbar.SetActive( false );
		}

		charList2 = new GameObject[players[1].chars.Count];
		for( int i=0; i<players[1].chars.Count; ++i )
		{
			charList2[i] = GameObject.Instantiate( ChessPiecePrefab ) as GameObject;
			charList2[i].transform.parent = charList2Anchor.transform;
			charList2[i].transform.localScale = new Vector3( 0.5f, 0.5f, 0.5f );
			charList2[i].transform.localPosition = new Vector3( 80.0f*(i%6), -80.0f*(i/6), 0 );
			charList2[i].GetComponent<NPC>().initial( players[1].chars[i] );
			foreach( UISprite us in charList2[i].GetComponent<chessUI>().arrows )
			{
				us.gameObject.SetActive( false );
			}
			charList2[i].GetComponent<chessUI>().hpbar.SetActive( false );
		}
	}

	public Player CurrentPlayer
	{
		get{ return players[playerTurn]; }
	}

	public Player Enemy( Player p )
	{
		return players[ (p.playerID+1)%2 ];
	}

	public Player GetPlayer( playerType type )
	{
		if( type == playerType.NUTUAL )
			return null;
		return players[ (int)type ];
	}

	public Player GetPlayer(NPC npc)
	{
		if( npc.playertype == playerType.NUTUAL )
			return null;

		return players[ npc.playerID ];
	}

	void putChessOnBoard( NPC n, NPC.PropertyData ndata )
	{
		n.initial( ndata );
		GetPlayer(n).chars[n.pData.teamSequence].gamestate = NPC.GameState.INCOMBAT;
		if( GetPlayer(n).playerID == 0 )
			charList1[n.pData.teamSequence].GetComponent<chessUI>().combat.gameObject.SetActive( true );
		else
			charList2[n.pData.teamSequence].GetComponent<chessUI>().combat.gameObject.SetActive( true );
		n.OnNPCDead = OnNPCDead;
	}

	void OnchessboardinitialFinish( GameObject[,] chessBoard )
	{
		//initial chessboard;
		int i = 0; int j = 0;
		foreach( NPC.PropertyData ndata in players[0].chars.GetRange(0, 8) )
		{
			NPC n = chessBoard[i, j].GetComponent<NPC>();
			putChessOnBoard( n, ndata );
			++j;
			if( j == chessBoard.GetLength(1) )
			{
				++i;
				j = 0;
			}
		}

		i = 2; j = 0;
		foreach( NPC.PropertyData ndata in players[1].chars.GetRange(0, 8) )
		{
			NPC n = chessBoard[i, j].GetComponent<NPC>();
			putChessOnBoard( n, ndata );
			++j;
			if( j == chessBoard.GetLength(1) )
			{
				++i;
				j = 0;
			}
		}

		playerTurnStart(chessBoard);
	}

	void playerTurnStart( GameObject[,] chessboard )
	{
		playerType ptype = players[playerTurn].playertype;
		for(int i=0; i<chessboard.GetLength(0); ++i)
		{
			for(int j=0; j<chessboard.GetLength(1); ++j)
			{
				if(chessboard[i,j].GetComponent<NPC>().playertype == ptype)
				{
					chessboard[i,j].GetComponent<chessUI>().blocker.SetActive( false );
				}
				else
				{
					chessboard[i,j].GetComponent<chessUI>().blocker.SetActive( true );
				}
			}
		}
	}

	void OnChessMoveFinished( GameObject[,] chessboard, List<Board.coordinate> playhistory)
	{
		foreach(Board.coordinate co in playhistory)
		{
			print("----move history----");
			co.Log();
		}

		playerAttack( playerTurn, chessboard, chessBoard.CurrentMovingCO );

		playerTurn = ( playerTurn + 1 ) % 2;

		StartCoroutine( nextturn(chessboard) );

	}

	IEnumerator nextturn(GameObject[,] chessboard)
	{
		yield return new WaitForSeconds( 1.5f );
		playerTurnStart(chessboard);
	}

//	void npcAttack( NPC npc)
//	{
//		ButtonCallBack bcb = npc.gameObject.GetComponent<ButtonCallBack>();
//		foreach( NPC nnpc in GetAttackTargets(npc, bcb.id1, bcb.id2 ) )
//		{
//			nnpc.OnHit( npc);
//		}
//	}

	void playerAttack(int turn, GameObject[,] chessboard, Board.coordinate atkco)
	{
		GameObject go = chessBoard.GetChess(atkco);
		NPC npc = go.GetComponent<NPC>();

		Dictionary<string, object> Params = new Dictionary<string, object>();
		Params.Add("this", go);
		npc.pData.aggressiveSkill.Trigger(Params);

		GameObject[] ngos = chessBoard.GetNeighbors(atkco);
//		if(npc.pData.charType == NPC.CharType.STR)
//		{
		foreach(GameObject ngo in ngos)
		{
			if(ngo != null)
			{
				NPC nnpc = ngo.GetComponent<NPC>();
				if(!NPC.IsEnemy(npc, nnpc))
				{
					Dictionary<string, object> Params2 = new Dictionary<string, object>();
					Params2.Add("this", nnpc.gameObject);
					nnpc.pData.assistSkill.Trigger(Params2);
				}
			}
		}
			
//		}
//		else if(npc.pData.charType == NPC.CharType.AGI)
//		{
//			foreach(GameObject ngo in ngos)
//			{
//				if(ngo != null)
//				{
//					NPC atkedNPC = ngo.GetComponent<NPC>();
//					if(NPC.IsEnemy(npc, atkedNPC))
//					{
//						foreach(GameObject assistpos in chessBoard.GetNeighbors(ngo))
//						{
//							if(assistpos == null)
//								continue;
//
//							NPC assistNPC = assistpos.GetComponent<NPC>();
//							if(assistNPC != null && assistNPC != npc && NPC.IsEnemy(assistNPC, atkedNPC))
//							{
//								npcAttack(assistNPC);
//							}
//						}
//					}
//				}
//			}
//
//		}
//		else if(npc.pData.charType == NPC.CharType.WIS)
//		{
//		}
//		for(int i=0; i<chessboard.GetLength(0); ++i)
//		{
//			for(int j=0; j<chessboard.GetLength(1); ++j)
//			{
//				NPC npc = chessboard[i,j].GetComponent<NPC>();
//				if(npc.playerID == turn)
//				{
//					foreach( NPC nnpc in GetAttackTargets(npc, i, j ) )
//					{
//						nnpc.OnHit( npc);
//					}
//				}
//			}
//		}
	}

	void checkAttackTarget( ref List<NPC> result, int i, int j, NPC npc, bool bEnemy)
	{
		GameObject go = chessBoard.GetChess( new Board.coordinate(i, j) );
		if( go != null && NPC.IsEnemy( go.GetComponent<NPC>() , npc ) == bEnemy )
			result.Add( go.GetComponent<NPC>() );
	}

	List<NPC> GetTargets( GameObject go, NPC.atkType atktype, bool bEnemy )
	{
		Board.coordinate co = chessBoard.GetChessCo(gameObject);
		NPC npc = go.GetComponent<NPC>();
		int i = co.x;
		int j = co.y;
		List<NPC> targets = new List<NPC>();
		if( atktype == NPC.atkType.CROSS )
		{
			checkAttackTarget( ref targets, i+1, j, npc, bEnemy );
			checkAttackTarget( ref targets, i-1, j, npc, bEnemy );
			checkAttackTarget( ref targets, i, j+1, npc, bEnemy );
			checkAttackTarget( ref targets, i, j-1, npc, bEnemy );
		}
		else if( atktype == NPC.atkType.BEVEL )
		{
			checkAttackTarget( ref targets, i+1, j+1, npc, bEnemy );
			checkAttackTarget( ref targets, i-1, j-1, npc, bEnemy );
			checkAttackTarget( ref targets, i-1, j+1, npc, bEnemy );
			checkAttackTarget( ref targets, i+1, j-1, npc, bEnemy );

		}
		else if( atktype == NPC.atkType.LINE_MAX )
		{
			List<NPC>[] alldir = new List<NPC>[4];
			alldir[0] = new List<NPC>();
			alldir[1] = new List<NPC>();
			alldir[2] = new List<NPC>();
			alldir[3] = new List<NPC>();

			checkAttackTarget( ref alldir[0], i+1, j, npc, bEnemy );
			checkAttackTarget( ref alldir[0], i+2, j, npc, bEnemy );
			checkAttackTarget( ref alldir[0], i+3, j, npc, bEnemy ); 

			checkAttackTarget( ref alldir[1], i-1, j, npc, bEnemy );
			checkAttackTarget( ref alldir[1], i-2, j, npc, bEnemy );
			checkAttackTarget( ref alldir[1], i-3, j, npc, bEnemy ); 

			checkAttackTarget( ref alldir[2], i, j+1, npc, bEnemy );
			checkAttackTarget( ref alldir[2], i, j+2, npc, bEnemy );
			checkAttackTarget( ref alldir[2], i, j+3, npc, bEnemy ); 

			checkAttackTarget( ref alldir[3], i, j-1, npc, bEnemy );
			checkAttackTarget( ref alldir[3], i, j-2, npc, bEnemy );
			checkAttackTarget( ref alldir[3], i, j-3, npc, bEnemy ); 

			int idx = 0;
			int count = 0;
			for(int m=0; m < 4; ++m)
			{
				if(count >= alldir[m].Count)
				{
					count = alldir[m].Count;
					idx = m;
				}
			}
			return alldir[idx];

		}
		else if( atktype == NPC.atkType.CROSS_LINE)
		{
			checkAttackTarget( ref targets, i+1, j, npc, bEnemy );
			checkAttackTarget( ref targets, i+2, j, npc, bEnemy );
			checkAttackTarget( ref targets, i+3, j, npc, bEnemy ); 
			checkAttackTarget( ref targets, i-1, j, npc, bEnemy );
			checkAttackTarget( ref targets, i-2, j, npc, bEnemy );
			checkAttackTarget( ref targets, i-3, j, npc, bEnemy );

			checkAttackTarget( ref targets, i, j+1, npc, bEnemy );
			checkAttackTarget( ref targets, i, j+2, npc, bEnemy );
			checkAttackTarget( ref targets, i, j+3, npc, bEnemy ); 
			checkAttackTarget( ref targets, i, j-1, npc, bEnemy );
			checkAttackTarget( ref targets, i, j-2, npc, bEnemy );
			checkAttackTarget( ref targets, i, j-3, npc, bEnemy ); 
		}
		return targets;
	}

	void playerBuff()
	{

	}

	void OnNPCDead( NPC npc )
	{
		StartCoroutine( _OnNPCDead(npc) );
	}

	IEnumerator _OnNPCDead( NPC npc )
	{
		if(npc.gamestate == NPC.GameState.DEFEAT)
			yield break;
		npc.gamestate = NPC.GameState.DEFEAT;
		GetPlayer(npc).chars[ npc.pData.teamSequence ].gamestate = NPC.GameState.DEFEAT;
		print( npc.name + "is dead" );

		if( GetPlayer(npc).playerID == 0 )
		{
			charList1[npc.pData.teamSequence].GetComponent<chessUI>().combat.gameObject.SetActive( false );
			charList1[npc.pData.teamSequence].GetComponent<chessUI>().Dead.gameObject.SetActive( true );
		}
		else if( GetPlayer(npc).playerID == 1 )
		{
			charList2[npc.pData.teamSequence].GetComponent<chessUI>().combat.gameObject.SetActive( false );
			charList2[npc.pData.teamSequence].GetComponent<chessUI>().Dead.gameObject.SetActive( true );
		}

		yield return StartCoroutine( npc._playDeath() );

		Player p = GetPlayer(npc);
		if(p == null)
			yield break;

		NPC.PropertyData data = null;
		for( int i=0; i< p.chars.Count; ++i )
		{
			if( p.chars[i].gamestate == NPC.GameState.WAITING )
			{
				data = p.chars[i];
				p.chars[i].gamestate = NPC.GameState.INCOMBAT;
				break;
			}
		}

		p = Enemy( p );
		if( data == null )
		{
			for( int i=0; i< p.chars.Count; ++i )
			{
				if( p.chars[i].gamestate == NPC.GameState.WAITING )
				{
					data = p.chars[i];
					p.chars[i].gamestate = NPC.GameState.INCOMBAT;
					break;
				}
			}
		}



		if(data == null)
		{
			npc.gameObject.GetComponent<chessUI>().NPC.gameObject.SetActive( false );
			npc.playertype = playerType.NUTUAL;
			npc.pData.atk = 0;
			npc.OnNPCDead = null;
			npc.hpbar.gameObject.SetActive( false );
		}
		else
		{
			putChessOnBoard( npc, data );
		}
		StartCoroutine( npc._playInitial() );


//		print("player1");
//		for( int i=0; i< GetPlayer(playerType.PLAYER1).chars.Count; ++i )
//		{
//			print(i.ToString() + ":" + GetPlayer(playerType.PLAYER1).chars[i].gamestate.ToString() );
//		}
//		
//		print("player2");
//		for( int i=0; i< GetPlayer(playerType.PLAYER2).chars.Count; ++i )
//		{
//			print(i.ToString() + ":" + GetPlayer(playerType.PLAYER2).chars[i].gamestate.ToString() );
//		}

		if(GetPlayer(npc) != null && playerLose(GetPlayer(npc)))
		{
			OnGameEnd(Enemy(GetPlayer(npc)), GetPlayer(npc));
		}
	}

	void OnGameEnd(Player winner, Player loser)
	{
		Debug.LogError( winner.playertype.ToString() + "WIN !!!!!" );
	}

	bool playerLose( Player p )
	{
		foreach( NPC.PropertyData prop in p.chars )
		{
			if(prop.gamestate != NPC.GameState.DEFEAT)
				return false;
		}
		return true;
	}	

	void OnGUI()
	{
		if(GUILayout.Button("restart"))
		{
			Application.LoadLevel(0);
		}
	}

	//temp skill
	public void atkSkill(Dictionary<string, object> Params)
	{
		GameObject target = (GameObject)Params["target"];
		NPC.atkType atktype = (NPC.atkType)Params["atktype"];
		foreach(NPC npc in GetAttackTargets( target, atktype ))
		{
			npc.OnHit( target.GetComponent<NPC>(), (int)Params["damage"] );
		}
	}
	
}
