using UnityEngine;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    private bool _isTimerTicking;
    
    void OnEnable()
    {
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.TIME_OUT, OnTimeOut);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTimerTicking) return;

        GameState.Time -= Time.deltaTime;

        if ((int)GameState.Time <= 0)
            EventManager.TriggerEvent(Events.TIME_OUT);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.TIME_OUT, OnTimeOut);
    }

    private void OnLevelStart()
    {
        _isTimerTicking = true;
    }

    private void OnLevelFinished()
    {
        _isTimerTicking = false;
    }

    private void OnPlayerDied()
    {
        _isTimerTicking = false;
    }
    
    private void OnTimeOut()
    {
        _isTimerTicking = false;
    }
}
