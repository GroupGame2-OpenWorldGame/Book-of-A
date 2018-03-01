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

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			target = theNPC.thisIsTriggerTarget;
			if (thisGO == target) {
				theNPC.waiting = true;
			}
		}
	}
}
