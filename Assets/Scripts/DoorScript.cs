using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

	private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
		source= GetComponent<AudioSource>();	
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player")) return;
		Debug.Log("Collision enter");
		animator.SetTrigger("Open");
		source.Play();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player")) return;
		Debug.Log("Collision exit");
		animator.SetTrigger("Closed");
		source.Play();
	}
}
