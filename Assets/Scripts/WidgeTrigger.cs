using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class WidgeTrigger : MonoBehaviour
{

    public enum Shape
    {
        Wedge,
        Spherical,
        Cone
    }

    public Shape shape;
    public Transform target;

    public float outerRadius = 1;
    public float innerRadius = 1;
    public float height = 1;
    [Range(0, 360)]
    public float fovDeg = 45;
    private float fovRad => fovDeg * Mathf.Deg2Rad;
    private float angleThresh => Mathf.Cos(fovRad / 2);

    void SetGizmoMatrix(Matrix4x4 setMatrix)
    {
        Handles.matrix = Gizmos.matrix = setMatrix;
    }

    void OnDrawGizmos()
    {
        SetGizmoMatrix(transform.localToWorldMatrix);
        Gizmos.color = Handles.color = Contains(target.position) ? Color.white : Color.red;
        switch (shape)
        {
            case Shape.Wedge:
                DrawWedgeGizmo();
                break;
            case Shape.Spherical:
                DrawSphericalGizmo();
                break;
            case Shape.Cone:
                DrawConeGizmo();
                break;
        }
    }

    void DrawWedgeGizmo()
    {
        Vector3 top = new Vector3(0, height, 0);
        float p = angleThresh;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vLeftOuter = new Vector3(-x, 0, p) * outerRadius;
        Vector3 vRighOuter = new Vector3(x, 0, p) * outerRadius;
        Vector3 vLeftInner = new Vector3(-x, 0, p) * innerRadius;
        Vector3 vRightInner = new Vector3(x, 0, p) * innerRadius;

        Handles.DrawWireArc(default, Vector3.up, vLeftOuter, fovDeg, outerRadius);
        Handles.DrawWireArc(top, Vector3.up, vLeftOuter, fovDeg, outerRadius);

        Handles.DrawWireArc(default, Vector3.up, vLeftInner, fovDeg, innerRadius);
        Handles.DrawWireArc(top, Vector3.up, vLeftInner, fovDeg, innerRadius);

        //horizontal line bottom and top
        Gizmos.DrawLine(vLeftInner, vLeftOuter);
        Gizmos.DrawLine(vRightInner, vRighOuter);
        Gizmos.DrawLine(top + vLeftInner, top + vLeftOuter);
        Gizmos.DrawLine(top + vRightInner, top + vRighOuter);

        //vertical line
        Gizmos.DrawLine(vLeftInner, vLeftInner + top);
        Gizmos.DrawLine(vRightInner, vRightInner + top);

        Gizmos.DrawLine(vLeftOuter, vLeftOuter + top);
        Gizmos.DrawLine(vRighOuter, vRighOuter + top);
    }
    private void DrawSphericalGizmo()
    {
        Gizmos.DrawWireSphere(default, innerRadius);
        Gizmos.DrawWireSphere(default, outerRadius);
    }

    private void DrawConeGizmo()
    {
        Vector3 top = new Vector3(0, height, 0);
        float p = angleThresh;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vLeftOuter = new Vector3(-x, 0, p) * outerRadius;
        Vector3 vRighOuter = new Vector3(x, 0, p) * outerRadius;
        Vector3 vLeftInner = new Vector3(-x, 0, p) * innerRadius;
        Vector3 vRightInner = new Vector3(x, 0, p) * innerRadius;

        //arc
        void DrawFlatWedge()
        {
            Handles.DrawWireArc(default, Vector3.up, vLeftInner, fovDeg, innerRadius);
            Handles.DrawWireArc(default, Vector3.up, vLeftOuter, fovDeg, outerRadius);
            Gizmos.DrawLine(vLeftInner, vLeftOuter);
            Gizmos.DrawLine(vRightInner, vRighOuter);
        }

        DrawFlatWedge();
        Matrix4x4 initialMatrix = Gizmos.matrix;
        SetGizmoMatrix(Gizmos.matrix * Matrix4x4.TRS(default, Quaternion.Euler(0, 0, 90), Vector3.one));
        DrawFlatWedge();
        SetGizmoMatrix(initialMatrix);

        //circle
        void Drawring(float turretRadius)
        {
            float angle = fovRad / 2;
            float distance = turretRadius * MathF.Cos(angle);
            float radius = turretRadius * Mathf.Sin(angle);
            Vector3 center = new Vector3(0, 0, distance);
            Handles.DrawWireDisc(center, Vector3.forward, radius);
        }

        Drawring(innerRadius);
        Drawring(outerRadius);
    }

    public bool Contains(Vector3 position) =>
         shape switch
         {
             Shape.Wedge => WedgeContains(position),
             Shape.Spherical => SphereContains(position),
             Shape.Cone => ConeContains(position),
             _ => throw new NotImplementedException()
         };

    bool WedgeContains(Vector3 position)
    {
        Vector3 vectorToTargetWorld = target.position - transform.position;

        Vector3 vecTorToTargetLocal = transform.InverseTransformVector(vectorToTargetWorld);
        //height check
        if (vecTorToTargetLocal.y < 0 || vecTorToTargetLocal.y > height)
            return false;

        Vector3 flatDirectionToTarget = vecTorToTargetLocal;
        flatDirectionToTarget.y = 0;
        float flatDistance = flatDirectionToTarget.magnitude;
        flatDirectionToTarget = flatDirectionToTarget.normalized; ;

        //angular widge check
        if (flatDirectionToTarget.z < angleThresh)
            return false; // outside the Widge

        //cylindrical radial Test
        if (flatDistance > outerRadius || flatDistance < innerRadius)
        {
            return false;// we outside
        }
        return true; //we inside
    }

    bool SphereContains(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        return distance <= outerRadius && distance >= innerRadius;
    }
    bool ConeContains(Vector3 position)
    {
        if (SphereContains(position) == false)
            return false;//out side
        Vector3 dirToTarget = (position - transform.position).normalized;
        float angleRad = Mathf.Acos(Mathf.Clamp(Vector3.Dot(transform.forward, dirToTarget),-1,1));

        return angleRad < fovRad / 2;

    }
}
