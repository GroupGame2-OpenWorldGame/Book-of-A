using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoadQuest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameDriver.Instance.SetQuest ("Verse 1", "InProgress", "");
		GameDriver.Instance.SetQuest ("Verse 2", "InProgress", "");
		GameDriver.Instance.SetQuest ("Verse 3", "InProgress", "");
		GameDriver.Instance.SetQuest ("Verse 4", "InProgress", "");
		GameDriver.Instance.SetQuest ("Verse 5", "InProgress", "");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
