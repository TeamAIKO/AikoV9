using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //changed from private to public for a test
    public List<Tile> path;
    private int curr = 0;

    public static PlayerController instance;
    public bool pathConfirmed = false;
    public bool isRunning = false;

    public Animator animator;

    private TurnBased turn;
    public int tilesMoved = 0;

    //variables for the double click 
    private bool oneClick = false;
    private bool timerIsRunning;
    public float timeForDoubleClick;

    //this is how long to allow for double click
    public float delay;

   
    public void SetPath(List<Tile> p)
    {
        curr = 0;
        path = p;
    }

    public void Start()
    {
        instance = this;

        turn = this.gameObject.GetComponent<TurnBased>();

    }



    public void Update()
    {
        //find the players tile so that the AI can use it like a waypoint
        

        if (path == null || path.Count == 0)
            return;


        isRunning = true;


        //if (!pathConfirmed || curr > path.Count - 1)
        if (curr > path.Count - 1)
        {

            isRunning = false;
            animator.SetBool("run", isRunning);
            return;
        }


        if (TurnBased.PlayerCanMove == false)
        {
            return;
       
        }

        else
        {
            //moves the player
            Vector3 toTarget = path[curr].gameObject.transform.position - this.transform.position;
            Quaternion rot = Quaternion.LookRotation(new Vector3(toTarget.x, 0, toTarget.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10.0f);

            transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);

            animator.SetBool("run", isRunning);
        }


        Vector3 target = path[curr].gameObject.transform.position;
        if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.1f)
        {
            curr++;

            if (curr > 1)
                tilesMoved++;
        }
    }
}
