using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour
{
    public static SphereScript instance;

    //public float rayLength = 5f;
    // Use this for initialization

    public Tile walkableTile;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, dwn, out hit,SphereManager.instance.RayLength))
        {
            walkableTile = hit.transform.gameObject.GetComponent<Tile>();
        }

        Debug.DrawRay(this.transform.position, dwn * SphereManager.instance.RayLength, Color.red);
    }
}
