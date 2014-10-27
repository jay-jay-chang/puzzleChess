using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IPlayerType {

	public enum atkType{ NONE, CROSS, BEVEL, LINE_MAX, CROSS_LINE, SURROUND, IN_RANG_3 };
	public enum GameState{ WAITING, INCOMBAT, DEFEAT };
	public enum CharType{ STR, AGI, WIS };

	public GameObject OnHitNumberFX;

	public UISlider hpbar;

	public PropertyData pData;
	public class PropertyData
	{
		public PropertyData(playerType playertype, string textureName, int atk, int hp, int maxhp, atkType atktype, CharType charType)
		{
			this.playertype = playertype;
			this.textureName = textureName;
			this.atk = atk;
			this.hp = hp;
			this.maxhp = maxhp;
			this.atktype = atktype;
			this.charType = charType;
		}

		public int teamSequence;
		public playerType playertype = playerType.NUTUAL;
		public string textureName = "";
		public int atk;
		public int hp;
		public int maxhp;
		public atkType atktype = atkType.CROSS;
		public GameState gamestate = GameState.WAITING;
		public CharType charType = CharType.STR;
		public Skill aggressiveSkill;
		public Skill assistSkill;
	}

	public int playerID
	{
		get{ return (int)pData.playertype; }
		set{ playertype = (playerType)value; }
	}

	public playerType playertype
	{
		get{ return pData.playertype; }
		set
		{
			pData.playertype = value;
			if(pData.playertype == playerType.PLAYER1)
				this.gameObject.GetComponent<chessUI>().chessBK.spriteName = "ui_kk_armor_icon_a01_r";
			else if(pData.playertype == playerType.PLAYER2)
				this.gameObject.GetComponent<chessUI>().chessBK.spriteName = "ui_kk_armor_icon_a01_s";
			else
				this.gameObject.GetComponent<chessUI>().chessBK.spriteName = "ui_kk_armor_icon_a01";
		}
	}

	public string textureName
	{
		get{ return pData.textureName; }
		set
		{
			pData.textureName = value;
			this.gameObject.GetComponent<chessUI>().NPC.spriteName = pData.textureName;
		}
	}

	public int hp
	{
		get{ return pData.hp; }
		set
		{
			pData.hp = value;
			if(pData.hp < 0)
				pData.hp = 0;
			hpbar.sliderValue = (float)pData.hp/pData.maxhp;
		}
	}


	
	public atkType atktype
	{
		get{ return pData.atktype; }
		set
		{
			chessUI ui = this.gameObject.GetComponent<chessUI>();
			pData.atktype = value;

//			ui.arrows[0].gameObject.SetActive( true );
//			ui.arrows[1].gameObject.SetActive( true );
//			ui.arrows[2].gameObject.SetActive( true );
//			ui.arrows[3].gameObject.SetActive( true );
//			ui.arrows[4].gameObject.SetActive( true );
//			ui.arrows[5].gameObject.SetActive( true );
//			ui.arrows[6].gameObject.SetActive( true );
//			ui.arrows[7].gameObject.SetActive( true );
//			
//			switch(pData.atktype)
//			{
//			case atkType.CROSS:
//				ui.arrows[1].gameObject.SetActive( false );
//				ui.arrows[3].gameObject.SetActive( false );
//				ui.arrows[5].gameObject.SetActive( false );
//				ui.arrows[7].gameObject.SetActive( false );
//				break;
//
//			case atkType.BEVEL:
//				ui.arrows[0].gameObject.SetActive( false );
//				ui.arrows[2].gameObject.SetActive( false );
//				ui.arrows[4].gameObject.SetActive( false );
//				ui.arrows[6].gameObject.SetActive( false );
//				break;
//
//			case atkType.LINE_UP:
//				//ui.arrows[0].gameObject.SetActive( false );
//				ui.arrows[1].gameObject.SetActive( false );
//				ui.arrows[2].gameObject.SetActive( false );
//				ui.arrows[3].gameObject.SetActive( false );
//				ui.arrows[4].gameObject.SetActive( false );
//				ui.arrows[5].gameObject.SetActive( false );
//				ui.arrows[6].gameObject.SetActive( false );
//				ui.arrows[7].gameObject.SetActive( false );
//				break;
//
//			case atkType.LINE_RIGHT:
//				ui.arrows[0].gameObject.SetActive( false );
//				ui.arrows[1].gameObject.SetActive( false );
//				//ui.arrows[2].gameObject.SetActive( false );
//				ui.arrows[3].gameObject.SetActive( false );
//				ui.arrows[4].gameObject.SetActive( false );
//				ui.arrows[5].gameObject.SetActive( false );
//				ui.arrows[6].gameObject.SetActive( false );
//				ui.arrows[7].gameObject.SetActive( false );
//				break;
//
//			case atkType.LINE_LEFT:
//				ui.arrows[0].gameObject.SetActive( false );
//				ui.arrows[1].gameObject.SetActive( false );
//				ui.arrows[2].gameObject.SetActive( false );
//				ui.arrows[3].gameObject.SetActive( false );
//				ui.arrows[4].gameObject.SetActive( false );
//				ui.arrows[5].gameObject.SetActive( false );
//				//ui.arrows[6].gameObject.SetActive( false );
//				ui.arrows[7].gameObject.SetActive( false );
//				break;
//
//			case atkType.LINE_DOWN:
//				ui.arrows[0].gameObject.SetActive( false );
//				ui.arrows[1].gameObject.SetActive( false );
//				ui.arrows[2].gameObject.SetActive( false );
//				ui.arrows[3].gameObject.SetActive( false );
//				//ui.arrows[4].gameObject.SetActive( false );
//				ui.arrows[5].gameObject.SetActive( false );
//				ui.arrows[6].gameObject.SetActive( false );
//				ui.arrows[7].gameObject.SetActive( false );
//				break;
//			case atkType.NONE:
//				ui.arrows[0].gameObject.SetActive( false );
//				ui.arrows[1].gameObject.SetActive( false );
//				ui.arrows[2].gameObject.SetActive( false );
//				ui.arrows[3].gameObject.SetActive( false );
//				ui.arrows[4].gameObject.SetActive( false );
//				ui.arrows[5].gameObject.SetActive( false );
//				ui.arrows[6].gameObject.SetActive( false );
//				ui.arrows[7].gameObject.SetActive( false );
//				break;
//			}
		}
	}


	public GameState gamestate 
	{
		get{ return pData.gamestate; }
		set
		{
			pData.gamestate = value;
		}
	}

	public static bool IsEnemy( NPC p1, NPC p2 )
	{
		if(p1.playertype != playerType.NUTUAL && p2.playertype != playerType.NUTUAL)
		{
			if(p1.playertype != p2.playertype)
				return true;
		}

		return false;
	}

	public System.Action<NPC> OnNPCDead;

	public void initial( PropertyData data )
	{
		//pData = data;
		pData = new PropertyData(data.playertype, data.textureName, data.atk, data.hp, data.maxhp, data.atktype, data.charType);
		pData.teamSequence = data.teamSequence;
		playertype = data.playertype;
		print( "-----" + playertype.ToString() + "---->" + data.playertype.ToString() );
		textureName = data.textureName;
		atktype = data.atktype;
		gamestate = data.gamestate;
		hp = data.hp;
	}

	void Awake()
	{
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnHit(NPC enemy, int dmg)
	{
		GameObject fx = NGUITools.AddChild( this.gameObject, OnHitNumberFX );
		fx.GetComponent<XSelfDestroy>().KillInSecs(1.0f);
		fx.GetComponent<UILabel>().text = enemy.pData.atk.ToString();
		fx.transform.localPosition = new Vector3( 0, 0, -10 );
		fx.transform.localScale = new Vector3( 50, 50, 1 );

		hp -= dmg;

		if(pData.hp == 0 && OnNPCDead != null )
			OnNPCDead( this );
	}

	public void playDeath()
	{
		StartCoroutine( _playDeath() );
	}

	public void playInitial()
	{
		StartCoroutine( _playInitial() );
	}

	public void playDeathThenInitial()
	{
		StartCoroutine( _playDeathThenInitial() );
	}

	public IEnumerator _playDeathThenInitial()
	{
		StartCoroutine( _playDeath() );
		yield return new WaitForSeconds( 0.6f );
		StartCoroutine( _playInitial() );

	}

	public IEnumerator _playDeath()
	{
		for(int i=30; i>=0; --i)
		{
			yield return new WaitForSeconds(0.015f);
			this.gameObject.transform.localScale = new Vector3( (float)i/30, (float)i/30, 1 );
		}
	}

	public IEnumerator _playInitial()
	{
		for(int i=0; i<=30; ++i)
		{
			yield return new WaitForSeconds(0.015f);
			this.gameObject.transform.localScale = new Vector3( (float)i/30, (float)i/30, 1 );
		}
	}
}
