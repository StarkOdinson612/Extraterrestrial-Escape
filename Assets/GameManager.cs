using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isPaused;

    private bool isOver;

    public GameObject pauseParent;
    public GameObject loseScreen;

    public Image danger;
    public Image ammo;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {

        Time.timeScale = isOver ? 0 : isPaused ? 0 : 1;

        if (Input.GetKeyUp(KeyCode.T) && !isOver)
        {
            TogglePause();
        }
    }

    public void GameOver()
    {
        isOver = true;
        loseScreen.SetActive(true);
    }

    private void TogglePause()
    { 
        isPaused= !isPaused;
        pauseParent.SetActive(!pauseParent.activeInHierarchy);
    }

    public void Play()
    {
        isPaused = false;
        pauseParent.SetActive(false);
    }

    public void Quit()
    {
        isPaused = false;
        SceneManager.LoadScene("MainScreen");
    }

    public void setDangerFill(float fill)
    {
        danger.fillAmount = 0.98f * fill;
    }

    public void resetDangerFill()
    {
        danger.fillAmount = Mathf.Lerp(danger.fillAmount, 0, .7f * Time.deltaTime * (1 / (danger.fillAmount - 0)));
    }

    public void setAmmoFill(int count)
    {
        ammo.fillAmount = count / 3f;
    }

    public bool isGamePaused() { return isPaused; }
}
