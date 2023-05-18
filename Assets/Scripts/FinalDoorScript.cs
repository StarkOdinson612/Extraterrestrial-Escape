using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorScript : MonoBehaviour
{
    public Sprite greenDoor;

    public AudioSource auth;
    public AudioSource rej;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player")) return;

        GameObject player = collision.gameObject;
        bool hasKey = player.GetComponent<PlayerInteractions>().getHasKeyCard();

        if (hasKey)
        {
            auth.Play();
            GetComponent<SpriteRenderer>().sprite = greenDoor;
            StartCoroutine(EndGame());
        }
        else
        {
            rej.Play();
        }
	}

    IEnumerator EndGame()
    {
        Time.timeScale = 0;
        yield return new WaitForSeconds(1);
        GameObject.Find("GameManager").GetComponent<GameManager>().GameWin();
    }
}
