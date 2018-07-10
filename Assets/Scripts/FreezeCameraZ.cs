using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// small script to keep the Z axis of the camera consistent doesn't work???
/// </summary>
public class FreezeCameraZ : MonoBehaviour{
    private float zValue;
	// Use this for initialization
	void Start (){
	    zValue = transform.position.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3(transform.position.x, transform.position.y, zValue);
	}
}
