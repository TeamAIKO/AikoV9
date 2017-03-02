using UnityEngine;
using System.Collections;

public class SphereManager : MonoBehaviour
{

    public static SphereManager instance;
    public Tile[] StoredTiles;
    public Transform ObjectToFollow;
    public float RayLength = 2.0f;
    public float SphereHeight = 2.0f;
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = new Vector3(ObjectToFollow.position.x, ObjectToFollow.transform.position.y + SphereHeight, ObjectToFollow.transform.position.z);

    }
}
