using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    public Vector2 worldCoord;
    public Vector2 localCoord;
   
    void OnDrawGizmos()
    {
        //Vector2 worldPos = LocalToWorld(localCoord);
         localCoord = WorldToLocal(worldCoord);
        Gizmos.DrawSphere(worldCoord, 0.1f);
    }

    Vector2 WorldToLocal(Vector2 world)
    {
        Vector2 relativeVector = world - (Vector2)transform.position;
        float x = Vector2.Dot(relativeVector, transform.right);
        float y = Vector2.Dot(relativeVector, transform.up);

        return new Vector2(x,y);
    }

    Vector2 LocalToWorld(Vector2 local)
    {
        Vector2 position = transform.position;

        position += local.x * (Vector2)transform.right;
        position += local.y * (Vector2)transform.up;

        return position;
    }
}
