using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
	private List<Tile> path;
	private int curr = 0;
    public static PlayerController instance;
    public bool pathConfirmed = false;
    public bool isRunning = false;
    public Animator animator;
    
    public void SetPath(List<Tile> p)
	{
		curr = 0;
		path = p;      
    }

    public void Start()
    {
        instance = this;
    }
	public void Update()
	{
		if(path == null || path.Count == 0 )
			return;

        isRunning = true;

        if (!pathConfirmed || curr > path.Count - 1)
        {
            isRunning = false;
            animator.SetBool("run", isRunning);
            return;
        }
  

		Vector3 toTarget = path[curr].gameObject.transform.position - this.transform.position;
		Quaternion rot = Quaternion.LookRotation(new Vector3(toTarget.x, 0, toTarget.z));
		transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10.0f);

		transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);

        animator.SetBool("run", isRunning);



		Vector3 target = path[curr].gameObject.transform.position;
		if(Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.1f)
		{
			curr++;
		}
	}
}
