using UnityEngine;

public class PlayerRope : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Player _player;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GetComponentInParent<Player>();
    }

    private void LateUpdate()
    {
        Vector3 start = transform.position;
        Vector3 end = start + (_player.IsFacingRight ? Vector3.right : Vector3.left) * 4f;
        
        Debug.DrawLine(start, end, Color.red);
    }

    public void Show()
    {
        EventManager.TriggerEvent(Events.PLAYER_SHOOT);
        
        _spriteRenderer.enabled = true;

        Vector3 start = transform.position;
        Vector3 end = start + (_player.IsFacingRight ? Vector3.right : Vector3.left) * 4f;
        
        RaycastHit2D[] hit2Ds = Physics2D.LinecastAll(start, end);

        foreach (RaycastHit2D hit2D in hit2Ds)
        {
            Enemy enemy = hit2D.collider.GetComponent<Enemy>();
            
            if (!enemy)
                continue;

            if (enemy.State == Enemy.EnemyState.Hit || enemy.State == Enemy.EnemyState.Dead)
                continue;
        
            enemy.ChangeState(Enemy.EnemyState.Hit);
            
            return;
        }
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }
}
