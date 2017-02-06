using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TurnBased : MonoBehaviour
{
    public int NumberOfMoves;
    public int MovesMade;
    

    public int PlayerPathLength;
    public int enemyPathLength;

    public int EnemyMovesMade;
    public int EnemyMoves;
    public GameObject Player;

    public static bool EnemyCanMove;
    public static bool PlayerCanMove;

    public bool turnInProgress = false;

    public PlayerController playerController;
    public static TurnBased instance;
    public GameObject[] Enemies;
    
    
    public enum GameStates
    {
        Planning,
        PlayerTurn,
        EndTurn,
        EnemyTurn
    }

    public GameStates currentState;
    
    // Use this for initialization
    void Start()
    {        
        currentState = GameStates.Planning;

        //finding all game objects with the tag of "Enemy"
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //EnemyMovesMade = AIController.instance.enemyTilesMoved;

        //getting a reference to the PlayerController script
        //GameObject thePlayer = GameObject.Find("Player");
        //PlayerController playerController = thePlayer.GetComponent<PlayerController>();    
        
           
        TotalMoves();
        //EnemyMovesBeingMade();
        
    }

    // Update is called once per frame
    void Update()
    {

        //MovesMade = playerController.tilesMoved;
        MovesMade = PlayerController.instance.tilesMoved;
        PlayerPathLength = PlayerController.instance.path.Count - 1;
        
     


        switch (currentState)
        {
            


            //CREATE PLANNING - SET PATH CONFIRMED TO FALSE UNITLL BUTTON PRESS. WHEN CONFIRMED MOVE TO PLAYER TURN THEN CHANGE BOOL TO TRUE. TURN OFF ONCE CLOSE TO END NODE OR MOVES LEFT = 0
            //MAYBE TURN OFF RAYCASTING IN PLAYER TURN TO PREVENT CHEATING OF MOVES
            case (GameStates.Planning):

                //Debug.Log("Planning");
                EnemyCanMove = false;
                EnemyMovesMade = 0;
                if (PlayerController.instance.pathConfirmed == true)
                {
                    currentState = GameStates.PlayerTurn;
                }
                
                break;

            case (GameStates.PlayerTurn):
                
                    PlayerCanMove = true;
                //Debug.Log("Player Turn");               

                //lock their transform. use bools for able to move. when they click end turn goes straight to enemy turn. count how many moves enemy has left
                // when eney runs out of moves goes back to player turn. unlock their transform and resets moves left.

                if (MovesMade >= NumberOfMoves)
                {
                    Debug.Log("Out of Moves");
                    currentState = GameStates.EndTurn;
                    PlayerCanMove = false;                                      
                }

                else if (MovesMade == PlayerPathLength)
                {
                    currentState = GameStates.EndTurn;
                    
                }
                break;

            case (GameStates.EndTurn):
                //when the player has run out of turn goes to end turn then staight to enemy turn 
                Debug.Log("End Turn");
                PlayerCanMove = false;
                //this.gameObject.GetComponent<PlayerController>().animator.SetBool("run", false);

                PlayerController.instance.animator.SetBool("run", false);
                PlayerController.instance.pathConfirmed = false;


                AStarPathfinder.instance.myPath.Clear();
                
                PlayerController.instance.isRunning = false;

                currentState = GameStates.EnemyTurn;
                break;

            case (GameStates.EnemyTurn):
                //put enemy Ai code in here. or reference the enemy Ai Script
                //currentState = GameStates.PlayerTurn;                             
                Debug.Log("Enemy Turn");

                EnemyCanMove = true;

                for (int i = 0; i < Enemies.Length; i++)

                {
                    Debug.Log(Enemies[i].name);
                }
                
                if (EnemyMovesMade >= EnemyMoves)
                {
                    
                    Debug.Log("enemy moves finished");
                    currentState = GameStates.Planning;
                    //MovesMade = NumberOfMoves + 1;
                    playerController.tilesMoved = 0;
                    //reset enemy turns
                    ResetEnemyMoves();
                }
               
                break;
        }
    }


    void MoveCountDown()
    {
        MovesMade--;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
        {
            MoveCountDown();
            Debug.Log("-1");
        }
    }

    void ConfirmingPath()
    {
        PlayerController.instance.pathConfirmed = true;
    }

    void ResetMovesLeft()
    {
        MovesMade = NumberOfMoves + 1;
    }

    public void ConfirmedPathTrue()
    {
        //Player.GetComponent<PlayerController>().pathConfirmed = true;
        PlayerController.instance.pathConfirmed = true;
        Debug.Log("Confirmed Path");
    }

    public void ConfirmedPathFalse()
    {
        Player.GetComponent<PlayerController>().pathConfirmed = false;
    }

    //takes the number of moves avaible from each individual AI character and adds them all together
    //to get the total number of moves avaible
    void TotalMoves()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            EnemyMoves += Enemies[i].GetComponent<AIController>().MovesToMake;
            //EnemyMovesMade += Enemies[i].GetComponent<AIController>().enemyTilesMoved;
        }
    }

    //counts how many moves the enemy is making
    void EnemyMovesBeingMade()
    {
        foreach(GameObject enemies in Enemies)
        {
            //EnemyMoves += Enemies[i].GetComponent<AIController>().MovesToMake;
            // EnemyMovesMade += Enemies[i].GetComponent<AIController>().enemyTilesMoved;
            EnemyMovesMade += enemies.GetComponent<AIController>().enemyTilesMoved;
            
        }
    }
    void ResetEnemyMoves()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
           Enemies[i].GetComponent<AIController>().enemyTilesMoved = 0;
            //EnemyMovesMade += Enemies[i].GetComponent<AIController>().enemyTilesMoved;
        }
    }
}
