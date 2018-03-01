using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObject : MonoBehaviour {

	[SerializeField]
	private float timeAlive = 60f;

	private float deathTimer = 0f;


	public TimedObject(float t){
		timeAlive = t;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		deathTimer -= Time.deltaTime;

		if (deathTimer >= timeAlive) {
			
		}
	}

	private void DestroyObject(){
		
	}
}
