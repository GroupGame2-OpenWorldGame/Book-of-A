using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour 
{

	//This Script is used to repeatedly set the rain prefab active, wait a bit, deactivate, and so on. With the intention of having organic sporadic
	//rain coming and going every so often 
	public GameObject rainPrefab;

	public bool Raining;


	// Update is called once per frame
	void Update () 
	{
		
	}

	IEnumerator Wait()
	{
		// suspend execution for 20 seconds
		yield return new WaitForSeconds(20);
		print("Wait" + Time.time);
		rainPrefab.SetActive (true);
		// suspend execution for 20 seconds
		yield return new WaitForSeconds(20);
		rainPrefab.SetActive (false);

	}

	IEnumerator Start()
	{
		print("Starting " + Time.time);

		// Start function WaitAndPrint as a coroutine
		yield return StartCoroutine("Wait");
		print("Done " + Time.time);
	}
}
