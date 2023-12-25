using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggered : MonoBehaviour
{

    public Transform A;
    public Transform B;
    private float scProj;
    private void OnDrawGizmos()
    {
        if (A == null || B == null)
        {
            return;
        }
        Vector2 a = A.position;
        Vector2 b = B.position;

        Gizmos.color = Color.red;
        Vector2 bNorm = b.normalized;
        scProj = Vector2.Dot(bNorm, a);
        Gizmos.DrawLine(a, b);
        if (scProj < bNorm.magnitude)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(a,bNorm.magnitude);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(a, bNorm.magnitude);
        }
       
    }
}
