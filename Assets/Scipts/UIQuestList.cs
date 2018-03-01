using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestList : MonoBehaviour {

	[Header("Menu Panels")]
	public GameObject openPanel;
	public GameObject menuPanel;
	public GameObject listPanel;
	public GameObject listContainer;
	public GameObject descriptionPanel;

	public GameObject questPanelPrefab;

	public float questLeft = 35.95f;
	public float questHeight = 72f;

	private List<GameObject> questPanels;

	// Use this for initialization
	void Awake () {
		questPanels = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowQuestList(){
		Debug.Log ("Show");
		//Cursor.visible = true;
		openPanel.SetActive (false);
		menuPanel.SetActive (true);
		descriptionPanel.SetActive (false);
		listPanel.SetActive (true);

		SetQuestPanels (GameDriver.Instance.QuestsUnlocked);
	}

	public void ShowQuestDescription(string questId){
		listPanel.SetActive (false);
		descriptionPanel.SetActive (true);
	}

	public void CloseMenu(){
		menuPanel.SetActive (false);
		foreach (GameObject p in questPanels) {
			Destroy (p);
		}
		openPanel.SetActive (true);
		//Cursor.visible = true;
	}

	private void SetQuestPanels(List<Quest> quests){
		for (int i = 0; i < quests.Count; i++) {
			GameObject newQP = (GameObject)Instantiate (questPanelPrefab, listContainer.transform);
			newQP.GetComponent<UIQuestListPanel> ().SetInfo (quests [i].Name, quests [i].Status, i);
			questPanels.Add (newQP);
		}
	}
}
