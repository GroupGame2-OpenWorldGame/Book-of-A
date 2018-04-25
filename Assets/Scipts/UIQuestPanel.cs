using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestPanel : MonoBehaviour {

	[SerializeField]
	private static Color successColor = Color.green;
	[SerializeField]
	private static Color failedColor = Color.red;
	[SerializeField]
	private static Color inProgressColor = Color.black;

	[SerializeField]
	private static Color selectedColor = Color.yellow;

	[SerializeField]
	private static Color unSelectedColor = new Color(0,0,0,0);


	[SerializeField]
	private Text questName;

	[SerializeField]
	private Image questStatus;

	[SerializeField]
	private int questId;

	public int QuestId{
		get{
			return questId;
		}

		set{
			questId = value;
			Debug.Log (value);
			questName.text = GameDriver.Instance.QuestsUnlocked [value].Name;
			string status = GameDriver.Instance.QuestsUnlocked [value].Status;
			if (status == "InProgress") {
				questStatus.fillCenter = false;
				questStatus.color = inProgressColor;
			} else if (status == "Success") {
				questStatus.fillCenter = true;
				questStatus.color = successColor;
			} else if (status == "Failed") {
				questStatus.fillCenter = true;
				questStatus.color = failedColor;
			}
			this.gameObject.SetActive (true);
		}
	}

	public void Select(bool s){
		if (s) {
			this.gameObject.GetComponent<Image> ().color = selectedColor;
			return;
		}

		this.gameObject.GetComponent<Image> ().color = unSelectedColor;
		return;
	}
}
