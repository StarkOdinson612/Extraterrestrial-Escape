using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPatrol : MonoBehaviour
{
    private CameraStateManager stateManager;

    public bool isRotating;

    [SerializeField]
    private float targetPoint;

    public float speed;

    public float angle;

    private float followSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = GetComponent<CameraStateManager>();

        targetPoint = angle;
        
        if (!isRotating)
        {
            angle = 0;
            targetPoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale == 0) { return; }
		CameraState state = stateManager.getState();
        if (state == CameraState.PATROLLING)
        {
            GetComponent<CameraVision>().setEnemyLight(3);
            if (isRotating)
            {

                if (targetPoint == angle)
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + 1 * speed);

                    if (transform.localEulerAngles.z >= (angle - 0.5f) && transform.localEulerAngles.z <= 180)
                    {

                        targetPoint = 360 - angle;
                    }
                }
                else if (targetPoint == 360 - angle)
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z - 1 * speed);

                    if (transform.localEulerAngles.z <= (360 - angle) - 0.5f && transform.localEulerAngles.z >= 180)
                    {

                        targetPoint = angle;
                    }
                }
            }
            else
            {
                if (transform.eulerAngles.z >= 0.5f && transform.eulerAngles.z < 180)
                {
					transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z - 1 * speed);
				}
                else if (transform.eulerAngles.z <= 359.5f && transform.eulerAngles.z > 180)
                {
					transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + 1 * speed);
				}

			}
            
        }
        else if (state == CameraState.DISCOVERED_PLAYER || state == CameraState.CAUGHT_PLAYER)
        {
            float dirAngle = GetAngleFromVectorFloat(stateManager.getPlayerPos().position - transform.position);

            Quaternion rotDir = Quaternion.Euler(new Vector3(0, 0, dirAngle - transform.rotation.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, followSpeed * Time.deltaTime);
        }
    }

    public void toggleFollowSpeed()
    {
        followSpeed = 4;
    }


    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

    private float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        Vector3 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector3.Angle(Vector3.right, diference) * sign - 90;
    }
}
