using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int maxBounces = 15;
    public void OnDrawGizmos()
    {
        Vector2 dir = transform.right;
        Vector2 origin = transform.position;

        Ray ray = new Ray(origin, dir);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(origin, dir + origin);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin, hitInfo.point);
                Vector2 reflected = Reflect(ray.direction, hitInfo.normal);
                Gizmos.DrawLine(hitInfo.point, (Vector2)hitInfo.point + reflected);
                ray.direction = reflected;
                ray.origin = hitInfo.point;
            }
            else
            {
                break;
            }
        }
      

    }

    Vector2 Reflect(Vector2 inDir, Vector2 groundNormal)
    {
        float proj = Vector2.Dot(inDir, groundNormal);
        return inDir - 2 * proj * groundNormal;
    }
}
