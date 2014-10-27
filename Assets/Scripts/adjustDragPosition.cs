using UnityEngine;
using System.Collections;

public class adjustDragPosition : MonoBehaviour {

	public static int ScreenWidth = 640;
	public static int ScreenHeight = 1136;

	bool pressed = false;

	void OnPress(bool isPress)
	{
		pressed = isPress;
	}

	void OnDrag()
	{
		if(!pressed)
			return;

		if(UICamera.currentTouch != null){
			Camera cam = UICamera.currentTouch.pressedCam;

			this.gameObject.transform.localPosition = 
				new Vector3( (UICamera.currentTouch.pos.x/cam.pixelWidth - 0.5f)*ScreenWidth,
				            (UICamera.currentTouch.pos.y/cam.pixelHeight - 0.5f)*ScreenHeight, 0 );
		}
	}
}
