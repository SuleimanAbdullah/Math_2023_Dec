using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggered : MonoBehaviour
{

    public Transform enemy;

    public float radius = 1f;
    private void OnDrawGizmos()
    {
        Vector3 barrelPos = transform.position;
        if (enemy == null)
        {
            return;
        }
        Vector3 enemyPos = enemy.position;
        
        float distance = Vector3.Distance(enemyPos, barrelPos);

        bool inside = distance <= radius;
        Gizmos.color = inside ? Color.green : Color.red;
        Gizmos.DrawWireSphere(barrelPos, radius);
    }
}
