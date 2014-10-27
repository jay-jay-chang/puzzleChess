using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IPlayerType {

	playerType _playertype = playerType.NUTUAL;

	public playerType playertype
	{
		get{ return _playertype; }
		set{ _playertype = value; }
	}

	public int playerID
	{
		get{ return (int)_playertype; }
		set{ _playertype = (playerType)value; }
	}



	public List<NPC.PropertyData> chars = new List<NPC.PropertyData>();

	public void initial1()
	{
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1307a", 40, 70, 70, NPC.atkType.CROSS, NPC.CharType.AGI ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1307a", 40, 70, 70, NPC.atkType.CROSS, NPC.CharType.AGI ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1303a", 20, 100, 100, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "mob0003a", 5, 200, 200, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "mob0003a", 5, 200, 200, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1303a", 20, 100, 100, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1307a", 40, 70, 70, NPC.atkType.CROSS, NPC.CharType.AGI ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "m1303a", 20, 100, 100, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER1, "mob0003a", 5, 200, 200, NPC.atkType.CROSS, NPC.CharType.STR ) );

		for(int i=0; i<chars.Count; ++i)
		{
			chars[i].teamSequence = i;
		}
	}

	public void initial2()
	{
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "mob0003a", 5, 200, 200, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1307a", 40, 70, 70, NPC.atkType.CROSS, NPC.CharType.AGI ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1303a", 20, 100, 100, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "mob0003a", 5, 200, 200, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "mob0003a", 5, 200, 200, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1303a", 20, 100, 100, NPC.atkType.CROSS, NPC.CharType.STR ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1307a", 40, 70, 70, NPC.atkType.CROSS, NPC.CharType.AGI ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1103a", 30, 80, 80, NPC.atkType.CROSS_LINE, NPC.CharType.WIS ) );
		chars.Add( new NPC.PropertyData( playerType.PLAYER2, "m1307a", 40, 70, 70, NPC.atkType.CROSS, NPC.CharType.AGI ) );

		for(int i=0; i<chars.Count; ++i)
		{
			chars[i].teamSequence = i;
		}
	}
}
