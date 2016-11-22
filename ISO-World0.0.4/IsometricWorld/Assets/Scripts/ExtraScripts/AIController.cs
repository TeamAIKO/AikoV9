using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour 
{
	public GameObject[] waypoints;
	public int currWaypoint = 0;
	public State currentState = State.Patrolling;
	public List<Tile> path = new List<Tile>();
	public bool loopPath = false;

	private int curr = 0;
	private Animator animator;
	private AStarPathfinder pathfinder;

	public enum State
	{
		Idle = 0,
		Patrolling = 1
	}

	public void Start()
	{
		pathfinder = this.GetComponent<AStarPathfinder>();
		animator = this.GetComponent<Animator>();
	}

	public void Update()
	{
		switch(currentState)
		{
		case State.Idle:
			break;

		case State.Patrolling:
			DoPatrol();
			break;
		}
	}

	private void DoPatrol()
	{
		if(path.Count == 0)
		{
			curr = 0;
			SearchPath(waypoints[currWaypoint]);
		}
		else
		{
			if(curr > path.Count -1)
			{
				animator.SetFloat("Speed", 0.0f);
				return;
			}

			Vector3 toTarget = path[curr].gameObject.transform.position - this.transform.position;
			Quaternion rot = Quaternion.LookRotation(new Vector3(toTarget.x, 0, toTarget.z));
			transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10.0f);

//			transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
			animator.SetFloat("Speed", 2.0f);

			Vector3 target = path[curr].gameObject.transform.position;
			if(Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.1f)
			{
				curr++;
			}

			Transform wpTr = waypoints[currWaypoint].gameObject.GetComponent<Waypoint>().tile.gameObject.transform;
			Vector3 wp = new Vector3(wpTr.position.x, this.transform.position.y, wpTr.position.z);
			float distToCurrWaypoint = Vector3.Distance(this.transform.position, wp);

			if(distToCurrWaypoint < 0.2f)
			{
				if(currWaypoint < waypoints.Length-1)
				{
					currWaypoint++;
					path.Clear();
				}
				else
				{
					if(loopPath)
					{
						currWaypoint = 0;
						path.Clear();
					}
					else
					{
						animator.SetFloat("Speed", 0.0f);
						return;
					}
				}
			}
		}
	}

	private void SearchPath(GameObject wp)
	{
		if(pathfinder.myPath.Count > 0)
			pathfinder.myPath.Clear();

		RaycastHit h;
		if(Physics.Raycast(this.transform.position + new Vector3(0,1f,0), Vector3.down, out h, 100f))
		{
			pathfinder.start = h.transform.gameObject.GetComponent<Tile>();
		}

		pathfinder.end = wp.GetComponent<Waypoint>().tile;

		pathfinder.Search();

		path = pathfinder.myPath;
	}

}
