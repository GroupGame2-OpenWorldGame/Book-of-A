using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour {

	[SerializeField]
	private MonoBehaviour[] components;

	[SerializeField]
	private bool startActive = false;

	void Awake(){
		SetActive (startActive);
	}

	public void SetActive(bool y){
		foreach (MonoBehaviour mb in components) {
				mb.enabled = y;
		}
	}
}
