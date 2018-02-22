using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float maxHealth;
	private float currentHealth;
	[SerializeField]
	private GameObject enemy;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			enemy.SetActive (false);
		}
	}

	public void HurtEnemy(float damageDealt)
	{
		currentHealth -= damageDealt;
	}
}
