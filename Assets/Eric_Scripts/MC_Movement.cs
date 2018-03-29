using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W))
			GetComponent<Animator> ().SetBool ("Running", true);
		else
			GetComponent<Animator> ().SetBool ("Running", false);
		
	}
}
