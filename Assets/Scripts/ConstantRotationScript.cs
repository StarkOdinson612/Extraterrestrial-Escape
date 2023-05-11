using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale == 0) { return; }
		transform.rotation = Quaternion.identity;
    }
}
