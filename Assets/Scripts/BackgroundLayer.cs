using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    [SerializeField] private float speedX = 0;
    [SerializeField] private float speedY = 0;

    private Camera _camera;
    private Vector3 _originalPosition;

    private void Start()
    {
        _camera = Camera.main;
        _originalPosition = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = _originalPosition + new Vector3(
            _camera.transform.position.x * speedX, 
            _camera.transform.position.y * speedY,
            0);
    }
}
