using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private bool onGround;

    [SerializeField]
    private Rigidbody2D playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 move = Vector3.ClampMagnitude(new Vector3(inputX * moveSpeed, inputY * moveSpeed, 0), moveSpeed);

        playerRb.velocity = move;
    }

    private void FixedUpdate()
    {
        
    }
}
