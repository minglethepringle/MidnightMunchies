using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;
    public Text objectiveText;

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    [Header("Level Management")]
    public List<GameObject> levelSpawners;

    // public static List<GameObject> staticLevelSpawners;

    public static bool isGameOver = false;

    private AudioSource backgroundMusicSource;
    public static int levelIndex = 0;

    [System.Serializable]
    public class Objective
    {
        public string text;
        public GameObject locationObject;
        public GameObject doorLocation;
    }

    [Header("Objectives")]
    public List<Objective> objectives;
    public static int currentObjectiveIndex = 0;

    [Header("Objective Indicator")]
    public GameObject cylinderPrefab;
    private GameObject currentCylinder;
    public float cylinderHeight = 30f;
    public float fadeStartDistance = 10f;
    public float fadeEndDistance = 2f;
    private bool cylinderFadedOut = false;

    void Start()
    {
        isGameOver = false;
        StartCoroutine(RecordCheckpointAfterStart());
        
        // staticLevelSpawners = new List<GameObject>(levelSpawners);
        
        // SpawnLevelEnemies();
        DisplayCurrentObjective();
        CreateObjectiveCylinder();
        
        PlayerLookController.SetMouseSens(PlayerPrefs.GetInt("Sensitivity", 5) * 40f);
        PlayerLookController.SetVolume(PlayerPrefs.GetInt("Volume", 10) / 10f);
    }

    private IEnumerator RecordCheckpointAfterStart()
    {
        yield return new WaitForEndOfFrame();
        PlayerCheckpoint.RecordCheckpoint();
    }

    void Update()
    {
        UpdateCylinderVisibility();
    }
    
    public static void SpawnLevelEnemies()
    {
        // staticLevelSpawners[levelIndex].GetComponent<EnemySpawner>().Spawn();
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
        PlayerHealth.SetCurrentHealth(100);
        
        levelIndex++;
        currentObjectiveIndex++;

        LevelManager instance = FindObjectOfType<LevelManager>();
        if (levelIndex >= 4)//staticLevelSpawners.Count)
        {
            if (instance != null)
            {
                instance.GameWon();
            }
            return;
        }
        
        // Destroy(staticLevelSpawners[levelIndex - 1]);

        // SpawnLevelEnemies();

        if (instance != null)
        {
            instance.DisplayCurrentObjective();
        }
        
        PlayerCheckpoint.RecordCheckpoint();
    }

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
            else
            {
                instance.DisplayCurrentObjective();
            }
        }
    }

    private void DisplayCurrentObjective()
    {
        if (objectiveText != null && currentObjectiveIndex < objectives.Count)
        {
            StartCoroutine(FadeObjectiveText(objectives[currentObjectiveIndex].text));
        }
        CreateObjectiveCylinder();
    }

    private IEnumerator FadeObjectiveText(string text)
    {
        objectiveText.text = text;
        objectiveText.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, t);
            yield return null;
        }

        yield return new WaitForSeconds(10);

        for (float t = 1; t > 0; t -= Time.deltaTime / 0.5f)
        {
            objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, t);
            yield return null;
        }

        objectiveText.gameObject.SetActive(false);
    }

    private void CreateObjectiveCylinder()
    {
        if (currentCylinder != null)
        {
            Destroy(currentCylinder);
        }

        cylinderFadedOut = false;

        if (currentObjectiveIndex < objectives.Count && objectives[currentObjectiveIndex].doorLocation != null)
        {
            Vector3 doorPosition = objectives[currentObjectiveIndex].doorLocation.transform.position;
            currentCylinder = Instantiate(cylinderPrefab, doorPosition + Vector3.up * (cylinderHeight / 2), Quaternion.identity);
            currentCylinder.transform.localScale = new Vector3(1, cylinderHeight, 1);
        }
    }

    private void UpdateCylinderVisibility()
    {
        if (currentCylinder != null && objectives[currentObjectiveIndex].doorLocation != null && !cylinderFadedOut)
        {
            Vector3 doorPosition = objectives[currentObjectiveIndex].doorLocation.transform.position;
            float distanceToDoor = Vector3.Distance(Camera.main.transform.position, doorPosition);
            
            float alpha = Mathf.Clamp01((distanceToDoor - fadeEndDistance) / (fadeStartDistance - fadeEndDistance));
            
            Renderer cylinderRenderer = currentCylinder.GetComponent<Renderer>();
            if (cylinderRenderer != null && cylinderRenderer.material != null)
            {
                Color cylinderColor = cylinderRenderer.material.color;
                cylinderRenderer.material.color = new Color(cylinderColor.r, cylinderColor.g, cylinderColor.b, alpha);
                
                if (alpha <= 0)
                {
                    cylinderFadedOut = true;
                    Destroy(currentCylinder);
                    currentCylinder = null;
                }
            }
        }
    }
}