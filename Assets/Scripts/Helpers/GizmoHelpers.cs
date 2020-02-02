using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoHelpers
{
    private const float GIZMO_DISK_THICKNESS = 0.01f;
    public static void DrawGizmoDisk(Vector3 position, Quaternion rotation, float radius, Color gizmoColor)
    {
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.color = gizmoColor;
        Gizmos.matrix = Matrix4x4.TRS(position, rotation, new Vector3(1, GIZMO_DISK_THICKNESS, 1));
        Gizmos.DrawSphere(Vector3.zero, radius);
        Gizmos.matrix = oldMatrix;
    }
}
