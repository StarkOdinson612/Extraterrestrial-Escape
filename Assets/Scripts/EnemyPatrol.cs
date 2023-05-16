using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points;
    private int currentDestPoint;

    public Animator animator;

    private EnemyStateManager stateManager;

    public Light2D enemyLight;

    public Vector3 dir;

    public float timeDelay = 1f;

    public float moveSpeed = 1.5f;

    public float stunDuration = 7;

    private Rigidbody2D rb;

    public float followSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (points.Length != 0)
        {
            currentDestPoint = 0;
        }
        else { return; }

        transform.up = points[currentDestPoint].position - transform.position;

        stateManager = GetComponent<EnemyStateManager>();
        stateManager.setState(EnemyState.PATROLLING);
    }

	// Update is called once per frame
	void Update()
    {
        animator.SetBool("isTased", false);
        animator.SetFloat("rotation", transform.eulerAngles.z);
        EnemyState state = stateManager.getState();
        if (state == EnemyState.PATROLLING)
        {
            animator.SetFloat("move", 1);
            animator.SetFloat("rotationalV", 1);
            dir = points[currentDestPoint].position - transform.position;
            float dirAngle = GetAngleFromVectorFloat(dir);

            Quaternion rotDir = Quaternion.Euler(new Vector3(0, 0, dirAngle - transform.rotation.z - 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, 4 * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, points[currentDestPoint].position) < 0.2f)
            {
                StartCoroutine(NextTargetLocation());
            }
            
            if (stateManager.getState() == EnemyState.PATROLLING)
            {
                transform.position = Vector2.MoveTowards(transform.position, points[currentDestPoint].position, moveSpeed * Time.deltaTime);
            }
        }
        else if (state == EnemyState.DISCOVERED_PLAYER || state == EnemyState.CAUGHT_PLAYER)
        {
            animator.SetFloat("move", 0);
            animator.SetFloat("rotationalV", 0);
            float dirAngle = GetAngleFromVectorFloat(stateManager.getPlayerPos().position - transform.position);

			Quaternion rotDir = Quaternion.Euler(new Vector3(0, 0, dirAngle - transform.rotation.z - 90));
			transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, followSpeed * Time.deltaTime);
		}
        else if (state == EnemyState.STUNNED)
        {
            animator.SetBool("isTased", true);
            animator.SetFloat("move", -1);
            animator.SetFloat("rotationalV", -1);
            //Debug.Log("Detected Stun State");
            enemyLight.intensity = Mathf.Lerp(enemyLight.intensity, 0.2f, 0.01f);
        }
        else if (state == EnemyState.STOPPED)
        {
            animator.SetFloat("move", 0);
            animator.SetFloat("rotationalV", 0);
            Debug.Log("Stopped");
        }
    }

    public void toggleFollowSpeed()
    {
        followSpeed = 4;
    }

    IEnumerator NextTargetLocation()
    {
        if (stateManager.getState() != EnemyState.STUNNED)
        {
            stateManager.setState(EnemyState.STOPPED);

            if (currentDestPoint < points.Length - 1)
            {
                currentDestPoint++;
            }
            else if (currentDestPoint == points.Length - 1)
            {
                currentDestPoint = 0;
            }
            else
            {
                yield break;
            }

            stateManager.setState(EnemyState.PATROLLING);
        }
        else 
        {
            
        }
    }

    public void setEnemyLight(float intensity) { enemyLight.intensity = intensity; }
    
    public IEnumerator EnemyStunned()
    {
        if (stateManager.getState() == EnemyState.CAUGHT_PLAYER) { yield break; }
        stateManager.setState(EnemyState.STUNNED);
        yield return new WaitForSecondsRealtime(stunDuration);
		enemyLight.intensity = 0.5f;
		yield return new WaitForSecondsRealtime(0.2f);
		enemyLight.intensity = 1.75f;
		yield return new WaitForSecondsRealtime(0.2f);
		enemyLight.intensity = 1.5f;
		yield return new WaitForSecondsRealtime(0.2f);
		enemyLight.intensity = 0.25f;
        yield return new WaitForSecondsRealtime(0.2f);
        enemyLight.intensity = 1.7f;
        yield return new WaitForSecondsRealtime(0.4f);
		stateManager.setState(EnemyState.PATROLLING);
        enemyLight.intensity = 3;
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
