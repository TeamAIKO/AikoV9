using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour
{
    public float rayLength = 5f;
    // Use this for initialization

    public Tile walkableTile;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, dwn, out hit, rayLength))
        {
            walkableTile = hit.transform.gameObject.GetComponent<Tile>();
        }

        Debug.DrawRay(this.transform.position, dwn * rayLength, Color.red);
    }
}
