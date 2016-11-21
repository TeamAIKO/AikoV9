using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    private List<Tile> Enemypath;
    private int curr = 0;
    public static EnemyAI instance;
    public bool EnemyPathConfirmed = false;

    public Transform[] PatrolPoints;

    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking,
        Returning
    }

    public EnemyStates currentEnemyState;

    public void SetEnemeyPath(List<Tile> Ep)
    {
        curr = 0;
        Enemypath = Ep;
    }

    // Use this for initialization
    void Start()
    {
        instance = this;
        currentEnemyState = EnemyStates.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {

        //insert Enemy Ai behaviours
        switch (currentEnemyState)
        {
            //the Enemy has to be travelling between the 2 points using A*. Maybe using a start and end node system for the partrolling.
            case (EnemyStates.Patrolling):

                break;

            //when enemy starts chasing the player maybe update the end node for the enemy to be the players position? or node that the player is currently on?
            case (EnemyStates.Chasing):
                break;

            //when in a certain distance rather nodes maybe perform an attack. this maybe useful for different varieties of enemy that have different attack distances.
            case (EnemyStates.Attacking):
                break;

            // should simply return the enemy unit to its orginal patrol path. maybe i should save the last position that it was on when it left it patrol route 
            //in order to carry on with patrolling rather than starting the whole patrol route again
            case (EnemyStates.Returning):
                break;            
        }
    }

    void Move()
    {
        if (Enemypath == null || Enemypath.Count == 0 || curr > Enemypath.Count - 1)
            return;

        if (!EnemyPathConfirmed)
            return;


        Vector3 toTarget = Enemypath[curr].gameObject.transform.position - this.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(toTarget.x, 0, toTarget.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10.0f);

        transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);

        Vector3 target = Enemypath[curr].gameObject.transform.position;
        if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.1f)
        {
            curr++;
        }
    }

    
}
