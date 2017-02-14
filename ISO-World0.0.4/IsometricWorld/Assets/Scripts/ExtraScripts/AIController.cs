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

    public bool Moved = false;

    public int enemyTilesMoved;
    public int MovesToMake;


    private int curr = 0;
    public float ChaseDistance = 5;

    private GameObject player;
    //public GameObject[] Enemies;
    //private Animator animator;
    public AStarPathfinder pathfinder;

    public static AIController instance;
    private TurnBased turnBased;
    public enum State
    {
        Idle = 0,
        Patrolling = 1,
        Chasing = 2,

    }
    public void Start()
    {
        turnBased = GameObject.Find("GameManager").GetComponent<TurnBased>();
        pathfinder = this.GetComponent<AStarPathfinder>();
        //animator = this.GetComponent<Animator>();

        //Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //MovesToMake = TurnBased.instance.EnemyMoves;
        pathfinder = AStarPathfinder.instance;

        player = GameObject.FindGameObjectWithTag("Player");

    }

    public void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                break;


            case State.Patrolling:
                
                UpdatePatrolState();

                //DoPatrol();
                break;

            case State.Chasing:
                UpdateChaseState();

                //DoChase();
                break;


        }
    }

    void UpdatePatrolState()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= ChaseDistance)
        {
            path.Clear();
            currentState = State.Chasing;
        }
        else
        {
            DoPatrol();
        }
    }

    void UpdateChaseState()
    {
        DoChase();

        if (Vector3.Distance(transform.position, player.transform.position) > ChaseDistance)
        {
            path.Clear();
            
            currentState = State.Patrolling;
        }
    }

    private void DoPatrol()
    {
        if (TurnBased.EnemyCanMove == true && TurnBased.EnemyToMove == this.gameObject)
        {
            if (path.Count == 0)
            {
                curr = 0;
                SearchPath(waypoints[currWaypoint]);
            }


            if (enemyTilesMoved == MovesToMake)
            {
                Moved = true;
                return;
            }
            else
            {
                if (curr > path.Count - 1)
                {
                    return;
                }

                //vector to new tile
                Vector3 toTarget = path[curr].gameObject.transform.position - this.transform.position;

                //calculate rotation looking to destiny
                Quaternion rot = Quaternion.LookRotation(new Vector3(toTarget.x, 0, toTarget.z));

                //apply rotation to this transform
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10.0f);

                //move this transform to tile destination               
                transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);               

                //get the tile from the path that we are currently heading
                Vector3 target = path[curr].gameObject.transform.position;

                //if we are close enough to the current tile, move to the next one
                if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.1f)
                {
                    curr++;

                    if (curr > 1)
                    {
                        enemyTilesMoved++;
                        turnBased.EnemyMovesMade++;
                    }


                }

                //calculate distance to the next waypoint
                Transform wpTr = waypoints[currWaypoint].gameObject.GetComponent<Waypoint>().tile.gameObject.transform;
                Vector3 wp = new Vector3(wpTr.position.x, this.transform.position.y, wpTr.position.z);
                float distToCurrWaypoint = Vector3.Distance(this.transform.position, wp);

                //if distance to waypoint is close enough, either stop or move to first again if we are looping
                if (distToCurrWaypoint < 0.2f)
                {
                    if (currWaypoint < waypoints.Length - 1)
                    {
                        currWaypoint++;
                        path.Clear();
                    }
                    else
                    {
                        if (loopPath)
                        {
                            currWaypoint = 0;
                            path.Clear();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    private void DoChase()
    {
        
        if (TurnBased.EnemyCanMove == true && TurnBased.EnemyToMove == this.gameObject)
        {
            

            if (path.Count == 0)
            {
                curr = 0;
                SearchPathToPlayer(player);
            }


            if (enemyTilesMoved == MovesToMake)
            {
                Moved = true;
                return;
            }
            else
            {
                if (curr > path.Count - 1)
                {
                    return;
                }

                //vector to new tile
                Vector3 toTarget = path[curr].gameObject.transform.position - this.transform.position;

                //calculate rotation looking to destiny
                Quaternion rot = Quaternion.LookRotation(new Vector3(toTarget.x, 0, toTarget.z));

                //apply rotation to this transform
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10.0f);

                //move this transform to tile destination
                //this needs to change animator speed instead if using mecanim root control
                transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
                //animator.SetFloat("Speed", 2.0f);

                //get the tile from the path that we are currently heading
                Vector3 target = path[curr].gameObject.transform.position;

                //if we are close enough to the current tile, move to the next one
                if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.1f)
                {
                    curr++;

                    if (curr > 1)
                    {
                        enemyTilesMoved++;
                        turnBased.EnemyMovesMade++;
                    }


                }

                //calculate distance to the next waypoint
                Transform plTr = player.gameObject.GetComponent<PlayerController>().currentTile.gameObject.transform;
                Vector3 pl = new Vector3(plTr.position.x, this.transform.position.y, plTr.position.z);
                float distToPlpoint = Vector3.Distance(this.transform.position, pl);
                



                //if distance to waypoint is close enough, either stop or move to first again if we are looping
                if (distToPlpoint < 0.2f)
                {
                    //if (currWaypoint < waypoints.Length - 1)
                    //{
                       // currWaypoint++;
                        path.Clear();
                    //}
                    //else
                    //{
                        if (loopPath)
                        {
                            currWaypoint = 0;
                            path.Clear();
                        SearchPathToPlayer(player);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
    



    private void SearchPath(GameObject wp)
    {
        if (pathfinder.myPath.Count > 0)
            pathfinder.myPath.Clear();

        RaycastHit h;
        if (Physics.Raycast(this.transform.position + new Vector3(0, 1f, 0), Vector3.down, out h, 100f))
        {
            pathfinder.start = h.transform.gameObject.GetComponent<Tile>();
        }

        pathfinder.end = wp.GetComponent<Waypoint>().tile;

        pathfinder.Search();

        path = pathfinder.myPath;


    }

    private void SearchPathToPlayer(GameObject pl)
    {
        if (pathfinder.myPath.Count > 0)
            pathfinder.myPath.Clear();

        RaycastHit h;
        if (Physics.Raycast(this.transform.position + new Vector3(0, 1f, 0), Vector3.down, out h, 100f))
        {
            pathfinder.start = h.transform.gameObject.GetComponent<Tile>();
        }

        pathfinder.end = pl.GetComponent<PlayerController>().currentTile;

        pathfinder.Search();

        path = pathfinder.myPath;

    }



}
