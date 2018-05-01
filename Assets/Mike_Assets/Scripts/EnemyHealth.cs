using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float maxHealth;
	private float currentHealth;
	[SerializeField]
	private bool questEnemy;
	private QuestObject theQO;
	[SerializeField]
	private bool aggressive;
	[SerializeField]
	private GameObject playerZone;

	public AgentMovement theAM;
	public NPCMove theNM;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		if (!aggressive) {
			playerZone.SetActive (false);
		}
		if (questEnemy) {
			theQO = GetComponent<QuestObject> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			if (questEnemy) {
				theQO.Collect ();
			}
//			this.gameObject.SetActive (false);
			if (theAM != null) {
				theAM.Die ();
			}
			if (theNM != null) {
				theNM.Die ();
			}
			playerZone.SetActive (false);
		} else if (!aggressive && currentHealth < maxHealth) {
			playerZone.SetActive (true);
			aggressive = true;
		}
	}

	public void HurtEnemy(float damageDealt)
	{
		currentHealth -= damageDealt;
	}

	public void Hit(){
		Debug.Log ("I HIT YOU"); 	
	}
}
