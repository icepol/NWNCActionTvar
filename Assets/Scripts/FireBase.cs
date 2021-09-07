using UnityEngine;

public class FireBase : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private ScoreBalloon scoreBalloonPrefab;

    private bool _isAvailable;
    
    void Start()
    {
        arrow.SetActive(false);
        
        EventManager.AddListener(Events.TAKE_STAKE, OnTakeSteak);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TAKE_STAKE, OnTakeSteak);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player _player = other.GetComponent<Player>();
        
        if (_player && _isAvailable)
        {
            _isAvailable = false;
            
            arrow.SetActive(false);
            
            GameState.Score += 100;
            
            var scoreBalloon = Instantiate(scoreBalloonPrefab, transform.position, Quaternion.identity);
            scoreBalloon.SetScore(100);
            
            EventManager.TriggerEvent(Events.LEVEL_FINISHED);
        }
    }

    private void OnTakeSteak()
    {
        _isAvailable = true;

        arrow.SetActive(true);
    }
}
