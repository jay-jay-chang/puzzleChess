//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

public class ButtonCallBack : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick,
	}

	public System.Action < ButtonCallBack, Trigger > CallBack;
	public Trigger trigger = Trigger.OnClick;
	public int id1;
	public int id2;
	
	bool mStarted = false;
	bool mHighlighted = false;
	
	void Start () { mStarted = true; }
	
	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }
	
	void OnHover (bool isOver)
	{
		if (enabled)
		{
			if (((isOver && trigger == Trigger.OnMouseOver) ||
			     (!isOver && trigger == Trigger.OnMouseOut))) Send();
			mHighlighted = isOver;
		}
	}

	void OnPress (bool isPressed)
	{

		if (enabled)
		{
			if ((isPressed && trigger == Trigger.OnPress) ||
			    (!isPressed && trigger == Trigger.OnRelease))
			{
				Send();
			}
		}
	}

	void OnClick () { if (enabled && trigger == Trigger.OnClick) Send(); }
	
	void OnDoubleClick () { if (enabled && trigger == Trigger.OnDoubleClick) Send(); }
	
	void Send ()
	{
		if (CallBack == null) return;

		CallBack( this, trigger );
	}
}