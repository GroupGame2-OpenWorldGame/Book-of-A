using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour {

	public bool attack;
	public PlayerHealth thePH;
	public EnemyAttack theEA;
	public bool readyToAttack;

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			readyToAttack = true;
			if (attack) {
				Debug.Log ("Attacked " + other.gameObject.name);
				thePH.HurtPlayer(theEA.damage);
				attack = false;
			}
		}

	}
}
