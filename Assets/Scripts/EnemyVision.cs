using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private EnemyStateManager stateManager;
    private Collider2D thisCollider;

    private float sightDist = 6.8f;

    private float hangTime = 2f;

    public float ang;

    [SerializeField]
    private LayerMask playerMask;
    private GameManager gameManager;

    int viewCounter;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = GetComponentInParent<EnemyStateManager>();
        thisCollider = GetComponentInParent<Collider2D>();
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
		EnemyState state = stateManager.getState();
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        for (float angle = -ang; angle <= ang; angle += ang / 10)
        {
            Vector3 vec = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            Debug.DrawRay(transform.position, transform.rotation * vec.normalized * sightDist, Color.cyan, 0.01f);

            hits.Add(Physics2D.Raycast(transform.position, transform.rotation * vec.normalized, 7, playerMask));
        }

        hits = hits.Where(hit => hit.collider != null && hit.collider.gameObject.CompareTag("Player")).ToList();

        if (hits.Count > 0)
        {
            EnemyPatrol patrolScript = GetComponent<EnemyPatrol>();
            // Debug.Log(hits[0].collider.gameObject);
            if (state != EnemyState.CAUGHT_PLAYER && state != EnemyState.STUNNED) { stateManager.setState(EnemyState.DISCOVERED_PLAYER); }
			stateManager.setPlayerPos(hits[0].collider.gameObject.transform);

            if (state != EnemyState.STUNNED) { viewCounter++; }
            // Debug.Log(viewCounter / 60);

            if (state != EnemyState.CAUGHT_PLAYER) { 
                patrolScript.setEnemyLight(1 + 0.5f * (viewCounter / 120.0f));
                gameManager.setDangerFill(viewCounter / 120f);
            }

			if (viewCounter / 60f > hangTime)
			{
				stateManager.setState(EnemyState.CAUGHT_PLAYER);
                patrolScript.toggleFollowSpeed();
			}
		}
		else { 
            viewCounter = 0; 
            if (state != EnemyState.CAUGHT_PLAYER && state != EnemyState.STUNNED) { stateManager.setState(EnemyState.PATROLLING); } 
            
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

   
}
