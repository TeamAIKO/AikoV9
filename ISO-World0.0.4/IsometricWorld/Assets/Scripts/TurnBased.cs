using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TurnBased : MonoBehaviour
{
    public int NumberOfMoves;
    public int MovesMade;

    public int pathLength;

    public int EnemyMoves;
    public GameObject Player;

    
    public static bool PlayerCanMove;
    public bool turnInProgress = false;

    public PlayerController playerController;

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

        
        
        //getting a reference to the PlayerController script
        //GameObject thePlayer = GameObject.Find("Player");
        //PlayerController playerController = thePlayer.GetComponent<PlayerController>();

        
              
    }

    // Update is called once per frame
    void Update()
    {

        MovesMade = playerController.tilesMoved;
        pathLength = AStarPathfinder.instance.myPath.Count - 1;

        switch (currentState)
        {
            


            //CREATE PLANNING - SET PATH CONFIRMED TO FALSE UNITLL BUTTON PRESS. WHEN CONFIRMED MOVE TO PLAYER TURN THEN CHANGE BOOL TO TRUE. TURN OFF ONCE CLOSE TO END NODE OR MOVES LEFT = 0
            //MAYBE TURN OFF RAYCASTING IN PLAYER TURN TO PREVENT CHEATING OF MOVES
            case (GameStates.Planning):

                //Debug.Log("Planning");
                
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

                else if (MovesMade == pathLength)
                {
                    currentState = GameStates.EndTurn;
                    
                }
                break;

            case (GameStates.EndTurn):
                //when the player has run out of turn goes to end turn then staight to enemy turn 
                Debug.Log("End Turn");
                PlayerCanMove = false;
                PlayerController.instance.pathConfirmed = false;


                AStarPathfinder.instance.myPath.Clear();
                
                PlayerController.instance.isRunning = false;

                currentState = GameStates.EnemyTurn;
                break;

            case (GameStates.EnemyTurn):
                //put enemy Ai code in here. or reference the enemy Ai Script
                //currentState = GameStates.PlayerTurn;                             
                Debug.Log("Enemy Turn");

                
                if (EnemyMoves <= 0)
                {
                    currentState = GameStates.Planning;
                    //MovesMade = NumberOfMoves + 1;
                    playerController.tilesMoved = 0;
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

    
}
