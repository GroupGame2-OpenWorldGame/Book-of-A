using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour {

	[SerializeField]
	private GameObject[] menues;

	public void MenuChange()
	{
		foreach (GameObject temp in menues) {
			temp.SetActive (!temp.activeInHierarchy);
		}
	}
}
