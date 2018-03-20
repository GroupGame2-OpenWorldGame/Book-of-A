using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour {

	[SerializeField]
	private AgentMovement theAM;
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
		if (other.gameObject.tag == "Animal") {
			target = theAM.thisIsTriggerTarget;
			if (thisGO == target) {
				theAM.waiting = true;
			}
		}
	}
}
