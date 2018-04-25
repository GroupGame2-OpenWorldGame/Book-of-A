using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private GameObject[] questPanels;

	private int selectedPanel = 0;
	private int pageNum = 0;
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

	public bool HasLeftPage{
		get{
			return hasLeftPage;
		}
	}

	public bool HasRightPage{
		get{
			return hasRightPage;
		}
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
		Debug.Log ("SET CALLED");
		hasLeftPage = (pageNum != 0);
		settingInfo = true;
		selectedPanel = 0;
		questsOnPage = questPanels.Length;
		int dif = GameDriver.Instance.QuestsUnlocked.Count - questPanels.Length * pageNum;
		hasRightPage = (dif > questPanels.Length);
		Debug.Log ("Has right page? : " + hasRightPage);
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
			Debug.Log ("TF: " + hasRightPage);
			if (!hasRightPage) {
				flipping = false;
				yield break;
				Debug.Log ("NOTBREAK!!!");
			}
			questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);
			questList.SetActive (false);
			questDecription.SetActive (false);
			bookAnimator.SetTrigger ("Flip");
			pageNum++;
		} else {
			Debug.Log ("TB: " + hasLeftPage); 
			if (!hasLeftPage) {
				flipping = false;
				yield break;
			}
			questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);
			questList.SetActive (false);
			questDecription.SetActive (false);
			bookAnimator.SetTrigger ("FlipBack");
			pageNum--;
		}
		yield return new WaitForSecondsRealtime (0.2f);
		//Debug.Log (!bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Flip"));
		yield return new WaitUntil(() => bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("BookOpen"));

		questList.SetActive (true);
		questDecription.SetActive (true);
		SetQuestPanels (pageNum);

		yield return new WaitWhile(() => settingInfo);

		flipping = false;
		yield break;
	}
}
