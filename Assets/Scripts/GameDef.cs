using UnityEngine;
using System.Collections;

public enum playerType{ PLAYER1 = 0, PLAYER2 = 1, NUTUAL = 2 };

public interface IPlayerType
{
	playerType 		playertype 		{ get; set; }
	int   			playerID   		{ get; set; }
}