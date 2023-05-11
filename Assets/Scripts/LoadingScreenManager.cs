using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject controlScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame() {  Application.Quit(); }

    public void PlayGame() { Debug.Log("Playing"); }

    public void ControlScreen() { Debug.Log("Control Screen Loading"); }
}
