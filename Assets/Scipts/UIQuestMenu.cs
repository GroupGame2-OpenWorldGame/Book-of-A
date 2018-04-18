using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectDir{
	Up,
	Down
}

public class UIQuestMenu : MonoBehaviour {
	
	[SerializeField]
	private Sprite openSprite;
	[SerializeField]
	private Sprite flipSprite;
	[SerializeField]
	private Animation flipAnimation;

	[SerializeField]
	private GameObject questList;
	[SerializeField]
	private GameObject questDecription;

	[SerializeField]
	private GameObject[] questPanels;

	private int selectedPanel = 0;

	private bool filpping = false;

	public void ChangeSelected(SelectDir dir){
		questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);

		if (dir == SelectDir.Up) {
			selectedPanel = (int)Mathf.Max (0, selectedPanel - 1);
		} else {
			selectedPanel = (int)Mathf.Min (questPanels.Length - 1, selectedPanel + 1);
		}
	
		questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (true);
		questDecription.GetComponent<UIQuestDescription> ().SetInfo (questPanels [selectedPanel].GetComponent<UIQuestPanel> ().QuestId);
	}

	public void SetQuestPanels(int pageNum){
		int count = questPanels.Length;
		int dif = GameDriver.Instance.QuestsUnlocked.Count - questPanels.Length * pageNum;
		if (dif < questPanels.Length) {
			for (int i = questPanels.Length - dif; i > 0; i--) {
				questPanels [questPanels.Length - i].SetActive (false);
			}
			count = dif;
		}

		for (int i = 0; i < count; i++) {
			questPanels [i].GetComponent<UIQuestPanel> ().QuestId = questPanels.Length * pageNum + i;
		}
	}
}
