using UnityEngine;
using System.Collections;

public class NPCTalk : MonoBehaviour {

    public float talkDistance;
    public GameObject Player;
    public GameObject PopPanel;
    public bool Open;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public void Update()
    {
        if (Open == false)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) <= talkDistance)
            {
                Open = true;
                PopPanel.gameObject.SetActive(true);
                Debug.Log("Dialog pop up");
            }
        }

    }

    public void OnClick()
    {
        
        PopPanel.gameObject.SetActive(false);
        Open = false;
       
   
    }

   
}

