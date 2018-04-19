using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestDescription : MonoBehaviour {
	[SerializeField]
	private static Color successColor = Color.green;
	[SerializeField]
	private static Color failedColor = Color.red;
	[SerializeField]
	private static Color inProgressColor = Color.yellow;


	[SerializeField]
	private Text name;
	[SerializeField]
	private Text description;
	[SerializeField]
	private Text status;

	public void SetInfo(int questId){
		if (questId >= 0 && questId < GameDriver.Instance.QuestsUnlocked.Count) {
			Quest q = GameDriver.Instance.QuestsUnlocked [questId];

			name.text = q.Name;
			description.text = q.Description;
			status.text = q.Status;

			if (q.Status == "InProgress") {
				status.color = inProgressColor;
			} else if (q.Status == "Success") {
				status.color = successColor;
			} else if (q.Status == "Failed") {
				status.color = failedColor;
			}
			return;
		}
		name.text = "";
		description.text = "";
		status.text = "";
	}
}
