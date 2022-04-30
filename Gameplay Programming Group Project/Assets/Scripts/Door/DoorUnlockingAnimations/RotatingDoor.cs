using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : DoorAnimator
{

    public Vector3 pivotPoint = Vector3.zero;
    public Enums.Axis rotationAxis = Enums.Axis.NONE;
    public bool transformRelativeRotation = true;
    public bool openingForward = true;
    float openingMag = 1.0f;
    Vector3 rotationVector = Vector3.zero;
    public float rotationSpeed = 1.0f;
    public float maxRotation = 80.0f;
    float rotationAmount = 0.0f;
    GameObject hinge = null;
    private void Awake()
    {
        type = DoorOpeningAnimation.ROTATING;
        hinge = new GameObject();
        hinge.name = "DoorHinge";
        SetUp();
    }

    public void SetUp()
    {
        pivotPoint.x = transform.position.x + pivotPoint.x;
        pivotPoint.y = transform.position.y + pivotPoint.y;
        pivotPoint.z = transform.position.z + pivotPoint.z;

        hinge.transform.position = pivotPoint;
        hinge.transform.rotation = transform.rotation;
        hinge.transform.SetParent(transform.parent, true);
        transform.SetParent(hinge.transform, true);
        if(!openingForward)
        {
            openingMag = -1.0f;
        }
        switch (rotationAxis)
        {
            case (Enums.Axis.XAXIS):
            {
                    if(transformRelativeRotation)
                    {
                        rotationVector = transform.up;
                    }
                    else
                    {
                        rotationVector = Vector3.up;
                    }
                break;
            }
            case (Enums.Axis.YAXIS):
                {
                    if (transformRelativeRotation)
                    {
                        rotationVector = transform.right;
                    }
                    else
                    {
                        rotationVector = Vector3.right;
                    }
                    break;
                }
            case (Enums.Axis.ZAXIS):
                {
                    if (transformRelativeRotation)
                    {
                        rotationVector = transform.forward;
                    }
                    else
                    {
                        rotationVector = Vector3.forward;
                    }
                    break;
                }
        }
    }
    public override void OpeningMovement()
    {
        hinge.transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime * openingMag);
        rotationAmount += rotationSpeed * Time.deltaTime;
        if (rotationAmount > maxRotation)
        {
            float difference = rotationAmount - maxRotation;
            rotationAmount = maxRotation;
            hinge.transform.Rotate(rotationVector, difference * -openingMag);
            rotationAmount = 0.0f;
            status = DoorStatus.OPEN;
        }
    }
    public override void ClosingMovement()
    {
        hinge.transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime * -openingMag);
        rotationAmount += rotationSpeed * Time.deltaTime;
        if (rotationAmount > maxRotation)
        {
            float difference = rotationAmount - maxRotation;
            rotationAmount = maxRotation;
            hinge.transform.Rotate(rotationVector,  difference * -openingMag);
            rotationAmount = 0.0f;
            status = DoorStatus.CLOSED;
        }
    }
}
