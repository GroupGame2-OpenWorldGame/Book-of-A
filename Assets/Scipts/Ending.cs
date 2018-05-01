using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour {

	[Header("Flags")]
	[SerializeField]
	private string flag1;
	[SerializeField]
	private string flag2;
	[SerializeField]
	private string flag3;
	[Space(8)]

	[Header("Endings")]
	[SerializeField]
	private string ending0;
	[SerializeField]
	private string ending1;
	[SerializeField]
	private string ending2;
	[SerializeField]
	private string ending3;
	[SerializeField]
	private string ending1_2;
	[SerializeField]
	private string ending1_3;
	[SerializeField]
	private string ending2_3;
	[SerializeField]
	private string endingAll;
	[Space(8)]

	[Header("Book")]
	public Animator bookAnimator;

	[Header("UI-Ending")]
	public GameObject page1;
	public Text endingText;
	public Text flag1Mark;
	public Text flag2Mark;
	public Text flag3Mark;
	[Space(8)]

	[Header("Ending Menu")]
	public GameObject page2;
	public GameObject[] endingButtons1;
	[Space(8)]

	[Header("Credits Page")]
	public GameObject page3;
	public GameObject[] endingButtons2;

	private int page = 1;
	private int selectedButton = 0;
	private GameObject[] activeButtons;
	private bool flipping = false;
	private GameObject currentPage;
	private bool verticalPress = false;


	// Use this for initialization
	void Start () {
		currentPage = page1;
		page1.SetActive (true);
		page2.SetActive (false);
		page3.SetActive (false);
		SetEnding ();
	}

	private void SetFlag1Text(bool isTrue){
		if (isTrue) {
			flag1Mark.text = "Gained the Sargent's support";
			flag1Mark.color = Color.green;
			return;
		}
		flag1Mark.text = "Did not gain the Sargent's support";
		flag1Mark.color = Color.red;
	}

	private void SetFlag2Text(bool isTrue){
		if (isTrue) {
			flag2Mark.text = "Found the mountaintop ambush spot";
			flag2Mark.color = Color.green;
			return;
		}
		flag2Mark.text = "Did not find an ambush spot";
		flag2Mark.color = Color.red;
	}

	private void SetFlag3Text(bool isTrue){
		if (isTrue) {
			flag3Mark.text = "Received help from the Wobani";
			flag3Mark.color = Color.green;
			return;
		}
		flag3Mark.text = "Did not receive outside help";
		flag3Mark.color = Color.red;
	}

	public void SetEnding(){
		if (GameDriver.Instance.IsFlagTrue (flag1)) {
			SetFlag1Text (true);
			if (GameDriver.Instance.IsFlagTrue (flag2)) {
				SetFlag2Text (true);
				if (GameDriver.Instance.IsFlagTrue (flag3)) {
					SetFlag3Text (true);
					endingText.text = endingAll;
					return;
				} 
				SetFlag3Text (false);
				endingText.text = ending1_2;
				return;
			}
			SetFlag2Text (false);
			if (GameDriver.Instance.IsFlagTrue (flag3)) {
				SetFlag3Text (true);
				endingText.text = ending1_3;
				return;
			} 
			SetFlag3Text (false);
			endingText.text = ending1;
			return;
		}
		SetFlag1Text (false);

		if (GameDriver.Instance.IsFlagTrue (flag2)) {
			SetFlag2Text (true);
			if (GameDriver.Instance.IsFlagTrue (flag3)) {
				SetFlag3Text (true);
				endingText.text = ending2_3;
				return;
			} 
			SetFlag3Text (false);
			endingText.text = ending2;
			return;
		}
		SetFlag2Text (false);

		if (GameDriver.Instance.IsFlagTrue (flag3)) {
			SetFlag3Text (true);
			endingText.text = ending3;
			return;
		} 
		SetFlag3Text (false);
		endingText.text = ending0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!flipping) {
			if (Input.GetAxis ("Select") >= 0.01) {
				if (page == 1) {
					flipping = true;
					StartCoroutine ("TurnPage");
				} else {
					EventSystem.current.currentSelectedGameObject.GetComponent<Button> ().onClick.Invoke ();
				}
			}

			if (!verticalPress) {
				if (Input.GetAxis ("Vertical") >= 0.01) {
					selectedButton = (int)Mathf.Max (0, selectedButton - 1);
					EventSystem.current.SetSelectedGameObject (activeButtons [selectedButton]);
					verticalPress = true;
				} else if (Input.GetAxis ("Vertical") <= -0.01) {
					selectedButton = (int)Mathf.Min (activeButtons.Length - 1, selectedButton + 1);
					EventSystem.current.SetSelectedGameObject (activeButtons [selectedButton]);
					verticalPress = true;
				} 
			} else if (Input.GetAxis("Vertical") == 0) {
				verticalPress = false;
			}

		}
	}

	public IEnumerator TurnPage(){
		page++;
		currentPage.SetActive (false);
		bookAnimator.SetTrigger ("Flip");
		yield return new WaitForSecondsRealtime (0.2f);
		yield return new WaitUntil(() => bookAnimator.GetCurrentAnimatorStateInfo (0).IsName ("BookOpen"));

		if (page == 2) {
			currentPage = page2;
			page2.SetActive (true);
			EventSystem.current.SetSelectedGameObject (endingButtons1 [0]);
			selectedButton = 0;
			activeButtons = endingButtons1;
		} else if (page == 3) {
			currentPage = page3;
			page3.SetActive (true);
			selectedButton = 0;
			activeButtons = endingButtons2;
			EventSystem.current.SetSelectedGameObject (endingButtons2 [0]);
		}
		flipping = false;
		yield break;
	}

	public void Quit(){
		Application.Quit ();
	}
		
	public void ReturnToTitle(){
		GameDriver.Instance.ReturnToTitle ();
	}

	public void Credits(){
		flipping = true;
		StartCoroutine ("TurnPage");
	}
}
