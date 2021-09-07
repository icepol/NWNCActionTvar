using UnityEngine;

public class ScoreBalloon : MonoBehaviour
{
    [SerializeField] private float positionOffset = 1;
    
    private void Start()
    {
        transform.position += Vector3.up * positionOffset + Vector3.back;
    }

    public void SetScore(int score)
    {
        Debug.Log("score bubble");
        foreach (TextMesh textMesh in GetComponentsInChildren<TextMesh>())
            textMesh.text = $"+{score}";
    }

    public void Destroy()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
}