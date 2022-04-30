using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : DoorAnimator
{
    public Enums.Directions openDirection;
    public float displacement;
    public float openingSpeed;

    Vector3 destinationPoint;
    Vector3 startingPoint;

    private void Awake()
    {
        type = DoorOpeningAnimation.SLIDING;
    }
    private void Start()
    {
        Vector3 temp_direction = Vector3.zero;

        switch(openDirection)
        {
            case Enums.Directions.UP:
                {
                    temp_direction = transform.up;
                    break;
                }
            case Enums.Directions.DOWN:
                {
                    temp_direction = -transform.up;
                    break;
                }
            case Enums.Directions.RIGHT:
                {
                    temp_direction = transform.right;
                    break;
                }
            case Enums.Directions.LEFT:
                {
                    temp_direction = -transform.right;
                    break;
                }
        }

        if(openDirection != Enums.Directions.NONE)
        {
            findDestination(temp_direction);
        }

        startingPoint = transform.position;
    }
    public override void OpeningMovement()
    {
        Vector3 direction = destinationPoint - transform.position;
        transform.Translate(direction.normalized * openingSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destinationPoint) < 0.06 * openingSpeed)
        {
            transform.position = destinationPoint;
            status = DoorStatus.OPEN;
            Debug.Log("Open");
            if(GetComponent<Movement>())
            {
                GetComponent<Movement>().center = transform.position;
            }
        }
    }
    public override void ClosingMovement()
    {
        Vector3 direction = startingPoint - transform.position;
        transform.Translate(direction.normalized * openingSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startingPoint) < 0.06 * openingSpeed)
        {
            transform.position = startingPoint;
            status = DoorStatus.CLOSED;
            Debug.Log("Close");
        }
    }
    void findDestination(Vector3 direction)
    {
        destinationPoint = transform.position + displacement * direction;
    }
}
