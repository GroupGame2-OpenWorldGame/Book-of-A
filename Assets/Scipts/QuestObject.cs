using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour {

	[SerializeField]
	private string counterToAddTo = "";
	private int valueToAdd = 1;

	[SerializeField]
	private MonoBehaviour[] components;

	[SerializeField]
	private GameObject interactSprite;

	[SerializeField]
	private bool startActive = false;

	void Awake(){
		interactSprite.SetActive (false);
		SetActive (startActive);
	}

	public void SetActive(bool y){
		foreach (MonoBehaviour mb in components) {
			mb.enabled = y;
		}
		foreach (Transform t in transform) {
			Debug.Log (t.gameObject.name);
			if (t.gameObject.GetComponent<QuestObject> () != null) {
				t.gameObject.GetComponent<QuestObject> ().SetActive (y);
			}
			t.gameObject.SetActive (y);
		}
		if (gameObject.GetComponent<Collider> () != null) {
			this.GetComponent<Collider>().enabled = y;
		}
		if (gameObject.GetComponent<MeshRenderer> () != null) {
			this.GetComponent<MeshRenderer> ().enabled = y;
		}
			
	}

	public void Collect(){
		GameDriver.Instance.SetCounter (counterToAddTo, "Add", valueToAdd.ToString ());
		Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			interactSprite.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			interactSprite.SetActive(false);
		}
	}

}
