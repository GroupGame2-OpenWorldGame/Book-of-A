using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour {

	[SerializeField]
	private NPCMove theNPC;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			theNPC.waiting = true;
		}
		Debug.Log ("Enter " + other.gameObject.name);
	}

//	void OnTriggerExit (Collider other)
//	{
//		if (other.CompareTag("NPC")) {
//			this.gameObject.tag = "PatrolPoint";
//		}
//		Debug.Log ("Exit " + other.name);
//	}
}
