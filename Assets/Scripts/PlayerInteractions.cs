using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField]
    private bool hasKeyCard;
    [SerializeField]
    private GameObject keyCardObj = null;

    public GameObject keyCardUIParent;

    

    // Start is called before the first frame update
    void Start()
    {
        hasKeyCard = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale == 0) { return; }
		if (keyCardUIParent.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(keyCardObj);
                hasKeyCard = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("KeyCard"))
        {
            keyCardUIParent.SetActive(true);
            keyCardObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("KeyCard"))
        {
            StartCoroutine(waitForTimeCustom(0.3f));
            keyCardUIParent.SetActive(false);
        }
    }

    public bool getHasKeyCard() {  return hasKeyCard; }

    IEnumerator waitForTimeCustom(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }
}
