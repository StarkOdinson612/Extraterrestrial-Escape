using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLook : MonoBehaviour
{
    public Transform lookIndicator;
    public Camera cam;

    private Animator playerAnimator;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Cursor.lockState = CursorLockMode.Confined;
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Time.timeScale == 0) { return; }
        Vector3 mousePos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 shootDir = mousePos - new Vector3(transform.position.x, transform.position.y, 0);

        float angleBetween = AngleBetweenVector3(transform.position, mousePos);

        playerAnimator.SetFloat("FlashlightAngle", -angleBetween < 0 ? 270 + 90 - Mathf.Abs(angleBetween) : -angleBetween);


		lookIndicator.SetLocalPositionAndRotation(new Vector3(0,0,0).normalized, Quaternion.Euler(new Vector3(0,0,angleBetween)));
        
    }

    private float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        Vector3 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector3.Angle(Vector3.right, diference) * sign - 90;
    }
}
