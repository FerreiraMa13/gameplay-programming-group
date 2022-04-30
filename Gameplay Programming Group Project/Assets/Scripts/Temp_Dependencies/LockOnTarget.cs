using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    public Vector3 cameraPoint = new Vector3(0.0f,0.0f,0.0f);
    [System.NonSerialized]  public Vector3 lookAtPoint;

    public void OnValidate()
    {
        if(cameraPoint == null)
        {
            cameraPoint = new Vector3(0.0f, 0.0f, 0.0f);
        }
        lookAtPoint = transform.position + cameraPoint;
    }
}
