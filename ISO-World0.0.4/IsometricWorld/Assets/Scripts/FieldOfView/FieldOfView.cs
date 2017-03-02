using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour
{
    //how far the enemy can see
    public float viewRadius;

    //sets the angle in which the enemy can see the player
    [Range(0, 360)]
    public float viewAngle;

    //assign what will block the raycast and what it will register
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool PlayerSpotted = false;
    //location of the player
    public Transform visibleTarget;

    public static FieldOfView instance;

    void Start()
    {
        instance = this;
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }

    //finding the target if it is in view range. goes view range -> angle of view -> raycast to dect if object is in the way
    void FindVisibleTarget()
    {
        PlayerSpotted = false;
        visibleTarget = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    //do stuff in here. need to limit the rotation of the field of view to every 90 degrees that the enemy turns.                    
                    visibleTarget = target;
                    PlayerSpotted = true;                    
                }
            }
        }

    }

    //create the direction at which the angle is calculated. Ie.forward of the enemy unit
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
            
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
}
