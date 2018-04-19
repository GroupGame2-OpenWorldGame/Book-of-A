using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.I)){
			this.GetComponent<Animator>().SetTrigger("Flip");
		} else if(Input.GetKeyUp(KeyCode.U)){
			this.GetComponent<Animator>().SetTrigger("FlipBack");
		}
	}


}
