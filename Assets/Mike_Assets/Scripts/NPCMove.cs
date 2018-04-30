using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class NPCMove : MonoBehaviour {

	private NPCMove thisScript;

	[SerializeField]
	private Transform destination;

	private NavMeshAgent navAgent;

	[SerializeField]
	private bool isMovingTarget;

	[SerializeField]
	private GameObject[] patrolPoints;
	[SerializeField]
	private int currentPoint;
	[SerializeField]
	private bool randomPatrol;
	[SerializeField]
	private bool patroling;

	[SerializeField]
	private float waitTime;
	private float currentWaitTime;
	public bool waiting;

	[SerializeField]
	private bool randomWait;
	[SerializeField]
	private float ranMin;
	[SerializeField]
	private float ranMax;

	public bool seePlayer;
	[SerializeField]
	private GameObject player;

	public GameObject thisIsTriggerTarget;

	public Animator theAnimAI;

	public bool attackingAnim;

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		thisScript = this.GetComponent <NPCMove> ();

		navAgent = this.GetComponent<NavMeshAgent> ();

		if (randomPatrol && !patroling) {
			patroling = true;
		}

		if (navAgent == null) {
			Debug.LogError ("The nav mesh agent is not attatched to " + gameObject.name);
		} else if (patroling) {
			ChangePatrol ();
		} else {
			SetDestination ();
		}


		if (randomWait) {
			waitTime = Random.Range (ranMin, ranMax);
		}
		currentWaitTime = waitTime;
	}

	void Update()
	{
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		if (!seePlayer) {
			theAnimAI.SetBool ("Run", false);
			theAnimAI.SetBool ("Attack", false);
			if (isMovingTarget) {
				SetDestination ();
			}

			if (currentPoint >= patrolPoints.Length) {
				currentPoint = 0;
			}

			if (waiting) {
				currentWaitTime -= Time.deltaTime;
				if (currentWaitTime <= 0) {
					waiting = false;
					ChangePatrol ();
					if (randomWait) {
						waitTime = Random.Range (ranMin, ranMax);
					}
					currentWaitTime = waitTime;
				}
			}
			if (waiting) {
				theAnimAI.SetBool ("Walk", false);
			} else {
				theAnimAI.SetBool ("Walk", true);
			}
		} else {
			FollowPlayer ();
			if (seePlayer) {
				theAnimAI.SetBool ("Run", true);
			} else {
				theAnimAI.SetBool ("Run", false);
			}
		} if (attackingAnim) {
			theAnimAI.SetBool ("Attack", true);
			attackingAnim = false;
		} else {
			theAnimAI.SetBool ("Attack", false);
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

	public void ChangePatrol()
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
		navAgent.SetDestination (player.transform.position);
	}

	public void Die()
	{
		theAnimAI.SetBool ("Die", true);
		thisScript.enabled = false;
	}
}
