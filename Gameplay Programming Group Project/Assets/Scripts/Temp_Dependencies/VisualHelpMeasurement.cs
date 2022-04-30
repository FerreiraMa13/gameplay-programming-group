using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualHelpMeasurement : MonoBehaviour
{
    public bool pointBased = true;
    public Vector3 startingPoint = Vector3.zero;
    public float distance = 0;
    public Enums.Axis axis = Enums.Axis.XAXIS;
    private Vector3 position = Vector3.zero;

    private void OnValidate()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        if (!pointBased)
        {
            startingPoint = Vector3.zero;
        }
        position = startingPoint;
        switch (axis)
        {
            case (Enums.Axis.XAXIS):
                position.x += distance;
                break;
            case (Enums.Axis.YAXIS):
                position.y += distance;
                break;
            case (Enums.Axis.ZAXIS):
                position.z += distance;
                break;
        }


        if (gameObject != null)
        {
            position.y -= (transform.localScale.y - 0.2f);
            transform.localPosition = position;
        }
    }

}
