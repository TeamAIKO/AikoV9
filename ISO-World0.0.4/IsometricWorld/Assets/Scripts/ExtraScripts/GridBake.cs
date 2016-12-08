using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridBake : MonoBehaviour 
{
    //how maximum amount of tiles that will be created ie a 10x10 square
	public int totalHorizontalTiles = 10;
	public int totalVerticalTiles = 10;

    //the amount that will be created each time a time the bake button is pressed ie a 4X4 square
	public int bakingStep = 4;

    //finding the prefab for the tile to be created
	public GameObject tilePrefab;
	public List<GameObject> tiles = new List<GameObject>();

	private int stepX = 0;
	private int stepY = 0;

	public void StartBaking()
	{

		for(int i = stepX ; i < stepX+bakingStep ; i++)
		{
			for(int z = stepY ; z < stepY+bakingStep ; z++)
			{
				GameObject tile = (GameObject)Instantiate(tilePrefab, this.gameObject.transform.position + new Vector3(i, 0, -z), Quaternion.Euler(90f, 0, 0));
				tile.transform.parent = this.gameObject.transform;
				tile.GetComponent<Tile>().gridPos = new Vector2(i, z);
				tile.tag = "Tile";

                
				RaycastHit hit;
				if(Physics.Raycast(tile.transform.position, Vector3.down, out hit, 100.0f))
				{
					tile.transform.position = hit.point + new Vector3(0.0f, 0.01f, 0.0f);

                    tiles.Add(tile);

                    //destroys the tile if the ray hits an abject that isnt on the layer walkable
                    if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Walkable"))
                    {
                        DestroyImmediate(tile.gameObject);
                    }
                    /*
					if(hit.transform.localScale.y != 0.25f)
					{
                        
						tile.GetComponent<Tile>().walkable = false;
						DestroyImmediate(tile.gameObject);
                        Debug.Log("destroying tile");
					}
					else
					{
//						tiles.Add(tile);
					}
                    */
                }
                
			}
		}
			
		stepX += bakingStep;
//		Debug.Log("stepX: " + stepX);

		if(stepX >= totalHorizontalTiles)
		{
			stepX = 0;
			stepY += bakingStep;
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
