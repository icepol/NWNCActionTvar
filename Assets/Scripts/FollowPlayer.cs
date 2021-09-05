using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float smooth = 0.25f;
    [SerializeField] private float maxSpeed = 1f;
    
    private Player _player;
    private Vector3 _currentVelocity;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (_player == null) return;

        Vector3 targetPosition = new Vector3(
            _player.transform.position.x,
            transform.position.y,
            transform.position.z);
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smooth, maxSpeed);
    }
}