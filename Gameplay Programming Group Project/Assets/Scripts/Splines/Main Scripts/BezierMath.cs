 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMath : MonoBehaviour
{
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		t = Mathf.Clamp01(t);
		float minusT = 1f - t;
		return
			minusT * minusT * p0 +
			2f * minusT * t * p1 +
			t * t * p2;
	}
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		return
			2f * (1f - t) * (p1 - p0) +
			2f * t * (p2 - p1);
	}
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float minusT = 1f - t;
		return
			minusT * minusT * minusT * p0 +
			3f * minusT * minusT * t * p1 +
			3f * minusT * t * t * p2 +
			t * t * t * p3;
	}
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float minusT = 1f - t;
		return
			3f * minusT * minusT * (p1 - p0) +
			6f * minusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}
	public static Vector3 GetProgress(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 point)
    {
		//(Vector)(whatever) + (Vector2)(morewhatever) + (Vector3)(evenmorewhatever) = A Point,
		//then vector.x(whatever) + vector2.x(whatever) + vector3.x(whatever) = point.x
		return Vector3.zero;
    }
}
