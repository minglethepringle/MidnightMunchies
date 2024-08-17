using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehavior : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pausePanel;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public AudioClip sfx;

    private void Start()
    {
        // On start / when the game is loaded, make sure the game is not paused
        isGamePaused = false;
        ResumeGame(); // to reset the game state
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position, 2f);

            if (isGamePaused)
            {
                if (pauseMenu.activeSelf)
                {
                    ResumeGame();
                }
                else
                {
                    pauseMenu.SetActive(true);
                    settingsMenu.SetActive(false);
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        PostProcessingManager.EnableEffects(0.03f);
        isGamePaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);

        PlayerMovementController.locked = true;
        PlayerLookController.locked = true;
        PlayerLookController.HideWeapons();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        PostProcessingManager.DisableEffects(0.05f);
        isGamePaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        PlayerMovementController.locked = false;
        PlayerLookController.locked = false;
        PlayerLookController.ShowWeapons();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
