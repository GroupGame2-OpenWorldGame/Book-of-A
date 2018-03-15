using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour {

	[SerializeField]
	private Transform destination; // Agent moves to 

	private NavMeshAgent navAgent; // Identify which agent

	[SerializeField]
	private bool isMovingTarget;  // chasing or not chasing 

	[SerializeField]
	private GameObject[] patrolPoints; // array of stops/ destinations
	[SerializeField]
	private int currentPoint; // current stop
	[SerializeField]
	private bool randomPatrol;  // randomize patrol or not 
	[SerializeField]
	private bool patroling;  // patrolling or not 

	[SerializeField]
	private float waitTime;  // these are used to determine how long you stay at each stop
	private float currentWaitTime;
	public bool waiting;

	[SerializeField]
	private bool randomWait;  // randomize wait time or not
	[SerializeField]
	private float ranMin;
	[SerializeField]
	private float ranMax;

	public bool seePlayer;  // begin chase or not (is player present) 
	[SerializeField]
	private GameObject player;  // player reference

	public GameObject thisIsTriggerTarget;  //redundant reference to destination for experimentation

	// Use this for initialization
	void Start () {
		navAgent = this.GetComponent<NavMeshAgent> ();  // get nav mesh component 

		if (randomPatrol && !patroling) // makes sure patrolling is on if random patrolling is also on 
		
		{
			patroling = true;
		}

		if (navAgent == null)
		{
			Debug.LogError ("The nav mesh agent is not attatched to " + gameObject.name);  // makes sure nav mesh component is obtained 
		} else if (patroling) 
		{
			ChangePatrol ();  // method call
		} else 
		{
			SetDestination ();  // call method 
		}


		if (randomWait) 
		{
			waitTime = Random.Range (ranMin, ranMax);
		}
		currentWaitTime = waitTime;
	}

	void Update()
	{
		if (!seePlayer)  // check if you're following the player or navigating patrol points 
		{
			if (isMovingTarget) 
			{
				SetDestination ();
			}

			if (currentPoint >= patrolPoints.Length)  // if final patrol point is reached begin again at original patrol point 
			{
				currentPoint = 0;
			}

			if (waiting)   // if you are waiting keep track of time waited, else begin patrolling again 
			{
				currentWaitTime -= Time.deltaTime;
				if (currentWaitTime <= 0) 
				{
					waiting = false;
					ChangePatrol ();
					if (randomWait) 
					{
						waitTime = Random.Range (ranMin, ranMax);
					}
					currentWaitTime = waitTime;
				}
			}
		} else
		
		{
			FollowPlayer ();
		}
	}

	void SetDestination () {
		if (destination != null && !patroling) {
			Vector3 targetVector = destination.transform.position;
			navAgent.SetDestination (targetVector);
		} else if (destination != null && patroling) {
			destination = patrolPoints [currentPoint].transform;
			navAgent.SetDestination (destination.transform.position);
		}
		thisIsTriggerTarget = destination.gameObject;
	}

	//	void OnTriggerEnter (Collider other)
	//	{
	//		if (other.CompareTag("PatrolPoint")) {
	//			ChangePatrol ();
	//			other.gameObject.SetActive (false);
	//		}
	//		Debug.Log ("enter " + other.name);
	//	}

	public void ChangePatrol()  // make sure agent is continuing past the first patrol point 
	{
		int tempPoint;
		if (randomPatrol) {
			tempPoint = Random.Range (0, patrolPoints.Length);
		} else {
			tempPoint = currentPoint + 1;
		}

		if (tempPoint == currentPoint) {
			tempPoint = currentPoint + 1;
		}
		if (tempPoint >= patrolPoints.Length) {
			tempPoint = 0;
		}
		navAgent.SetDestination (patrolPoints [tempPoint].transform.position);
		currentPoint = tempPoint;
		thisIsTriggerTarget = patrolPoints [tempPoint];
	}

	void FollowPlayer()
	{
		navAgent.SetDestination (player.transform.position);  // follow player if player is mean to be followed 
	}
}
