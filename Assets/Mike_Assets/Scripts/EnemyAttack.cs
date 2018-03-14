using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	public float damage;
	public bool playerInRange;
	public EnemyAttackTrigger theEAT;
	private float timeToAttack;
	[SerializeField]
	private float attackDelay;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeToAttack -= Time.deltaTime;

		if (theEAT.readyToAttack) {
		
			if (timeToAttack <= 0) {
				theEAT.attack = true;
				timeToAttack = attackDelay;
			}
		}
	}
}
