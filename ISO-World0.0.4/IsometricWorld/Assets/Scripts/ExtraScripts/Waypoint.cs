using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour 
{
	public Tile tile;

    //changed from Start To Update
	public void Update()
	{
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
		{
			tile = hit.transform.gameObject.GetComponent<Tile>();
		}
	}
}
