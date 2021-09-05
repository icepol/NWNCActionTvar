using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDeadZone deadZone = other.GetComponent<IDeadZone>();
        deadZone?.OnDeadZone();
    }
}
