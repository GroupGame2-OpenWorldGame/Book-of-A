using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivate : MonoBehaviour {

	[SerializeField]
	private GameObject[] objects;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			foreach (GameObject obj in objects) {
				obj.SetActive (true);
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			foreach (GameObject obj in objects) {
				obj.SetActive (false);
			}
		}
	}
}
