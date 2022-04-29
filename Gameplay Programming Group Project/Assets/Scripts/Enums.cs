using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum Directions
    {
        NONE = 0,
        UP = 1,
        RIGHT = 2,
        DOWN = 3,
        LEFT = 4
    }
    public enum Axis
    {
        NONE = 0,
        XAXIS = 1,
        YAXIS = 2,
        ZAXIS = 3
    }

    public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored
    }
}
