using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectDir{
	Up,
	Down
}

public enum TurnDir{
	Foward,
	Back
}

public class UIQuestMenu : MonoBehaviour {
	[SerializeField]
	private Animator bookAnimator;

	[SerializeField]
	private GameObject questList;
	[SerializeField]
	private GameObject questDecription;
	[SerializeField]
	private Text pageNumText;

	[SerializeField]
	private GameObject[] questPanels;

	private int selectedPanel = 0;
	private int pageNum = 0;
	private int totalPages= 0;
	private int questsOnPage = 4;

	private bool flipping = false;
	private bool settingInfo = false;

	private bool hasLeftPage = false;
	private bool hasRightPage = false;

	public bool IsFlipping{
		get{
			return flipping;
		}
	}

	public int PageNumber{
		get{
			return pageNum;
		}
	}

	/* public bool HasLeftPage{
		get{
			return hasLeftPage;
		}
	}

	public bool HasRightPage{
		get{
			return hasRightPage;
		}
	} */

	public void RefreshPageCount(){
		totalPages = ((int)GameDriver.Instance.QuestsUnlocked.Count / 4) + ((GameDriver.Instance.QuestsUnlocked.Count % 4) == 0 ? 0 : 1) - 1;
		Debug.Log ("Total Page Count : " + totalPages);
		pageNumText.text = string.Format ("{0:00} / {1:00}", pageNum + 1, totalPages + 1);
	}

	void Awake(){
		bookAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
	}

	public void ChangeSelected(SelectDir dir){
		questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);

		if (dir == SelectDir.Up) {
			selectedPanel = (int)Mathf.Max (0, selectedPanel - 1);
		} else {
			selectedPanel = (int)Mathf.Min (questsOnPage - 1, selectedPanel + 1);
		}
	
		questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (true);
		questDecription.GetComponent<UIQuestDescription> ().SetInfo (questPanels [selectedPanel].GetComponent<UIQuestPanel> ().QuestId);
	}

	public void SetQuestPanels(int pageNum){
		//pageNumText.text = string.Format ("{0:00} / {0:00}", pageNum + 1, totalPages);
		Debug.Log ("SET CALLED");
		settingInfo = true;
		selectedPanel = 0;
		questsOnPage = questPanels.Length;
		int dif = GameDriver.Instance.QuestsUnlocked.Count - questPanels.Length * pageNum;
		if (dif < questPanels.Length) {
			for (int i = questPanels.Length - dif; i > 0; i--) {
				questPanels [questPanels.Length - i].SetActive (false);
			}
			questsOnPage = dif;
		} 

		for (int i = 0; i < questsOnPage; i++) {
			questPanels [i].GetComponent<UIQuestPanel> ().QuestId = questPanels.Length * pageNum + i;
		}

		questPanels [0].GetComponent<UIQuestPanel> ().Select (true);
		questDecription.GetComponent<UIQuestDescription> ().SetInfo (questPanels [0].GetComponent<UIQuestPanel> ().QuestId);

		settingInfo = false;
	}

	public void TurnPage(TurnDir dir){
		StartCoroutine (TurnPageEnum (dir));
	}

	private IEnumerator TurnPageEnum(TurnDir dir){
		Debug.Log ("ENUMHIT");
		flipping = true;
		Debug.Log ("TURNPAGE HIT 1");
		if (dir == TurnDir.Foward) {
			if (pageNum >= totalPages) {
				flipping = false;
				yield break;
			}
			questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);
			questList.SetActive (false);
			questDecription.SetActive (false);
			pageNumText.text = "";
			bookAnimator.SetTrigger ("Flip");
			pageNum++;
		} else { 
			if (pageNum <= 0) {
				flipping = false;
				yield break;
			}
			questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);
			questList.SetActive (false);
			questDecription.SetActive (false);
			pageNumText.text = "";
			bookAnimator.SetTrigger ("FlipBack");
			pageNum--;
		}
		yield return new WaitForSecondsRealtime (0.2f);
		//Debug.Log (!bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Flip"));
		yield return new WaitUntil(() => bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("BookOpen"));

		questList.SetActive (true);
		questDecription.SetActive (true);
		pageNumText.text = string.Format ("{0:00} / {1:00}", pageNum + 1, totalPages + 1);
		SetQuestPanels (pageNum);

		yield return new WaitWhile(() => settingInfo);

		flipping = false;
		yield break;
	}
}
