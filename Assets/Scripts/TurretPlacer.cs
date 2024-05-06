using System;
using UnityEngine;

public class TurretPlacer : MonoBehaviour {

	public Transform turret;

	public float pitchDeg;
	public float yawDeg;

	public float mouseSensitivity = 1f;
    public float turretYawSensitivity = 6;
    float turretYawOffset;

    private void Awake()
    {
        Vector3 startEuler = transform.eulerAngles;
        pitchDeg = startEuler.x;
        yawDeg = startEuler.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        PlaceTurret();
        MouseLook();
        UpdatTurretYawInput();
    }

    private void UpdatTurretYawInput()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        turretYawOffset += scrollDelta * turretYawSensitivity;
        turretYawOffset = Mathf.Clamp(turretYawOffset, -90, 90);
    }

    private void MouseLook()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;
        float xDelta = Input.GetAxis("Mouse X");
        float yDelta = Input.GetAxis("Mouse Y");

        pitchDeg += -yDelta * mouseSensitivity;
        pitchDeg = Mathf.Clamp(pitchDeg, -89, 89);
        yawDeg += xDelta * mouseSensitivity;
        transform.rotation = Quaternion.Euler(pitchDeg, yawDeg, 0);
    }

    void PlaceTurret() {
		Ray ray = new Ray( transform.position, transform.forward );
		if( Physics.Raycast( ray, out RaycastHit hit ) ) {
			turret.position = hit.point;
			Vector3 yAxis = hit.normal;
			Vector3 zAxis = Vector3.Cross( transform.right, yAxis ).normalized;
			Debug.DrawLine( ray.origin, hit.point, new Color( 1, 1, 1, 0.4f ) );

            Quaternion OffsetRot = Quaternion.Euler(0, turretYawOffset, 0);
			turret.rotation = Quaternion.LookRotation( zAxis, yAxis ) * OffsetRot;
		}
	}

}