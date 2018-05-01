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

	[Header("UI")]
	public Text endingText;
	public Text flag1Mark;
	public Text flag2Mark;
	public Text flag3Mark;

	// Use this for initialization
	void Start () {
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
		
	}
}
