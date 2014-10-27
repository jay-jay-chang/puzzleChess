using UnityEngine;
using System.Collections;

public class ShakeSample : MonoBehaviour {

	void Start(){
		iTween.CameraFadeAdd(); 
	}
	
	void Update () {
		if( Input.GetKeyDown( KeyCode.A ) ) {
			//iTween.CameraFadeFrom( 0.0f, 1.0f );
			iTween.CameraFadeTo( 0.0f, 0.5f );
			//iTween.ShakePosition(gameObject, new Vector3(0.5f,0.5f,0.0f), 0.5f);
		}
		else if( Input.GetKeyDown( KeyCode.S ) ) { 
			//iTween.CameraFadeTo( 1.0f, 0.5f );
			iTween.CameraFadeTo( iTween.Hash( "amount", 1.0f, "time", 0.5f, "oncompletetarget", gameObject, "oncomplete", "OnCameraFadeComplete" ) );
				
		}
	}
	
	void OnCameraFadeComplete () {
		Debug.Log( "OnCameraFadeComplete" );
	}
}
