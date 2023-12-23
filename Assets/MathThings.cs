using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathThings : MonoBehaviour
{
    public Transform A;
    public Transform B;

    public float scProj;

    private void OnDrawGizmos()
    {
        Vector2 a = A.position;
        Vector2 b = B.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(default, a);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(default, b);

        //Normalize A and draw it

        //Manual version of Normalizing
        //(for optmisation)\\ float alen = Mathf.Sqrt(a.x * a.x * a.y * a.y);
        //for readerbility\\ float alen = a.magnitude;
        //Vector2 aNorm = a / alen;

        //Quick version of Normalizing
        Vector2 aNorm = a.normalized;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(aNorm, 0.05f);

        //Scaler Projection
        scProj = Vector2.Dot(aNorm, b);

        //Vector Projection
        Vector2 vecProj = aNorm * scProj;

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(vecProj, 0.05f);


        //Gizmos.DrawSphere(transform.position, 1f);
    }
}
