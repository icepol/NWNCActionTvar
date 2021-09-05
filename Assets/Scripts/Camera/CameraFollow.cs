using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Rigidbody2D _targetBody;
    private Transform _target;
    private Vector2 _velocity;

    [SerializeField] private float smoothX;
    [SerializeField] private float smoothY;
    [SerializeField] private float overflow;

    [SerializeField] private float offsetY;
    
    [SerializeField] private bool checkMinMax;

    [SerializeField] private float stopFollowingDelay = 1f;

    float _xMin;
    float _xMax;

    float _yMin;
    float _yMax;

    private float _width;
    private float _height;

    private float _z;

    private bool _isFollowing;

    private void Awake()
    {
        _isFollowing = false;
        
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.AddListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.AddListener(Events.CAMERA_START_FOLLOWING, OnCameraStartFollowing);
        EventManager.AddListener(Events.CAMERA_STOP_FOLLOWING, OnCameraStopFollowing);
    }

    private void Start()
    {
        _height = Camera.main.orthographicSize * 2f;
        _width = _height * Camera.main.aspect;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        _target = player.transform;
        _targetBody = player.GetComponent<Rigidbody2D>();

        _z = transform.position.z;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.BOUNDARIES_BOTTOM_LEFT, OnBoundariesBottomLeft);
        EventManager.RemoveListener(Events.BOUNDARIES_TOP_RIGHT, OnBoundariesTopRight);
        EventManager.RemoveListener(Events.CAMERA_START_FOLLOWING, OnCameraStartFollowing);
        EventManager.RemoveListener(Events.CAMERA_STOP_FOLLOWING, OnCameraStopFollowing);
    }

    private void Update() {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (!_isFollowing) return;
        if (!_target) return;
        
        Vector2 currentPosition = transform.position;
        
        Vector2 targetPosition = _target.position;
        Vector2 targetVelocity = _targetBody.velocity;

        float targetX = targetPosition.x - targetVelocity.x * Time.deltaTime;
        float targetY = targetPosition.y - targetVelocity.y * Time.deltaTime + offsetY;

        float x = Mathf.SmoothDamp(currentPosition.x, targetX, ref _velocity.x, smoothX);
        float y = Mathf.SmoothDamp(currentPosition.y, targetY, ref _velocity.y, smoothY);

        if (checkMinMax)
        {
            x = Mathf.Clamp(x, _xMin, _xMax);
            y = Mathf.Clamp(y ,_yMin, _yMax);
        }

        transform.position = new Vector3(Round(x), Round(y), _z);
    }

    float Round(float value)
    {
        return (int) (value * 50) / 50f;
    }

    void OnPlayerDied()
    {
        StartCoroutine(WaitAndStopFollowing());
    }

    IEnumerator WaitAndStopFollowing()
    {
        yield return new WaitForSeconds(stopFollowingDelay);
        
        _isFollowing = false;
    }

    void OnBoundariesBottomLeft(Vector3 vector)
    {
        _xMin = vector.x + _width * 0.5f + overflow;
        _xMin = _xMin < 0 ? _xMin : 0;
        
        _yMin = vector.y + _height * 0.5f + overflow;
        _yMin = _yMin < 0 ? _yMin : 0;
    }

    void OnBoundariesTopRight(Vector3 vector)
    {
        _xMax = vector.x - _width * 0.5f - overflow;
        _xMax = _xMax > 0 ? _xMax : 0;
        
        _yMax = vector.y - _height * 0.5f - overflow;
        _yMax = _yMax > 0 ? _yMax : 0;
    }

    void OnCameraStartFollowing()
    {
        _isFollowing = true;
    }
    
    void OnCameraStopFollowing()
    {
        _isFollowing = false;
    }
}