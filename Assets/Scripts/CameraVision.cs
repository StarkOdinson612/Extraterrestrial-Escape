using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraVision : MonoBehaviour
{
    private CameraStateManager stateManager;

    private float sightDist = 11f;

    public float hangTime = 3f;

	public Light2D enemyLight;

    private GameManager gameManager;



	[SerializeField]
    private LayerMask playerMask;

    public Transform origin;

    int viewCounter;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = GetComponentInParent<CameraStateManager>();
        playerMask = LayerMask.GetMask("Player", "Default");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
		if (Time.timeScale == 0) { return; }
		CameraState state = stateManager.getState();
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        for (float angle = -20f; angle <= 20f; angle += 4)
        {
            Vector3 vec = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            Debug.DrawRay(origin.position, transform.localRotation * vec.normalized * sightDist, Color.cyan, 0.01f);

            hits.Add(Physics2D.Raycast(origin.position, transform.localRotation * vec.normalized * sightDist * 2, 7, playerMask));
        }

  

        hits = hits.Where(hit => hit.collider != null && hit.collider.gameObject.CompareTag("Player")).ToList();

        if (hits.Count > 0)
        { 
            if (state != CameraState.CAUGHT_PLAYER) { stateManager.setState(CameraState.DISCOVERED_PLAYER); }
            stateManager.setPlayerPos(hits[0].collider.gameObject.transform);

            viewCounter++; 
            Debug.Log(viewCounter / 60);

			if (state != CameraState.CAUGHT_PLAYER) { 
                setEnemyLight(3 + 0.5f * (viewCounter / 180.0f));
                gameManager.setDangerFill(viewCounter / 180f);
            }

			if (viewCounter / 60f > hangTime)
            {
                stateManager.setState(CameraState.CAUGHT_PLAYER);
                GetComponent<CameraPatrol>().toggleFollowSpeed();
            }
        }
        else { 
            viewCounter = 0;
            if (state != CameraState.CAUGHT_PLAYER) 
            { 
                stateManager.setState(CameraState.PATROLLING);
            } 
        }
    }

    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos(angleInDegree * Mathf.Deg2Rad), Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0);
    }

	public void setEnemyLight(float intensity) { enemyLight.intensity = intensity; }


}
