using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool _isFinished;
    private bool _isMenuRequested;
    private bool _isGameOver;

    private void Awake()
    {
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
        EventManager.AddListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.AddListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
        EventManager.AddListener(Events.NET_ALL_REPAIRED, OnNetAllRepaired);
    }

    void Start()
    {
        EventManager.TriggerEvent(Events.LEVEL_START);
    }
    
    void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.LEVEL_FAILED, OnLevelFailed);
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.RemoveListener(Events.TRANSITION_CLOSE_FINISHED, OnTransitionCloseFinished);
        EventManager.RemoveListener(Events.NET_ALL_REPAIRED, OnNetAllRepaired);
    }

    void OnLevelStart()
    {
        GameState.Time = 60;
        
        EventManager.TriggerEvent(Events.TRANSITION_OPEN);
    }
    
    void OnLevelFailed()
    {
        if (GameState.Lives <= 0)
            _isGameOver = true;
        
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    void OnLevelFinished()
    {
        _isFinished = true;

        StartCoroutine(WaitAndClose());
    }

    IEnumerator WaitAndClose()
    {
        yield return new WaitForSeconds(2f);
        
        EventManager.TriggerEvent(Events.TRANSITION_CLOSE);
    }

    void OnTransitionCloseFinished()
    {
        if (_isFinished)
            LoadNextLevel();
        else if (_isMenuRequested)
            ShowMenu();
        else if (_isGameOver)
            ShowGameOver();
        else
            Restart();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ShowMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    void ShowGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    
    void LoadNextLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex + 1);
    }
    
    void OnNetAllRepaired()
    {
        // all are repaired, we can switch to next level
        EventManager.TriggerEvent(Events.LEVEL_FINISHED);
    }
}
