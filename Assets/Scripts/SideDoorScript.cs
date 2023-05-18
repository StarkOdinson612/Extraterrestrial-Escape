using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoorScript : MonoBehaviour
{
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		source.Play();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		source.Play();
	}
}
