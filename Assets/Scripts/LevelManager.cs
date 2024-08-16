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

    [Header("Level Management")]
    public List<GameObject> levelSpawners;

    private static List<GameObject> staticLevelSpawners;

    public static bool isGameOver = false;

    private AudioSource backgroundMusicSource;
    private static int levelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        StartCoroutine(RecordCheckpointAfterStart());
        
        staticLevelSpawners = new List<GameObject>(levelSpawners);
        
        SpawnLevelEnemies();
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
    
    private static void SpawnLevelEnemies()
    {
        staticLevelSpawners[levelIndex].GetComponent<EnemySpawner>().Spawn();
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

    public static void LevelBeat()
    {
        PlayerCheckpoint.RecordCheckpoint();
        
        levelIndex++;
        
        // Destroy old enemies by destroying the previous level spawner
        Destroy(staticLevelSpawners[levelIndex - 1]);

        SpawnLevelEnemies();
    }
}