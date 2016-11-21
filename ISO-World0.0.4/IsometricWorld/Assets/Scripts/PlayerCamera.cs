using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{  
    public Vector3 distance;
    public GameObject targetObject;

    private float targetAngle = 0;
    const float rotationAmount = 1.5f;

    public float rDistance = 1.0f;
    public float rSpeed = 1.0f;

    private bool isRotating = false;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LockPosition();
        RotateButtons();            
    }


    //locks the camera onto the player transform
    void LockPosition()
   {
        if (!isRotating)
        {
            Vector3 pos = targetObject.transform.position;
            pos += distance;
            transform.position = pos;
        }     
   }
 
    void RotateButtons()
    {
        // Trigger functions if Rotate is requested
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetAngle -= 90.0f;
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {      
            targetAngle += 90.0f;
        }

        if (targetAngle != 0)
        {
            isRotating = true;
            Rotate();            
        }
        else
        {
            distance = this.transform.position - targetObject.transform.position;
            isRotating = false;
        }
    }
   
    //makes camera rotate around the targeted object
     void Rotate()
    {             
        if (targetAngle > 0)
        {
            transform.RotateAround(targetObject.transform.position, Vector3.up, -rotationAmount);
            targetAngle -= rotationAmount;
        }
        else if (targetAngle < 0)
        {
            transform.RotateAround(targetObject.transform.position, Vector3.up, rotationAmount);
            targetAngle += rotationAmount;
        }
    }
    
}
