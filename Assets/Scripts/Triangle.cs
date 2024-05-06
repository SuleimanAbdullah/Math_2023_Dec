using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{

    public Transform aTF;
    public Transform bTF;
    public Transform cTF;
    public Transform pTF;

    private void OnDrawGizmos()
    {
        Vector2 a = aTF.position;
        Vector2 b = bTF.position;
        Vector2 c = cTF.position;
        Vector2 pt = pTF.position;
        Gizmos.DrawSphere(pt, 0.02f);

        Gizmos.color = TriangleContains(a, b, c, pt) ? Color.white : Color.red;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, a);
    }

    public float Wedge(Vector2 a ,Vector2 b)
    {
        float proj = a.x * b.y - a.y * b.x;
        return proj;
    }
    bool TriangleContains(Vector2 a, Vector2 b, Vector2 c, Vector2 pt)
    {
        bool ab = GetSide(a, b,pt);
        bool bc = GetSide(b, c, pt);
        bool ca = GetSide(c, a, pt);

        return ab == bc && bc == ca;
    }

    bool GetSide( Vector2 a, Vector2 b ,Vector2 pt)
    {
        Vector2 sideVect = b - a;
        Vector2 relVect = pt - a;
        float proj = Wedge(relVect, sideVect);
        return  proj >0;
    }
}
