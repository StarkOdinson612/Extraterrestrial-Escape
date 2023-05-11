using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float shootDist = 1.0f;
    private LayerMask enemyMask;
	public Transform childTransform;

	public GameObject lightningPrefab;

	public float shootAngle;

	[SerializeField]
	int shots;

	private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyMask = LayerMask.GetMask("Enemy", "Default");
		shots = 3;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale == 0) { return; }
		for (float angle = -shootAngle; angle <= shootAngle; angle += 8)
		{
			Vector3 vec = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
			Debug.DrawRay(transform.position, childTransform.rotation * vec.normalized * shootDist, Color.red, 0.01f);

			// hits.Add(Physics2D.Raycast(transform.position, transform.rotation * vec.normalized, shootDist, enemyMask));
		}

		if (Input.GetMouseButtonDown(0))
        {
			if (shots == 0) { return; }

			shots--;
			gameManager.setAmmoFill(shots);

			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			for (float angle = -shootAngle; angle <= shootAngle; angle += 8)
			{
				Vector3 vec = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;

				hits.Add(Physics2D.Raycast(transform.position, childTransform.rotation * vec.normalized, shootDist, enemyMask));
			}


			hits = hits.Where(hit => hit.collider != null && hit.collider.gameObject.CompareTag("Enemy")).ToList();

			if (hits.Count > 0)
			{
				Debug.DrawLine(transform.position, hits[0].point, Color.green, 1.0f);


				GameObject t_obj = Instantiate(lightningPrefab, transform);
				GameObject hitEnemy = hits[0].collider.gameObject;
				try
				{ 
					StartCoroutine(hitEnemy.GetComponent<EnemyPatrol>().EnemyStunned());
					StartCoroutine(InstantiateLightning(1, t_obj, hitEnemy));
				}
				catch
				{
					
				}
			}
		}
    }

	IEnumerator InstantiateLightning(float seconds, GameObject lightningObj, GameObject tObj)
	{
		FollowTwoObj tScript = lightningObj.GetComponent<FollowTwoObj>();
		tScript.obj = gameObject;
		tScript.target = tObj;
		yield return new WaitForSecondsRealtime(seconds);
		Destroy(lightningObj);
	}
}
