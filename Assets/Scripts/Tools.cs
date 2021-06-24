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
}
