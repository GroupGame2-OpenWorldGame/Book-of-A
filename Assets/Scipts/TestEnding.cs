using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnding : MonoBehaviour {

	public string flag1;
	public string flag2;
	public string flag3;
	[Space(8)]

	public bool setFlag1;
	public bool setFlag2;
	public bool setFlag3;

	public Ending ending;

	// Use this for initialization
	void Start () {
		GameDriver.Instance.SetFlag (flag1, setFlag1.ToString());
		GameDriver.Instance.SetFlag (flag2, setFlag2.ToString());
		GameDriver.Instance.SetFlag (flag3, setFlag3.ToString());

		ending.SetEnding ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
