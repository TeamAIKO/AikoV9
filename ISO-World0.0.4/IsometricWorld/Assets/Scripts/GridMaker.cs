using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMaker : MonoBehaviour
{
    public int h = 3;
    public int v = 3;
    public Transform player;

    public GameObject tilePrefab;
    private Camera mainCamera;
    public static List<Transform> tiles = new List<Transform>();


    private Transform tileTr;
    private Transform currTileTr;
    private AStarPathfinder path;

    private static GridMaker instance;
    //counting the number of trigger colliders that the player has passed through in order to get the number of moves they have made/remain.
    //public int NumberOfMoves = 0;
    //public int MovesLeft;

    //getting the player so that you can detect when it hits a trigger collider
    //public GameObject Player;
    //public Collider PlayerCollider;


    //variables for the double click 
    private bool oneClick = false;
    private bool timerIsRunning;
    public float timeForDoubleClick;

    //this is how long to allow for double click
    public float delay;

    public void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(instance);
            return;
        }
        instance = this;
    }

    public static GridMaker Instance
    {
        get { return instance; }
    }

    public void Start()
    {
        mainCamera = Camera.main;
        path = this.GetComponent<AStarPathfinder>();

        //CreateTiles();
        GameObject[] tt = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tt)
        {
            tiles.Add(tile.transform);
        }

        SearchAdjacentTiles();

        Debug.Log(tiles.Count);

        //MovesLeft = NumberOfMoves;

        //PlayerCollider = Player.GetComponent<Collider>();
    }

    private void CreateTiles()
    {
        for (int i = 0; i < h; i++)
        {
            for (int z = 0; z < v; z++)
            {
                GameObject tile = (GameObject)Instantiate(tilePrefab, this.gameObject.transform.position + new Vector3(i, 0, -z), Quaternion.Euler(90f, 0, 0));
                tile.transform.parent = this.gameObject.transform;
                tile.GetComponent<Tile>().gridPos = new Vector2(i, z);
                //				tiles.Add(tile.transform);

                RaycastHit hit;
                if (Physics.Raycast(tile.transform.position, Vector3.down, out hit, 100.0f))
                {
                    //Debug.Log(hit.transform.gameObject.layer);

                    tile.transform.position = hit.point + new Vector3(0.0f, 0.01f, 0.0f);

                    if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Walkable")
                    {
                        tiles.Add(tile.transform);
                    }
                    else
                    {
                        Destroy(tile.gameObject);
                    }

                    //if(hit.transform.localScale.y != 0.25f)

                    //{
                    //  Destroy(tile.gameObject);
                    //}
                    //else
                    //{
                    //  tiles.Add(tile.transform);
                    //}

                    /*
					if(hit.transform.localScale.y != 0.25f)
					{
						Destroy(tile.gameObject);
					}
					else
					{
						tiles.Add(tile.transform);
					}
                    */
                }
            }
        }
    }

    public void GetTilesInRange(Tile curr, int range)
    {
        //List<Tile> adj = curr.adjacentNodes;
        //Debug.Log(adj.Count);

        Debug.Log("curr: " + curr.gridPos);
    }

    private void SearchAdjacentTiles()
    {
        foreach (Transform t in tiles)
        {
            Tile tt = t.GetComponent<Tile>();
            Vector2 pos = tt.gridPos;

            //			orthogonal adjacent tiles
            SetAdjacentTile(tt, (int)pos.x, (int)pos.y - 1);
            SetAdjacentTile(tt, (int)pos.x - 1, (int)pos.y);
            SetAdjacentTile(tt, (int)pos.x + 1, (int)pos.y);
            SetAdjacentTile(tt, (int)pos.x, (int)pos.y + 1);

            //			diagonal adjacent tiles
            //SetAdjacentTile(tt, (int)pos.x-1, (int)pos.y+1);
            //SetAdjacentTile(tt, (int)pos.x+1, (int)pos.y-1);
            //SetAdjacentTile(tt, (int)pos.x-1, (int)pos.y-1);
            //SetAdjacentTile(tt, (int)pos.x+1, (int)pos.y+1);
        }
    }

    private void SetAdjacentTile(Tile t, int x, int y)
    {
        Tile tile = GetTile(x, y);
        if (tile != null)
        {
            t.adjacentNodes.Add(tile);
        }
    }

    private Tile GetTile(int x, int y)
    {
        Tile tile = null;
        foreach (Transform t in tiles)
        {
            if (t.GetComponent<Tile>().gridPos.x == x && t.GetComponent<Tile>().gridPos.y == y)
            {
                tile = t.GetComponent<Tile>();
            }
        }
        return tile;
    }

    public void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        tileTr = null;

        if (tiles.Count > 0)
        {
            foreach (Transform tile in tiles)
            {
                if (path.start != tile && path.end != tile)
                {
                    Color c = tile.gameObject.GetComponent<MeshRenderer>().material.color;
                    c = Color.red;
                    c.a = 0.2f;
                    tile.gameObject.GetComponent<MeshRenderer>().material.color = c;
                }
            }
        }



       
        DoubleClick();



        //not optimised! only for debugging
        if (path.myPath != null)
        {
            foreach (Tile t in path.myPath)
            {
                ChangeTileColor(t.transform, Color.blue, 0.2f);
            }
        }

        if (path.start != null)
        {
            ChangeTileColor(path.start.transform, Color.blue, 0.2f);
        }


        if (path.end != null)
        {
            ChangeTileColor(path.end.transform, Color.blue, 0.2f);
        }

        if (tileTr != null)
        {
            ChangeTileAlpha(tileTr, 0.5f);
        }
    }



    private void SearchPath(RaycastHit hit)
    {
        if (path.myPath.Count > 0)
            path.myPath.Clear();

        RaycastHit h;
        if (Physics.Raycast(player.position, Vector3.down, out h, 100f))
        {
            path.start = h.transform.gameObject.GetComponent<Tile>();
        }

        path.end = hit.transform.gameObject.GetComponent<Tile>();

        path.Search();


        player.gameObject.GetComponent<PlayerController>().SetPath(path.myPath);
    }

    private void SetStartAndFinalTiles(RaycastHit hit)
    {
        if (path.start == null)
        {
            path.myPath.Clear();
            path.start = hit.transform.gameObject.GetComponent<Tile>();
            path.end = null;
        }
        else
        {
            if (path.end == null)
            {
                path.end = hit.transform.gameObject.GetComponent<Tile>();

                path.Search();
            }
            else
            {
                path.myPath.Clear();
                path.start = hit.transform.gameObject.GetComponent<Tile>();
                path.end = null;
            }
        }
    }

    private void ChangeTileColor(Transform t, Color color, float a)
    {
        color.a = a;
        t.gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    private void ChangeTileAlpha(Transform t, float alpha)
    {
        Color c = t.gameObject.GetComponent<MeshRenderer>().material.color;
        c.a = alpha;
        t.gameObject.GetComponent<MeshRenderer>().material.color = c;
    }


    public void DoubleClick()
    {
        if (PlayerController.instance.pathConfirmed == true)
        {
            return;

        }
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            tileTr = hit.transform;


            if (Input.GetMouseButtonDown(0))
            {
                if (!oneClick) // first click no previous clicks
                {
                    oneClick = true;

                    timeForDoubleClick = Time.time; // save the current time
                                                    // do one click things;
                }
                else
                {
                    oneClick = false; // found a double click, now reset

                    //do double click things

                    SetStartAndFinalTiles(hit);

                    SearchPath(hit);

                    //confirming the path the enable the gameplay
                    PlayerController.instance.pathConfirmed = true;
                    Debug.Log("Confirmed Path");
                }
            }
            if (oneClick)
            {
                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timeForDoubleClick) > delay)
                {

                    //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.

                    oneClick = false;

                }

            }
        }
    }
}

