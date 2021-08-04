using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static Vector3 GetRayPlaneIntersectionPoint (Vector3 planeOrigin, Vector3 planeNormal, Ray ray)
    {
        float planeOriginDot = Vector3.Dot (planeNormal, planeOrigin);
        float rayOriginDot = Vector3.Dot (-planeNormal, ray.origin);
        float rayDirectionDot = Vector3.Dot (planeNormal, ray.direction);

        float targetMagnitude = (planeOriginDot + rayOriginDot) / rayDirectionDot;

        return ray.origin + ray.direction * targetMagnitude;
    }

    public static Vector3 GetRayPlaneIntersectionPoint (Vector3 planeOrigin, Vector3 planeNormal, Vector3 rayOrigin, Vector3 rayDirection)
    {
        float planeOriginDot = Vector3.Dot (planeNormal, planeOrigin);
        float rayOriginDot = Vector3.Dot (-planeNormal, rayOrigin);
        float rayDirectionDot = Vector3.Dot (planeNormal, rayDirection);

        float targetMagnitude = (planeOriginDot + rayOriginDot) / rayDirectionDot;

        return rayOrigin + rayDirection * targetMagnitude;
    }

    public static void DrawBox (Vector3 center, Vector2 size, Vector3 rotation, Color colour)
    {
        Vector3[] corners = GetCorners (center, size, rotation);
        Debug.DrawLine (corners[0], corners[1], colour);
        Debug.DrawLine (corners[1], corners[2], colour);
        Debug.DrawLine (corners[2], corners[3], colour);
        Debug.DrawLine (corners[3], corners[0], colour);
    }

    public static Vector3[] GetCorners (Vector3 center, Vector2 size, Vector3 rotation)
    {
        Vector3[] corners = new Vector3[4];
        Vector3 up = Quaternion.Euler (rotation) * new Vector3 (0, 1, 0) * size.y / 2;
        Vector3 right = Quaternion.Euler (rotation) * new Vector3 (1, 0, 0) * size.x / 2;

        corners[0] = center - up - right;
        corners[1] = center - up + right;
        corners[2] = center + up + right;
        corners[3] = center + up - right;

        return corners;
    }
}
