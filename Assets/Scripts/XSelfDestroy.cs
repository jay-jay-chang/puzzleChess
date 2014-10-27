using UnityEngine;
using System.Collections;

public class XSelfDestroy : MonoBehaviour {
	
	public void kill(){
		GameObject.Destroy( this.gameObject );
	}
	
	public void KillInSecs(float WaitSecs){
		Invoke("kill", WaitSecs);
	}
}
