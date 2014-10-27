using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.ShakePosition(gameObject, new Vector3(1.0f,1.0f,0.0f), 1.0f);
	}
}

