using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour 
{
	public Tile tile;

	public void Start()
	{
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
		{
			tile = hit.transform.gameObject.GetComponent<Tile>();
		}
	}
}
