using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridBake : MonoBehaviour 
{
	public int totalHorizontalTiles = 10;
	public int totalVerticalTiles = 10;
	public int bakingStep = 4;
	public GameObject tilePrefab;
//	public List<GameObject> tiles = new List<GameObject>();

	private int stepX = 0;
	private int stepY = 0;

	public void StartBaking()
	{

		for(int i = stepX ; i < stepX+2 ; i++)
		{
			for(int z = stepY ; z < stepY+2 ; z++)
			{
				GameObject tile = (GameObject)Instantiate(tilePrefab, this.gameObject.transform.position + new Vector3(i, 0, -z), Quaternion.Euler(90f, 0, 0));
				tile.transform.parent = this.gameObject.transform;
				tile.GetComponent<Tile>().gridPos = new Vector2(i, z);
				tile.tag = "Tile";

				RaycastHit hit;
				if(Physics.Raycast(tile.transform.position, Vector3.down, out hit, 100.0f))
				{
					tile.transform.position = hit.point + new Vector3(0.0f, 0.01f, 0.0f);

					if(hit.transform.localScale.y != 0.25f)
					{
                        
						tile.GetComponent<Tile>().walkable = false;
						DestroyImmediate(tile.gameObject);
					}
					else
					{
//						tiles.Add(tile);
					}
				}
			}
		}
			
		stepX += 2;
//		Debug.Log("stepX: " + stepX);

		if(stepX >= totalHorizontalTiles)
		{
			stepX = 0;
			stepY += 2;
		}
	}

	public void ClearTiles()
	{
		stepX = stepY = 0;

		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		foreach(GameObject tile in tiles)
		{
			DestroyImmediate(tile);
		}

	}

//	private void BakeCurrentStep()
//	{
//		currentStep++;
//		Debug.Log("Baking step: " + currentStep);
//	}

}
