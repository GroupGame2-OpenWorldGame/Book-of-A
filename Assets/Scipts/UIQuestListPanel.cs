using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestListPanel : MonoBehaviour {

	public int id = 0;

	public void SetInfo(string name, string status, int id){
		this.id = id;
		this.GetComponentInChildren<Text> ().text = name;
		Image statusImage = this.GetComponentsInChildren<Image> ()[1];
		if (status == "InProgress") {
			statusImage.color = Color.white;
			statusImage.gameObject.GetComponentInChildren<Text> ().text = "In Progress";
		} else if (status == "Success") {
			statusImage.color = Color.green;
			statusImage.gameObject.GetComponentInChildren<Text> ().text = "Success";
		}else if (status == "Failed") {
			statusImage.color = Color.red;
			statusImage.gameObject.GetComponentInChildren<Text> ().text = "Failed";
		}
	}

	void Awake(){
		this.GetComponent<Button> ().onClick.AddListener (ShowQuestDescription);
	}

	private void ShowQuestDescription(){
		GameObject.FindGameObjectWithTag ("QuestMenu").GetComponent<UIQuestList> ().ShowQuestDescription (id);
	}


}
