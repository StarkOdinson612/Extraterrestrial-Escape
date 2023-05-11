using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    private RectTransform thisTransform;

    private Vector3 offset = new Vector3(40,20,0);

    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<RectTransform>();
        playerTransform = transform.parent.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pPos = Camera.main.WorldToScreenPoint(playerTransform.position);
        thisTransform.SetPositionAndRotation(pPos + offset, Quaternion.identity);
    }
}
