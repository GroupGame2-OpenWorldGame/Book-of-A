using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour {

	public bool attack;
	public Attack theA;

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Enemy") {
			if (attack) {
				EnemyHealth theEH = other.GetComponentInParent<EnemyHealth> ();
				theEH.HurtEnemy (theA.damage);
			}
		}

	}
}
