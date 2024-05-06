using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public WidgeTrigger trigger;
    public Transform gunTransform;
    public float smoothingFactor = 8f;

    Quaternion targetRotation;

    void Update()
    {
        if (trigger.Contains(target.position))
        {
            Vector3 vectorToTarget = target.position - gunTransform.position;

             targetRotation = Quaternion.LookRotation(vectorToTarget, transform.up);
        }
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, targetRotation, smoothingFactor * Time.deltaTime);
    }
}
