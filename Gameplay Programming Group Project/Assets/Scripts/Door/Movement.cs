using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum MovementType
    {
        NONE = 0,
        LINEAR = 1,
        ROTATION = 2,
        ERRATIC = 3
    }

    public MovementType movementType = MovementType.NONE;
    public float speed = 1.0f;

    /// <summary>
    /// First point of movement for the object. Only used by LINEAR movement.
    /// </summary>
    public Vector3 point1;
    /// <summary>
    /// Second point of movement for the object. Only used by LINEAR movement.
    /// </summary>
    public Vector3 point2;
    [System.NonSerialized] public Vector3 center;
    Vector3 target;
    
    /// <summary>
    /// Only used in ERRATIC movement. If empty, it will be random
    /// </summary>
    public float radius = 0;
    bool rand = false;
    /// <summary>
    /// Only used in ERRATIC movement. Lower value on left, higher value on right.
    /// </summary>
    public Vector2 range = new Vector2(20,30);
    float index = 0;
    bool going_towards;

    private void Awake()
    {
        center = gameObject.transform.position;
        SetUp();
    }

    private void FixedUpdate()
    {
        Move();
    }
    void SetUp()
    {
        switch (movementType)
        {
            case (MovementType.LINEAR):
                {
                    target = point1;
                    break;
                }
            case (MovementType.ERRATIC):
                {
                    if (radius == 0)
                    {
                        rand = true;
                    }
                    break;
                }
        }
    }
     void Move()
    {
        switch (movementType)
        {
            case (MovementType.LINEAR):
                {
                    LinearMovement();
                    break;
                }
            case (MovementType.ROTATION):
                {
                    transform.Rotate(Vector3.up, speed * Time.deltaTime);
                    break;
                }
            case (MovementType.ERRATIC):
                {
                    ErraticMovement();
                    break;
                }
        }
    }

    void LinearMovement()
    {
        if (going_towards)
        {
            target = point1;
        }
        else
        {
            target = point2;
        }

        target.x = center.x + target.x;
        target.y = center.y + target.y;
        target.z = center.z + target.z;

        Vector3 direction = target -  transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.1 * speed)
        {
            transform.position = target;
            going_towards = !going_towards;
        }
    }
    void ErraticMovement()
    {
        if (index < 1)
        {
            if (rand)
            {
                radius = Random.Range(1, 10);

                target.x = Random.Range(-radius, radius);
                target.y = Random.Range(-radius, radius);
                target.z = Random.Range(-radius, radius);
            }
            else
            {
                float[] choice = new float [2];
                choice[0] = -radius;
                choice[1] = radius;
                int lenght = choice.Length;
                int randIndex;
                randIndex = Random.Range(0, lenght);
                target.x = choice[randIndex];
                randIndex = Random.Range(0, lenght);
                target.y = choice[randIndex];
                randIndex = Random.Range(0, lenght);
                target.z = choice[randIndex];
            }

            target.x = center.x + target.x;
            target.y = center.y + target.y;
            target.z = center.z + target.z;

            transform.position = target;

            index = Random.Range((int)range.x, (int)range.y);
        }
        else
        {
            index -= Time.deltaTime;
            Vector3 direction = center - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, center) < 0.05 * speed)
            {
                /*transform.position = target;*/
            }
        }
    }
}
