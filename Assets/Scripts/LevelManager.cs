using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public Text gameText;

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    public static bool isGameOver = false;

    public string nextLevel;


    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        StartCoroutine(RecordCheckpointAfterStart());
    }

    private IEnumerator RecordCheckpointAfterStart()
    {
        yield return new WaitForEndOfFrame();

        PlayerCheckpoint.RecordCheckpoint();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LevelLost()
    {
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        //AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);
        PlayerMovementController.locked = true;
        PlayerLookController.locked = true;

        Invoke("RestartAtCheckpoint", 2);
        

    }

    public void RestartAtCheckpoint()
    {
        PlayerCheckpoint.RevertToCheckpoint();
        gameText.gameObject.SetActive(false);

    }

    /*public void LevelBeat()
    {
        Debug.Log("level complete");
        isGameOver = true;
        gameText.text = "FOOD ORDERED!";
        gameText.gameObject.SetActive(true);

        //AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

        if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }*/

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SaveCheckPoint()
    {
        //store player items/dollars
    }
}