using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	[SerializeField]
	private Slider healthBar;
	private float currentHealth;
	[SerializeField]
	private float maxHealth;

	[SerializeField]
	private GameObject hudHealth;

	// Use this for initialization
	void Start () {
		healthBar.maxValue = maxHealth;
		currentHealth = healthBar.value;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth == maxHealth) {
			hudHealth.SetActive (false);
		} else {
			hudHealth.SetActive (true);
		}

		healthBar.value = currentHealth;

		if (currentHealth <= 0) {
			Debug.Log ("kill player");
		}
	}

	public void HurtPlayer(float damageDealt)
	{
		currentHealth -= damageDealt;
	}
}
