using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour 
{

	/*This Script is used to repeatedly set the rain prefab active, wait a bit, deactivate, and so on. With the intention of having organic sporadic
	*rain coming and going every so often 

	*/

		//float startDelay = 5f;
		//float repeatDelay = 15f;



		public GameObject rainPrefab;

		public bool continueCoroutine; //bool used to determine if coroutine will continue repeating or not 
		float timer = 15f;
		float tempTime;
	//	public bool Raining;

	void Awake()
	{

	}

	void Start ()
	{
		tempTime = timer; 
		//InvokeRepeating ("Wait", startDelay, repeatDelay);
	}

	// Update is called once per frame
	void Update () 
	{
		tempTime -= Time.deltaTime; 

		if (tempTime <= 0) 
		{
			//rainPrefab.SetActive (!rainPrefab.activeSelf); 
			tempTime = timer; 


			/*
			if (rainPrefab.activeInHierarchy)
			{
				rainPrefab.SetActive (false);
			}

			else
			{
				rainPrefab.SetActive(true);
			}
		//	Debug.Log (timer); 
		}

*/
	}
}
			/*
	IEnumerator Wait()
	{
	
		// suspend execution for 10 seconds
		yield return new WaitForSeconds(10);
		print("Wait" + Time.time);
		rainPrefab.SetActive (true);
		// suspend execution for 10 seconds
		yield return new WaitForSeconds(10);
		rainPrefab.SetActive (false);

	}

*/

}
