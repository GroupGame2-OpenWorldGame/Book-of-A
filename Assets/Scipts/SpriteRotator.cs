using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour {

	private Transform cameraTransform;

	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = Quaternion.LookRotation (Camera.main.transform.forward);
	}
}
