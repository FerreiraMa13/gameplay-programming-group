using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurvedLine))]
public class CurvedLineInspector : Editor
{
	private CurvedLine curve;
	private Transform handleTransform;
	private Quaternion handleRotation;
	private const int lineSteps = 10;
	private const float directionScale = 0.5f;

	private void OnSceneGUI()
	{
		curve = target as CurvedLine;
		handleTransform = curve.transform;

		handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

		Vector3 p0 = ShowPoint(0);
		Vector3 p1 = ShowPoint(1);
		Vector3 p2 = ShowPoint(2);
		Vector3 p3 = ShowPoint(3);

		Handles.color = Color.grey;
		Handles.DrawLine(p0, p1);
		Handles.DrawLine(p2, p3);

		ShowDirections();
		Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

		Handles.color = Color.white;
		Vector3 lineStart = curve.GetPoint(0f);
		Handles.color = Color.green;
		Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f));
		for (int i = 1; i <= lineSteps; i++)
		{
			Vector3 lineEnd = curve.GetPoint(i / (float)lineSteps);
			Handles.color = Color.white;
			Handles.DrawLine(lineStart, lineEnd);
			Handles.color = Color.green;
			Handles.DrawLine(lineEnd, lineEnd + curve.GetVelocity(i / (float)lineSteps));
			lineStart = lineEnd;
		}

		Handles.DoPositionHandle(p0, handleRotation);
		Handles.DoPositionHandle(p1, handleRotation);
		Handles.DoPositionHandle(p2, handleRotation);

		/*EditorGUI.BeginChangeCheck();
		startPoint = Handles.DoPositionHandle(startPoint, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			line.startPoint= handleTransform.InverseTransformPoint(startPoint);
		}
		EditorGUI.BeginChangeCheck();
		endPoint = Handles.DoPositionHandle(endPoint, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			line.endPoint= handleTransform.InverseTransformPoint(endPoint);
		}*/
	}
	
	private Vector3 ShowPoint(int index)
	{
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		EditorGUI.BeginChangeCheck();
		point = Handles.DoPositionHandle(point, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(curve, "Move Point");
			EditorUtility.SetDirty(curve);
			curve.points[index] = handleTransform.InverseTransformPoint(point);
		}
		return point;
	}

	private void ShowDirections()
	{
		Handles.color = Color.green;
		Vector3 point = curve.GetPoint(0f);
		Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
		for (int i = 1; i <= lineSteps; i++)
		{
			point = curve.GetPoint(i / (float)lineSteps);
			Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
		}
	}

}
