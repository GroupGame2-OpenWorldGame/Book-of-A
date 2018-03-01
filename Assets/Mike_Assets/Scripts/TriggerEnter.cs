using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour {

	[SerializeField]
	private NPCMove theNPC;
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private GameObject thisGO;

	void Start()
	{
		thisGO = this.gameObject;
	}

	void Update()
	{
		target = theNPC.thisIsTriggerTarget;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			if (thisGO == target) {
				theNPC.waiting = true;
			}
//			theNPC.waiting = true;
		}
	}

//	void OnTriggerExit (Collider other)
//	{
//		if (other.CompareTag("NPC")) {
//			this.gameObject.tag = "PatrolPoint";
//		}
//		Debug.Log ("Exit " + other.name);
//	}
}
