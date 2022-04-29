using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
	private void OnSceneGUI()
	{
		Line line = target as Line;
		Transform handleTransform = line.transform;

		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

		Vector3 startPoint = handleTransform.TransformPoint(line.startPoint);
		Vector3 endPoint = handleTransform.TransformPoint(line.endPoint);

		Handles.color = Color.white;
		Handles.DrawLine(line.startPoint, line.endPoint);
		Handles.DoPositionHandle(startPoint, handleRotation);
		Handles.DoPositionHandle(endPoint, handleRotation);

		EditorGUI.BeginChangeCheck();
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
		}
	}


}
