using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour {

	public bool attack;
	public PlayerHealth thePH;
	public EnemyAttack theEA;
	public bool readyToAttack;
	public NPCMove theEnemy;
	private float tempTimer;
	private bool attacking;

	void Update () {
		if (attacking) {
			theEnemy.attackingAnim = true;
			tempTimer = 1;
		}
			
		tempTimer -= Time.deltaTime;
		if (tempTimer <= 0) {
			theEnemy.attackingAnim = false;
			attacking = false;
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			readyToAttack = true;
			if (attack) {
				attacking = true;
				Debug.Log ("Attacked " + other.gameObject.name);
				thePH.HurtPlayer(theEA.damage);
				attack = false;
			}
		}

	}
}
