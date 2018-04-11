using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public bool hasWeapon;

	[SerializeField]
	private float attackDelay;
	private bool canAttack;
	private float timeToAttack;

	[SerializeField]
	private float attackSpeed;
	private float attackingTime;
	private bool isAttacking;

	[SerializeField]
	private Animator swordAnim;

	public AttackTrigger theAT;

	public float damage;

	// Use this for initialization
	void Start () {
		timeToAttack = attackDelay;
		attackingTime = attackSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasWeapon) {
			if (!canAttack) {
				timeToAttack -= Time.deltaTime;
			}

			if (timeToAttack <= 0) {
				canAttack = true;
				timeToAttack = attackDelay;
			}

			if (canAttack) {
				if (Input.GetAxisRaw ("Fire1") >= 0.01f) {
					isAttacking = true;
				}
			}

			if (isAttacking) {
				theAT.attack = true;
				attackingTime -= Time.deltaTime;
				if (attackingTime <= 0) {
					theAT.attack = false;
					isAttacking = false;
					canAttack = false;
					attackingTime = attackSpeed;
				}

				if (isAttacking) {
					swordAnim.SetBool ("attacking", true);
				} else {
					swordAnim.SetBool ("attacking", false);
				}
			}
		}
	}
}
