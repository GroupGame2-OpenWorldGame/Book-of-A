using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour {

	[SerializeField]
	private Text foundText;
	[SerializeField]
	private Text failedText;
	[SerializeField]
	private Text succeedText;

	[SerializeField]
	private Button resumeButton;
	[SerializeField]
	private Button quitButton;

	[SerializeField]
	private Animator bookAnimator;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnPage(TurnDir dir){
		if (dir == TurnDir.Foward) {
	
		}
	}
}
