using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour {

	private GameObject player;
	public bool attack;
	public PlayerHealth thePH;
	public EnemyAttack theEA;
	public bool readyToAttack;
	public NPCMove theEnemy;
	public AgentMovement theAgent;
	private float tempTimer;
	private bool attacking;

	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		if (thePH == null) {
			thePH = player.GetComponent<PlayerHealth>();
		}

		if (attacking) {
			if (theEnemy != null) {
				theEnemy.attackingAnim = true;
			}
			if (theAgent != null) {
				theAgent.attackingAnim = true;
			}
			tempTimer = 1;
			attacking = false;
		}
			
		tempTimer -= Time.deltaTime;
		if (tempTimer <= 0) {
			if (theEnemy != null) {
				theEnemy.attackingAnim = false;
			}
			if (theAgent != null) {
				theAgent.attackingAnim = false;
			}
//			attacking = false;
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
