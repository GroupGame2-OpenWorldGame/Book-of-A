﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditFist : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hit(){
		PlayerHealth.Me.GetHit (this);
	}
}