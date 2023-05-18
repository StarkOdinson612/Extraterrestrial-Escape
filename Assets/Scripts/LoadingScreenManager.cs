using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void PlayGame() { SceneManager.LoadScene("SampleScene"); }

    public void ControlScreen() { controlScreen.SetActive(true); }

    public void CloseControlScreen() {  controlScreen.SetActive(false); }  
}
