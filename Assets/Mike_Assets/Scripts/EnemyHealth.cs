using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float maxHealth;
	private float currentHealth;
	[SerializeField]
	private bool questEnemy;
	private QuestObject theQO;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		if (questEnemy) {
			theQO = GetComponent<QuestObject> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			if (questEnemy) {
				theQO.Collect();
			}
			this.gameObject.SetActive (false);
		}
	}

	public void HurtEnemy(float damageDealt)
	{
		currentHealth -= damageDealt;
	}
}
