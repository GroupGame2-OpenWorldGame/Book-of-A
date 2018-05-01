using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SelectDir{
	Up,
	Down
}

public enum TurnDir{
	Foward,
	Back
}

public enum MenuState{
	Pause,
	Quests
}

public class UIQuestMenu : MonoBehaviour {
	[SerializeField]
	private Animator bookAnimator;

	[SerializeField]
	private Text pageNumText;

	[Header("Quests")]
	[SerializeField]
	private GameObject questMenu;
	[SerializeField]
	private GameObject questList;
	[SerializeField]
	private GameObject questDecription;
	[SerializeField]
	private GameObject[] questPanels;
	[Space(8)]

	[Header("Pause Menu")]
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject[] menuButtons;
	[SerializeField]
	private Text foundText;
	[SerializeField]
	private Text failedText;
	[SerializeField]
	private Text succeedText;

	private MenuState state = MenuState.Pause;
	private int selectedPanel = 0;
	private int pageNum = 0;
	private int totalPages= 1;
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

	public MenuState State{
		get{ return state; }
		set{
			state = value;
			if (value == MenuState.Pause) {
				questMenu.SetActive (false);
				SetUpPauseMenu ();
			} else {
				pauseMenu.SetActive (false);
				SetUpQuestMenu ();
			}
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
		totalPages = ((int)GameDriver.Instance.QuestsUnlocked.Count / 4) + ((GameDriver.Instance.QuestsUnlocked.Count % 4) == 0 ? 0 : 1);
		pageNum = 0;
		Debug.Log ("Total Page Count : " + totalPages);
		pageNumText.text = string.Format ("{0:00} / {1:00}", pageNum + 1, totalPages + 1);
	}

	void Start(){
		bookAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
	}

	private void SetUpPauseMenu(){
		pauseMenu.SetActive (true);
		foundText.text = GameDriver.Instance.QuestsUnlocked.Count.ToString ();
		int failed = 0;
		int success = 0;
		foreach (Quest q in GameDriver.Instance.QuestsUnlocked) {
			if (q.Status == "Failed") {
				failed++;
			} else if (q.Status == "Success") {
				success++;
			}
		}
		failedText.text = failed.ToString();
		succeedText.text = success.ToString ();
		EventSystem.current.SetSelectedGameObject (menuButtons [0]);
		selectedPanel = 0;
	}

	private void SetUpQuestMenu(){
		questMenu.SetActive (true);
		foreach (GameObject go in questPanels) {
			go.GetComponent<UIQuestPanel> ().Select (false);
		}
		SetQuestPanels (pageNum);
	}

	public void ChangeSelected(SelectDir dir){
		if (state == MenuState.Quests) {
			questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (false);

			if (dir == SelectDir.Up) {
				selectedPanel = (int)Mathf.Max (0, selectedPanel - 1);
			} else {
				selectedPanel = (int)Mathf.Min (questsOnPage - 1, selectedPanel + 1);
			}

			questPanels [selectedPanel].GetComponent<UIQuestPanel> ().Select (true);
			questDecription.GetComponent<UIQuestDescription> ().SetInfo (questPanels [selectedPanel].GetComponent<UIQuestPanel> ().QuestId);

		} else if (state == MenuState.Pause) {
			if (dir == SelectDir.Up) {
				selectedPanel = (int)Mathf.Max (0, selectedPanel - 1);
			} else {
				selectedPanel = (int)Mathf.Min (menuButtons.Length - 1, selectedPanel + 1);
			}

			EventSystem.current.SetSelectedGameObject (menuButtons [selectedPanel]);
		}

	}

	public void SetQuestPanels(int pageNum){
		//pageNumText.text = string.Format ("{0:00} / {0:00}", pageNum + 1, totalPages);
		Debug.Log ("SET CALLED");
		settingInfo = true;
		selectedPanel = 0;
		questsOnPage = questPanels.Length;
		int dif = GameDriver.Instance.QuestsUnlocked.Count - questPanels.Length * (pageNum - 1);
		if (dif < questPanels.Length) {
			for (int i = questPanels.Length - dif; i > 0; i--) {
				questPanels [questPanels.Length - i].SetActive (false);
			}
			questsOnPage = dif;
		} 

		for (int i = 0; i < questsOnPage; i++) {
			questPanels [i].GetComponent<UIQuestPanel> ().QuestId = questPanels.Length * (pageNum - 1) + i;
		}

		questPanels [0].GetComponent<UIQuestPanel> ().Select (true);
		questDecription.GetComponent<UIQuestDescription> ().SetInfo (questPanels [0].GetComponent<UIQuestPanel> ().QuestId);

		settingInfo = false;
	}

	public void TurnPage(TurnDir dir){
		StartCoroutine (TurnPageEnum (dir));
	}

	private IEnumerator TurnPageEnum(TurnDir dir){
		Debug.Log ("Update Time: " + bookAnimator.updateMode);
		flipping = true;
		Debug.Log ("TURNPAGE HIT 1");
		if (dir == TurnDir.Foward) {
			if (pageNum >= totalPages) {
				flipping = false;
				yield break;
			}
			questMenu.SetActive (false);
			pauseMenu.SetActive (false);
			pageNumText.text = "";
			bookAnimator.SetTrigger ("Flip");
			pageNum++;
		} else { 
			if (pageNum <= 0) {
				flipping = false;
				yield break;
			}
			questMenu.SetActive (false);
			pauseMenu.SetActive (false);
			pageNumText.text = "";
			bookAnimator.SetTrigger ("FlipBack");
			pageNum--;
		}
		yield return new WaitForSecondsRealtime (0.2f);
		//Debug.Log (!bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Flip"));
		yield return new WaitUntil(() => bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("BookOpen"));

		if (pageNum == 0) {
			State = MenuState.Pause;
		} else {
			State = MenuState.Quests;
		}
			
		pageNumText.text = string.Format ("{0:00} / {1:00}", pageNum + 1, totalPages + 1);

		yield return new WaitWhile(() => settingInfo);

		flipping = false;
		yield break;
	}

	public void Quit(){
		Application.Quit ();
	}

	public void Resume(){
		GameDriver.Instance.CloseQuestMenu ();
	}

	public void ReturnToTitle(){
		GameDriver.Instance.ReturnToTitle ();
	}

}
