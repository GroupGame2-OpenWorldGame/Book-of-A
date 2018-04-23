using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour 
{

	/*This Script is used to repeatedly set the rain prefab active, wait a bit, deactivate, and so on. With the intention of having organic sporadic
	*rain coming and going every so often 

	*/

		float startDelay = 5f;
		float repeatDelay = 15f;

		public GameObject rainPrefab;
		public bool continueCoroutine; //bool used to determine if coroutine will continue repeating or not 
	//	public bool Raining;

	void Start ()
	{
		InvokeRepeating ("Wait", startDelay, repeatDelay);
	}

	// Update is called once per frame
	void Update () 
	{
		/*while (continueCoroutine == true)
		{
			yield return StartCoroutine("Wait");
		}
		*/
	}

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

}
