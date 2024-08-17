using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;
    public Text objectiveText; // New variable for objective text

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    [Header("Level Management")]
    public List<GameObject> levelSpawners;

    private static List<GameObject> staticLevelSpawners;

    public static bool isGameOver = false;

    private AudioSource backgroundMusicSource;
    private static int levelIndex = 0;

    [System.Serializable]
    public class Objective
    {
        public string text;
        public GameObject locationObject;
    }

    [Header("Objectives")]
    public List<Objective> objectives;
    private static int currentObjectiveIndex = 0;

    void Start()
    {
        isGameOver = false;
        StartCoroutine(RecordCheckpointAfterStart());
        
        staticLevelSpawners = new List<GameObject>(levelSpawners);
        
        SpawnLevelEnemies();
        DisplayCurrentObjective();
    }

    private IEnumerator RecordCheckpointAfterStart()
    {
        yield return new WaitForEndOfFrame();
        PlayerCheckpoint.RecordCheckpoint();
    }

    void Update()
    {
        // Update logic here if needed
    }
    
    private static void SpawnLevelEnemies()
    {
        staticLevelSpawners[levelIndex].GetComponent<EnemySpawner>().Spawn();
    }

    public void GameWon()
    {
        PostProcessingManager.EnableEffects(0.5f);

        gameText.text = "YOU WON!";
        gameText.gameObject.SetActive(true);

        PlayerMovementController.locked = true;
        PlayerLookController.locked = true;

        Invoke("ReturnToMainMenu", 2);
    }


    public void LevelLost()
    {
        PostProcessingManager.EnableEffects(0.5f);

        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        PlayerMovementController.locked = true;
        PlayerLookController.locked = true;

        Invoke("RestartAtCheckpoint", 2);
    }

    public void RestartAtCheckpoint()
    {
        PlayerCheckpoint.RevertToCheckpoint();
        PostProcessingManager.DisableEffects(0.05f);
        gameText.gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public static void LevelBeat()
    {
        PlayerCheckpoint.RecordCheckpoint();
        
        levelIndex++;
        currentObjectiveIndex++;
        
        Destroy(staticLevelSpawners[levelIndex - 1]);

        SpawnLevelEnemies();

        LevelManager instance = FindObjectOfType<LevelManager>();
        if (instance != null)
        {
            instance.DisplayCurrentObjective();
        }
    }

    // Updated methods for objective management
    public static string GetCurrentObjectiveText()
    {
        LevelManager instance = FindObjectOfType<LevelManager>();
        if (instance != null && currentObjectiveIndex < instance.objectives.Count)
        {
            return instance.objectives[currentObjectiveIndex].text;
        }
        return "No current objective";
    }

    public static GameObject GetCurrentObjectiveLocationObject()
    {
        LevelManager instance = FindObjectOfType<LevelManager>();
        if (instance != null && currentObjectiveIndex < instance.objectives.Count)
        {
            return instance.objectives[currentObjectiveIndex].locationObject;
        }
        return null;
    }

    public static Vector3 GetCurrentObjectiveLocation()
    {
        GameObject locationObject = GetCurrentObjectiveLocationObject();
        return locationObject != null ? locationObject.transform.position : Vector3.zero;
    }

    public static void AdvanceToNextObjective()
    {
        currentObjectiveIndex++;
        LevelManager instance = FindObjectOfType<LevelManager>();
        if (instance != null)
        {
            if (currentObjectiveIndex >= instance.objectives.Count)
            {
                instance.GameWon();
            }
            instance.DisplayCurrentObjective();
        }
    }

    private void DisplayCurrentObjective()
    {
        Debug.Log("Displaying current objective");
        if (objectiveText != null && currentObjectiveIndex < objectives.Count)
        {
            Debug.Log("Starting coroutine");
            StartCoroutine(FadeObjectiveText(objectives[currentObjectiveIndex].text));
        }
    }

    private IEnumerator FadeObjectiveText(string text)
    {
        Debug.Log("Displaying objective: " + text);
        objectiveText.text = text;
        objectiveText.gameObject.SetActive(true);
        Debug.Log("Objective text active: " + objectiveText.gameObject.activeSelf);

        // Fade in
        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, t);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        // Fade out
        for (float t = 1; t > 0; t -= Time.deltaTime / 0.5f)
        {
            objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, t);
            yield return null;
        }

        objectiveText.gameObject.SetActive(false);
    }
}